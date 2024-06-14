using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class PlayerLocomotions : MonoBehaviour
    {
        // CameraHandler ����
        CameraHandler cameraHandler;
        // PlayerManager ����
        PlayerManager playerManager;
        // PlayerStats ����
        PlayerStats playerStats;
        // ī�޶� ��ġ ����
        Transform cameraTransform;
        // InputHandler ����
        InputHandler inputHandler;
        public Vector3 moveDirection; // �̵� ����

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public PlayerAnimatorManager animatorHandler;

        // Rigidbody ����
        public new Rigidbody rigidbody;
        // �Ϲ� ī�޶� ����
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = -0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5f; // �̵� �ӵ�
        [SerializeField] float walkingSpeed = 1f; // �ȱ� �ӵ�
        [SerializeField] float sprintSpeed = 7f; // �޸��� �ӵ�
        [SerializeField] float rotationSpeed = 10f; // ȸ�� �ӵ�
        [SerializeField] float fallingSpeed = 45f;

        [Header("Stamina Costs")]
        [SerializeField] int rollStaminaCost = 15;
        [SerializeField] int backstepStaminaCost = 12;
        [SerializeField] int sprintStaminaCost = 1;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        public float jumpSpeed = 2.5f;

        // public bool isSprinting; -> PlayerManager.cs

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>(); // CameraHandler ������Ʈ ����
            playerManager = GetComponent<PlayerManager>(); // PlayerManager ������Ʈ �ʱ�ȭ
            playerStats = GetComponent<PlayerStats>(); // PlayerStats ������Ʈ �ʱ�ȭ
            rigidbody = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �ʱ�ȭ
            inputHandler = GetComponent<InputHandler>(); // InputHandler ������Ʈ �ʱ�ȭ
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler ������Ʈ �ʱ�ȭ
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraTransform = Camera.main.transform; // ī�޶� ��ġ ���� �ʱ�ȭ
            playerTransform = transform; // �÷��̾� ��ġ ���� �ʱ�ȭ
            animatorHandler.Initialized();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 6 | 1 << 9); // 8 << 11
        }

        #region Movement
        Vector3 normalVector; // ����
        Vector3 targetPosition; // Ÿ�� ��ġ

        // ȸ�� ó��
        public void HandleRotation(float delta)
        {
            // canRotate ���� �� HandleRotation �޼ҵ� ����
            if (animatorHandler.canRotate)
            {
                // ������ ����ִ� ���
                if (inputHandler.lockOnFlag)
                {
                    // ������Ʈ ���°ų�, ������ ������ ���
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        // ī�޶� ���⿡ ���� ��� ������ ���
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0f;

                        // ��� ������ 0�� ���
                        if (targetDirection == Vector3.zero)
                        {
                            // ���� �������� ����
                            targetDirection = transform.forward;
                        }

                        // ��� ������ �ٶ󺸴� ȸ���� ���
                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        // ���� ȸ���� ��� ȸ�� ���̸� �ε巴�� ����
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        // ���� ��� ��� ��ġ�� ������� ȸ�� ���� ���
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                        rotationDirection.y = 0f;
                        rotationDirection.Normalize();
                        // ��� ������ �ٶ󺸴� ȸ�� ���
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        // ���� ȸ���� ��� ȸ�� ���̸� �ε巴�� ����
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    // Ÿ�� ���� ���
                    Vector3 targetDirection = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount; // inputHanlder���� �̵��Ÿ��� �����´�

                    // camera�� ���� ���Ϳ� �������� ���ؼ� targetDirection ���
                    targetDirection = cameraTransform.forward * inputHandler.vertical;
                    // camera�� ������ ���Ϳ� ������ ���ؼ� targetDirection�� ���Ѵ�
                    targetDirection += cameraTransform.right * inputHandler.horizontal;

                    targetDirection.Normalize(); // targetDirection�� ����ȭ�Ͽ� ���� ���ͷ� �����
                    targetDirection.y = 0f; // ���� ���� ����(���� �̵�X)

                    // Ÿ�� ������ 0�� ��� Player�� ������ ����
                    if (targetDirection == Vector3.zero)
                    {
                        // playerTransform�� ������ targetDirection���� ����
                        targetDirection = playerTransform.forward;
                    }

                    float rs = rotationSpeed; // ȸ�� �ӵ�

                    // [Ÿ�� ȸ�� ���]
                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);

                    playerTransform.rotation = targetRotation; // Player ȸ�� ����
                }
            }
        }

        // �̵� ó��
        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag) return;

            if (playerManager.isInteracting) return;

            // [�̵� ���� ���]
            moveDirection = cameraTransform.forward * inputHandler.vertical;
            moveDirection += cameraTransform.right * inputHandler.horizontal;
            moveDirection.Normalize(); // moveDirection�� ����ȭ�Ͽ� ���� ���ͷ� �����
            moveDirection.y = 0f; // ���� ���� ����(���� �̵�X)

            // �ӵ� ���
            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.TakeStaminaDamage(sprintStaminaCost); // ���¹̳� ����
            }
            // Sprint �̽��� �� moveDirection�� Speed�� ����
            else
            {
                if (inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            // �̵� ����� ���Ϳ� ���� �ӵ� ���
            // �̵� ������ ProjectOnPlane�� �����Ͽ� �ӵ� ���
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity; // ������ �ӵ��� rigidbody�� �ӵ��� ����

            // ������ �Ῡ�ְ�, ������Ʈ ���� �ƴ϶��
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                // ����, ���� �Է� ���� ������Ʈ ���¸� ������Ʈ
                animatorHandler.UpdateAnimatorValue(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                // �� ���� ���, �̵� ��� ������Ʈ ���¸� ������Ʈ
                animatorHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0f, playerManager.isSprinting);
            }
        }

        // Roll ó��
        public void HandleRollingAndSprinting(float delta)
        {
            // ��ȣ�ۿ� ���̸� ����
            if (animatorHandler.animator.GetBool("isInteracting")) return;
            // ���¹̳��� ������ ����
            if (playerStats.currentStamina <= 0) return;

            // Roll ���� �� Roll ó�� �Լ�
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraTransform.right * inputHandler.horizontal;

                // �̵� �Ÿ��� 0���� Ŭ �� Roll �ִϸ��̼� ���
                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayAnimation("Roll", true);
                    moveDirection.y = 0; // ���� ���� ����(���� �̵�X)
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    playerTransform.rotation = rollRotation; // playerTransform�� ȸ���� rollRotation���� ����
                    playerStats.TakeStaminaDamage(rollStaminaCost); // ���¹̳� ����
                }
                // �̵� �Ÿ��� 0���� ���� ��� BackStep �ִϸ��̼� ���
                else
                {
                    animatorHandler.PlayAnimation("Backstep", true);
                    playerStats.TakeStaminaDamage(backstepStaminaCost); // ���¹̳� ����
                }
            }
        }

        // �߶� ����
        public void HandleFalling(float delta, Vector4 moveDirection)
        {
            // Player�� ���� �ִ��� Ȯ��
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = playerTransform.position;
            // ���� ���� Ray�� �������� Player�� ��ġ�� �߰�
            origin.y += groundDetectionRayStartPoint;

            // Player ���濡�� ���� ���� Ray�� ���
            if (Physics.Raycast(origin, playerTransform.forward, out hit, 0.4f))
            {
                // Ray�� ���� ������ �̵� ������ 0���� ����
                moveDirection = Vector3.zero;
            }

            // Player�� ���߿� �ִ� ���
            if (playerManager.isInAir)
            {
                // �߷��� �����Ͽ� Player�� �Ʒ��� ����߸���
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                // �̵� ���⿡ ���� Player�� ������ �о��
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize(); // dir�� ����ȭ�Ͽ� ���� ���ͷ� ��ȯ
            // �̵� ���⿡ ���� Ray�� ������ ����
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = playerTransform.position;

            // ���� ��� ���� �ּ� �Ÿ� ǥ��
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            // ���� ��� ���� Ray �߻�
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                // Ray�� ���� ������ Player�� ���� �ִ� ������ ǥ��
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                // Player�� ���߿� �ִٸ�, ���� �ִϸ��̼� ���
                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            // Ray�� ���� ���� �ʾҴٸ�, Player�� ���� ���� ���� ���� ������ ǥ��
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                // Player�� ���߿� ���� �ʰ�, ��ȣ�ۿ� ���� �ƴ϶��, �߶� �ִϸ��̼� ���
                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayAnimation("Falling", true);
                    }

                    Vector3 velocity = rigidbody.velocity;
                    velocity.Normalize(); // velocity�� ����ȭ�Ͽ� ���� ���ͷ� ��ȯ
                    // Player�� �ӵ��� �����Ͽ� �������� �ӵ� ����
                    rigidbody.velocity = velocity * (movementSpeed / 2f);
                    playerManager.isInAir = true;
                }
            }

            // Player�� ���� �ִٸ�, �̵� ��ȣ�ۿ뿡 ���� Player�� ��ġ�� ����
            if (playerManager.isGrounded)
            {
                // Player�� ��ȣ�ۿ� ���̰ų� �̵��Ÿ��� 0���� ū ���
                if (playerManager.isInteracting || inputHandler.moveAmount > 0f)
                {
                    // Player�� ���� ��ġ���� ��ǥ ��ġ�� (�ε巴��)�̵�
                    // Time.deltaTime�� ����Ͽ� �������� �ӵ��� �̵�
                    playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, Time.deltaTime);
                }
                // �̵��Ÿ��� 0�� ���, Player�� ��ġ�� ��ǥ ��ġ�� ����
                else
                {
                    playerTransform.position = targetPosition;
                }
            }

            // Player�� ��ȣ�ۿ� ���̰ų� �̵��Ÿ��� 0���� ū ���
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                // �÷��̾��� ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵��մϴ�.
                // Time.deltaTime / 0.1f�� ����Ͽ� �̵� �ӵ��� �����մϴ�.
                playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            // �̵��Ÿ��� 0�� ���, Player�� ��ġ�� ��ǥ ��ġ�� ����
            else
            {
                playerTransform.position = targetPosition;
            }
        }

        // ���� ó��
        public void HandleJumping()
        {
            // Player�� ��ȣ�ۿ� ���� ��� ���� ���
            if (playerManager.isInteracting) return;
            // ���¹̳��� ������ ����
            if (playerStats.currentStamina <= 0) return;

            // ���� �Է� ��
            if (inputHandler.jumpInput)
            {
                // ī�޶��� ����� Player�� �������� �����Ͽ� ���� ������ ����
                moveDirection = cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraTransform.right * inputHandler.horizontal;
                // Jump �ִϸ��̼� ���
                animatorHandler.PlayAnimation("Jump", true);
                moveDirection.y = 0; // ���� ������ ����
                // ���� ������ ������� jumpRotation ���
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                // Player�� ȸ���� jumpRotation���� ����
                playerTransform.rotation = jumpRotation;

                // �����̰� �ִ� ���
                if (inputHandler.moveAmount > 0)
                {
                    
                }
            }
        }

        #endregion
    }
}