using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;
using UnityEngineInternal;

namespace KHC
{
    public class CameraHandler : MonoBehaviour
    {
        // InputHandler ����
        InputHandler inputHandler;
        // PlayerManager ����
        PlayerManager playerManager;

        // EnemyStats ����
        EnemyStats enemyStats;

        // Ÿ�� �̵� ��ġ ����
        public Transform targetTransform;
        // ī�޶� �̵� ��ġ ����
        public Transform cameraTransform;
        // ī�޶� pivot �̵� ��ġ ����
        public Transform cameraPivotTransform;
        // Player �̵� ��ġ ����
        private Transform playerTransform;

        private Vector3 cameraTransformPosition; // ī�޶� ��ȯ �̵� ��ġ
        private LayerMask ignoreLayers; // ������ LayerMask
        public LayerMask environmentLayer; // �ֺ�ȯ�� LayerMask // E.26
        private Vector3 cameraFollowVelocity = Vector3.zero; // ī�޶� ���󰡴� �ӵ�

        // CameraHandler �̱���
        public static CameraHandler singleton;

        public float lookSpeed = 0.1f; // ī�޶� ȸ�� �ӵ�
        public float followSpeed = 0.1f; // ī�޶� ���� �ӵ�
        public float pivotSpeed = 0.03f; // ī�޶� pivot ȸ�� �ӵ�

        private float targetPosition; // Ÿ�� ��ġ
        private float defaultPosition; // �⺻ ��ġ
        private float lookAngle; // ���� ����
        private float pivotAngle; // pivot ����
        public float minimumPivot = -35f; // �ּ� pivot ����
        public float maximumPivot = 35f; // �ִ� pivot ����

        public float cameraSphereRadius = 0.2f; // ī�޶� ��ü ������
        public float cameraCollisionOffSet = 0.2f; // ī�޶� �浹 offset
        public float minimumCollisisonOffSet = 0.2f; // ī�޶� �浹 �ּ� offset
        public float lockPivotPosition = 2.25f; // ���� ���� Pivot ��ġ
        public float unlockPivotPosition = 1.65f; // ���� ���� ���� pivot ��ġ

        // ���� ��� ���
        public Transform currentLockOnTarget;

        // ��� ������ ��� List
        List<CharacterManager> availableTarget = new List<CharacterManager>();
        // ���� ����� ���� ���
        public Transform nearestLockOnTarget;
        // ���� ���� ���(���� ���� ��� ����)
        public Transform leftLockTarget;
        // ������ ���� ���(���� ���� ��� ����)
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 30f; // �ִ� ���� ���� �Ÿ�

        private void Awake()
        {
            singleton = this; // �̱��� ���� // ���ϸ� ī�޶� �ȵ���...
            playerTransform = transform; // Player �̵� ��ġ�� transform���� ����
            defaultPosition = cameraTransform.localPosition.z; // �⺻ ��ġ ����
            ignoreLayers = ~(1 << 6 | 1 << 7 | 1 << 8 | 1 << 10); // ������ Layer ����
            targetTransform = FindObjectOfType<PlayerManager>().transform; // PlayerManager ������Ʈ ��ġ ����
            inputHandler = FindObjectOfType<InputHandler>(); // InputHandler ������Ʈ ����
            playerManager = FindObjectOfType<PlayerManager>(); // PlayerManager ������Ʈ ����
            enemyStats = FindObjectOfType<EnemyStats>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment"); // E.26
        }

        private void Update()
        {
            // ����Ʈ���� ü���� 0�� ����� ����
            //availableTarget.RemoveAll(enemy => enemy.GetComponent<EnemyStats>().currentHealth <= 0);
        }

        // Ÿ�� ����
        public void FollowTarget(float delta)
        {
            // Ÿ�� ��ġ SmootDamp
            Vector3 targetPosition = Vector3.SmoothDamp(
                playerTransform.position, targetTransform.position,ref cameraFollowVelocity ,delta / followSpeed);

            playerTransform.position = targetPosition; // Player �̵� ��ġ�� Ÿ�� ��ġ�� ����

            // ī�޶� �浹 ó�� �޼ҵ�(HandleCameraCollisions) ����
            HandleCameraCollisions(delta);
        }

        // ī�޶� ȸ�� ó��
        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            // ���� Ÿ���� ���� ���
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                // ī�޶� ȸ�� �ӵ� ����
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                // pivot ���� ����
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                // [Player ȸ�� ����]
                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                playerTransform.rotation = targetRotation;

                // ī�޶� pivot ȸ�� ����
                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            // ���� Ÿ���� �ִ� ���
            else
            {
                // float velocity = 0f;

                // ���� Ÿ�����κ����� ���� ���
                Vector3 direction = currentLockOnTarget.position - transform.position;
                direction.Normalize();
                direction.y = 0f;

                // ���⿡ ���� ȸ�� ����
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;

                // ī�޶� Pivot ���� ���
                direction = currentLockOnTarget.position - cameraPivotTransform.position;
                direction.Normalize();

                // ���⿡ ���� ȸ�� ����
                targetRotation = Quaternion.LookRotation(direction);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0f;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        // ī�޶� �浹 ó��
        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition; // Ÿ�� ��ġ�� �⺻ ��ġ�� ����
            RaycastHit hit;
            // ���� ���� ����
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize(); // direction�� ����ȭ�Ͽ� ���� ���ͷ� ����

            // ��ü Raycast ����
            if (Physics.SphereCast(
                cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                // �浹 �Ÿ� ���
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet); // Ÿ�� ��ġ ������Ʈ
            }

            // �ּ� �浹 offset
            if (Mathf.Abs(targetPosition) < minimumCollisisonOffSet)
            {
                targetPosition = -minimumCollisisonOffSet;
            }

            // ī�޶� ��ġ ���� ����
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition; // ī�޶� ��ġ ������Ʈ
        }

