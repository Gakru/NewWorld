using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class PlayerLocomotions : MonoBehaviour
    {
        // CameraHandler 참조
        CameraHandler cameraHandler;
        // PlayerManager 참조
        PlayerManager playerManager;
        // PlayerStats 참조
        PlayerStats playerStats;
        // 카메라 위치 참조
        Transform cameraTransform;
        // InputHandler 참조
        InputHandler inputHandler;
        public Vector3 moveDirection; // 이동 방향

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public PlayerAnimatorManager animatorHandler;

        // Rigidbody 참조
        public new Rigidbody rigidbody;
        // 일반 카메라 참조
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = -0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5f; // 이동 속도
        [SerializeField] float walkingSpeed = 1f; // 걷기 속도
        [SerializeField] float sprintSpeed = 7f; // 달리기 속도
        [SerializeField] float rotationSpeed = 10f; // 회전 속도
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
            cameraHandler = FindObjectOfType<CameraHandler>(); // CameraHandler 컴포넌트 참조
            playerManager = GetComponent<PlayerManager>(); // PlayerManager 컴포넌트 초기화
            playerStats = GetComponent<PlayerStats>(); // PlayerStats 컴포넌트 초기화
            rigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 초기화
            inputHandler = GetComponent<InputHandler>(); // InputHandler 컴포넌트 초기화
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler 컴포넌트 초기화
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraTransform = Camera.main.transform; // 카메라 위치 참조 초기화
            playerTransform = transform; // 플레이어 위치 참조 초기화
            animatorHandler.Initialized();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 6 | 1 << 9); // 8 << 11
        }

        #region Movement
        Vector3 normalVector; // 벡터
        Vector3 targetPosition; // 타겟 위치

        // 회전 처리
        public void HandleRotation(float delta)
        {
            // canRotate 실행 시 HandleRotation 메소드 실행
            if (animatorHandler.canRotate)
            {
                // 시점이 잠겨있는 경우
                if (inputHandler.lockOnFlag)
                {
                    // 스프린트 상태거나, 구르는 상태인 경우
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        // 카메라 방향에 따라 대상 방향을 계산
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0f;

                        // 대상 방향이 0인 경우
                        if (targetDirection == Vector3.zero)
                        {
                            // 현재 방향으로 설정
                            targetDirection = transform.forward;
                        }

                        // 대상 방향을 바라보는 회전을 계산
                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        // 현재 회전과 대상 회전 사이를 부드럽게 보간
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        // 현재 잠금 대상 위치를 기반으로 회전 방향 계산
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                        rotationDirection.y = 0f;
                        rotationDirection.Normalize();
                        // 대상 방향을 바라보는 회전 계산
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        // 현재 회전과 대상 회전 사이를 부드럽게 보간
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    // 타겟 방향 계산
                    Vector3 targetDirection = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount; // inputHanlder에서 이동거리를 가져온다

                    // camera의 전방 벡터와 수직값을 곱해서 targetDirection 계산
                    targetDirection = cameraTransform.forward * inputHandler.vertical;
                    // camera의 오른쪽 벡터와 수평값을 곱해서 targetDirection에 더한다
                    targetDirection += cameraTransform.right * inputHandler.horizontal;

                    targetDirection.Normalize(); // targetDirection을 정규화하여 단위 벡터로 만든다
                    targetDirection.y = 0f; // 수직 방향 제거(수직 이동X)

                    // 타겟 방향이 0인 경우 Player의 앞으로 설정
                    if (targetDirection == Vector3.zero)
                    {
                        // playerTransform의 전방을 targetDirection으로 설정
                        targetDirection = playerTransform.forward;
                    }

                    float rs = rotationSpeed; // 회전 속도

                    // [타겟 회전 계산]
                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);

                    playerTransform.rotation = targetRotation; // Player 회전 설정
                }
            }
        }

        // 이동 처리
        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag) return;

            if (playerManager.isInteracting) return;

            // [이동 방향 계산]
            moveDirection = cameraTransform.forward * inputHandler.vertical;
            moveDirection += cameraTransform.right * inputHandler.horizontal;
            moveDirection.Normalize(); // moveDirection을 정규화하여 단위 벡터로 만든다
            moveDirection.y = 0f; // 수직 방향 제거(수직 이동X)

            // 속도 계산
            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.TakeStaminaDamage(sprintStaminaCost); // 스태미나 감소
            }
            // Sprint 미실행 시 moveDirection을 Speed로 변경
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

            // 이동 방향과 벡터에 따른 속도 계산
            // 이동 방향을 ProjectOnPlane에 투영하여 속도 계산
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity; // 투영된 속도을 rigidbody의 속도로 설정

            // 시점이 잠여있고, 스프린트 중이 아니라면
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                // 수직, 수평 입력 값과 스프린트 상태를 업데이트
                animatorHandler.UpdateAnimatorValue(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                // 그 외의 경우, 이동 양과 스프린트 상태를 업데이트
                animatorHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0f, playerManager.isSprinting);
            }
        }

        // Roll 처리
        public void HandleRollingAndSprinting(float delta)
        {
            // 상호작용 중이면 종료
            if (animatorHandler.animator.GetBool("isInteracting")) return;
            // 스태미나가 없으면 종료
            if (playerStats.currentStamina <= 0) return;

            // Roll 실행 시 Roll 처리 함수
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraTransform.right * inputHandler.horizontal;

                // 이동 거리가 0보다 클 시 Roll 애니메이션 재생
                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayAnimation("Roll", true);
                    moveDirection.y = 0; // 수직 방향 제거(수직 이동X)
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    playerTransform.rotation = rollRotation; // playerTransform의 회전을 rollRotation으로 설정
                    playerStats.TakeStaminaDamage(rollStaminaCost); // 스태미나 감소
                }
                // 이동 거리가 0보다 작을 경우 BackStep 애니메이션 재생
                else
                {
                    animatorHandler.PlayAnimation("Backstep", true);
                    playerStats.TakeStaminaDamage(backstepStaminaCost); // 스태미나 감소
                }
            }
        }

        // 추락 여부
        public void HandleFalling(float delta, Vector4 moveDirection)
        {
            // Player가 지상에 있는지 확인
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = playerTransform.position;
            // 지상 감시 Ray의 시작점을 Player의 위치에 추가
            origin.y += groundDetectionRayStartPoint;

            // Player 전방에서 땅을 향해 Ray를 쏜다
            if (Physics.Raycast(origin, playerTransform.forward, out hit, 0.4f))
            {
                // Ray가 지상에 닿으면 이동 방향을 0으로 설정
                moveDirection = Vector3.zero;
            }

            // Player가 공중에 있는 경우
            if (playerManager.isInAir)
            {
                // 중력을 적용하여 Player를 아래로 떨어뜨린다
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                // 이동 방향에 따라 Player를 앞으로 밀어낸다
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize(); // dir을 정규화하여 단위 벡터로 변환
            // 이동 방향에 따라 Ray의 시작점 조정
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = playerTransform.position;

            // 지상에 닿기 위한 최소 거리 표시
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            // 지상에 닿기 위한 Ray 발사
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                // Ray가 땅에 닿으면 Player가 지상에 있는 것으로 표기
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                // Player가 공중에 있다면, 착지 애니메이션 재생
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
            // Ray가 지상에 닿지 않았다면, Player는 현재 지상에 있지 않은 것으로 표기
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                // Player가 공중에 있지 않고, 상호작용 중이 아니라면, 추락 애니메이션 재생
                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayAnimation("Falling", true);
                    }

                    Vector3 velocity = rigidbody.velocity;
                    velocity.Normalize(); // velocity를 정규화하여 단위 벡터로 변환
                    // Player의 속도를 조정하여 떨어지는 속도 설정
                    rigidbody.velocity = velocity * (movementSpeed / 2f);
                    playerManager.isInAir = true;
                }
            }

            // Player가 지상에 있다면, 이동 상호작용에 따라 Player의 위치를 조정
            if (playerManager.isGrounded)
            {
                // Player가 상호작용 중이거나 이동거리가 0보다 큰 경우
                if (playerManager.isInteracting || inputHandler.moveAmount > 0f)
                {
                    // Player의 현재 위치에서 목표 위치로 (부드럽게)이동
                    // Time.deltaTime을 사용하여 독립적인 속도로 이동
                    playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, Time.deltaTime);
                }
                // 이동거리가 0인 경우, Player의 위치를 목표 위치로 설정
                else
                {
                    playerTransform.position = targetPosition;
                }
            }

            // Player가 상호작용 중이거나 이동거리가 0보다 큰 경우
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                // 플레이어의 현재 위치에서 목표 위치로 부드럽게 이동합니다.
                // Time.deltaTime / 0.1f를 사용하여 이동 속도를 조절합니다.
                playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            // 이동거리가 0인 경우, Player의 위치를 목표 위치로 설정
            else
            {
                playerTransform.position = targetPosition;
            }
        }

        // 점프 처리
        public void HandleJumping()
        {
            // Player가 상호작용 중인 경우 점프 취소
            if (playerManager.isInteracting) return;
            // 스태미나가 없으면 종료
            if (playerStats.currentStamina <= 0) return;

            // 점프 입력 시
            if (inputHandler.jumpInput)
            {
                // 카메라의 방향과 Player의 움직임을 결합하여 점프 방향을 결정
                moveDirection = cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraTransform.right * inputHandler.horizontal;
                // Jump 애니메이션 재생
                animatorHandler.PlayAnimation("Jump", true);
                moveDirection.y = 0; // 수직 움직임 제거
                // 점프 방향을 기반으로 jumpRotation 계산
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                // Player의 회전을 jumpRotation으로 설정
                playerTransform.rotation = jumpRotation;

                // 움직이고 있는 경우
                if (inputHandler.moveAmount > 0)
                {
                    
                }
            }
        }

        #endregion
    }
}