        // ī�޶� ���� ���� ó��
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity; // ���� ����� �Ÿ�
            float shortestDistanceOfLeftTarget = Mathf.Infinity; // ���� ����� ���� Ÿ���� �Ÿ�
            float shortestDistanceOfRightTarget = Mathf.Infinity; // ���� ����� ������ Ÿ���� �Ÿ�

            // Ÿ�� ����(20) ���� �浹ü �˻�
            Collider[] collider = Physics.OverlapSphere(targetTransform.position, 20);

            for (int i = 0; i < collider.Length; i++)
            {
                // �浹ü �迭���� CharacterManager ������Ʈ�� ���� ��ü ã��
                CharacterManager character = collider[i].GetComponent<CharacterManager>();

                // CharacterManager ������Ʈ�� �ִ� ��� + ü���� �ִ� ���(����ִ� ����)
                if (character != null && enemyStats.currentHealth > 0)
                {
                    // Ÿ�ٿ��� character������ ���� ���
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    // Ÿ�ٿ��� character������ �Ÿ� ���
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    // ī�޶� ����� character ���� ������ ���� ���
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;
                    
                    // ����� ��Ʈ ��ü�� �ٸ���, �þ� ������ 50�� �̳��̸�, �ִ� �Ÿ� ���� ���
                    if (character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        // ����Ʈ�� �߰�
                        availableTarget.Add(character);

                        // LockOnEnvironmet E.26
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            // PlayerManager�� lockOnTransform�� character�� lockOnTransform ���̿� ���� �׸���
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            // �浹�� ��ü�� Layer�� environmentLayer�� ��ġ�ϴ��� Ȯ��
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {

                            }
                            else
                            {
                                // environmentLayer�� ������ �ʴ� ���, ����Ʈ�� �߰�
                                availableTarget.Add(character);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < availableTarget.Count; i++)
            {
                // ���� ����� Ÿ�� ã��
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget[i].transform.position);

                // ���� ���� ����� �Ÿ����� ���� ���
                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget; // ���� ����� �Ÿ� �Ÿ��� ����
                    nearestLockOnTarget = availableTarget[i].lockOnTransform; // ���� ����� Ÿ�� ����
                }

                // ������ �����Ǿ��� ���
                if (inputHandler.lockOnFlag)
                {
                    // ���� ���� ���� Ÿ�ٿ� ���� ���� ��ġ ���
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTarget[i].transform.position);
                    // ���� ���� ���� Ÿ�ٰ� ���� Ÿ�� ������ �Ÿ� ���
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTarget[i].transform.position.x;
                    // ���� ���� ���� Ÿ�ٰ� ������ Ÿ�� ������ �Ÿ� ���
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTarget[i].transform.position.x;

                    // ���� ���� ���� ���� Ÿ���� ���ʿ� �ְ�, ���� Ÿ�� �� ���� ����� �Ÿ��� ���� ���
                    if (relativeEnemyPosition.x > 0.00f && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget; // ���� Ÿ���� ���� ����� �Ÿ� ����
                        leftLockTarget = availableTarget[i].lockOnTransform; // ���� Ÿ�� ����
                    }

                    // ���� ���� ���� ���� Ÿ���� �����ʿ� �ְ�, ������ Ÿ�� �� ���� ����� �Ÿ��� ���� ���
                    if (relativeEnemyPosition.x < 0.00f && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget; // ������ Ÿ���� ���� ����� �Ÿ� ����
                        rightLockTarget = availableTarget[i].lockOnTransform; // ������ Ÿ�� ����
                    }
                }
            }
        }

        // Ÿ�� �ʱ�ȭ
        public void ClearLockOnTarget()
        {
            // ��� ������ Ÿ�� ����Ʈ ����
            availableTarget.Clear();
            // ���� ����� ���� Ÿ�� �ʱ�ȭ
            nearestLockOnTarget = null;
            // ���� ���� Ÿ�� �ʱ�ȸ
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            // �ӵ��� ������ Vector3 ���� �ʱ�ȭ
            Vector3 velocity = Vector3.zero;
            // ī�޶� Pivot�� ���ο� ��ġ ����
            Vector3 newLockedPosition = new Vector3(0, lockPivotPosition);
            // ī�޶� Pivot�� ���� ��ġ ����
            Vector3 newUnlockedPosition = new Vector3(0, unlockPivotPosition);

            // ���� ���� ���� Ÿ���� �ִ��� Ȯ��
            if (currentLockOnTarget != null)
            {
                // ���� ���� Ÿ���� ������, ī�޶� Pivot�� ���� ��ġ�� newLockedPosition���� �ε巴�� ��ȯ
                // �ð��� ������ ���� ��ǥ ��ġ�� �ε巴�� ��ȯ
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                // ���� ���� ����� ������, ī�޶� Pivot�� ���� ��ġ�� newUnlockedPositionm���� �ε巴�� ��ȯ
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}