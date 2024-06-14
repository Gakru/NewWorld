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
        // InputHandler 참조
        InputHandler inputHandler;
        // PlayerManager 참조
        PlayerManager playerManager;

        // EnemyStats 참조
        EnemyStats enemyStats;

        // 타겟 이동 위치 참조
        public Transform targetTransform;
        // 카메라 이동 위치 참조
        public Transform cameraTransform;
        // 카메라 pivot 이동 위치 참조
        public Transform cameraPivotTransform;
        // Player 이동 위치 참조
        private Transform playerTransform;

        private Vector3 cameraTransformPosition; // 카메라 변환 이동 위치
        private LayerMask ignoreLayers; // 무시할 LayerMask
        public LayerMask environmentLayer; // 주변환경 LayerMask // E.26
        private Vector3 cameraFollowVelocity = Vector3.zero; // 카메라를 따라가는 속도

        // CameraHandler 싱글톤
        public static CameraHandler singleton;

        public float lookSpeed = 0.1f; // 카메라 회전 속도
        public float followSpeed = 0.1f; // 카메라 추적 속도
        public float pivotSpeed = 0.03f; // 카메라 pivot 회전 속도

        private float targetPosition; // 타겟 위치
        private float defaultPosition; // 기본 위치
        private float lookAngle; // 보기 각도
        private float pivotAngle; // pivot 각도
        public float minimumPivot = -35f; // 최소 pivot 각도
        public float maximumPivot = 35f; // 최대 pivot 각도

        public float cameraSphereRadius = 0.2f; // 카메라 구체 반지름
        public float cameraCollisionOffSet = 0.2f; // 카메라 충돌 offset
        public float minimumCollisisonOffSet = 0.2f; // 카메라 충돌 최소 offset
        public float lockPivotPosition = 2.25f; // 시점 고정 Pivot 위치
        public float unlockPivotPosition = 1.65f; // 시점 고정 해제 pivot 위치

        // 현재 잠금 대상
        public Transform currentLockOnTarget;

        // 사용 가능한 대상 List
        List<CharacterManager> availableTarget = new List<CharacterManager>();
        // 가장 가까운 고정 대상
        public Transform nearestLockOnTarget;
        // 왼쪽 고정 대상(현재 고정 대상 기준)
        public Transform leftLockTarget;
        // 오른쪽 고정 대상(현재 고정 대상 기준)
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 30f; // 최대 시점 고정 거리

        private void Awake()
        {
            singleton = this; // 싱글톤 설정 // 안하면 카메라 안따라감...
            playerTransform = transform; // Player 이동 위치를 transform으로 설정
            defaultPosition = cameraTransform.localPosition.z; // 기본 위치 설정
            ignoreLayers = ~(1 << 6 | 1 << 7 | 1 << 8 | 1 << 10); // 무시할 Layer 설정
            targetTransform = FindObjectOfType<PlayerManager>().transform; // PlayerManager 컴포넌트 위치 참조
            inputHandler = FindObjectOfType<InputHandler>(); // InputHandler 컴포넌트 참조
            playerManager = FindObjectOfType<PlayerManager>(); // PlayerManager 컴포넌트 참조
            enemyStats = FindObjectOfType<EnemyStats>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment"); // E.26
        }

        private void Update()
        {
            // 리스트에서 체력이 0인 대상을 제거
            //availableTarget.RemoveAll(enemy => enemy.GetComponent<EnemyStats>().currentHealth <= 0);
        }

        // 타겟 추적
        public void FollowTarget(float delta)
        {
            // 타겟 위치 SmootDamp
            Vector3 targetPosition = Vector3.SmoothDamp(
                playerTransform.position, targetTransform.position,ref cameraFollowVelocity ,delta / followSpeed);

            playerTransform.position = targetPosition; // Player 이동 위치를 타겟 위치로 설정

            // 카메라 충돌 처리 메소드(HandleCameraCollisions) 실행
            HandleCameraCollisions(delta);
        }

        // 카메라 회전 처리
        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            // 현재 타겟이 없읗 경우
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                // 카메라 회전 속도 조정
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                // pivot 각도 제한
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                // [Player 회전 설정]
                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                playerTransform.rotation = targetRotation;

                // 카메라 pivot 회전 설정
                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            // 현재 타겟이 있는 경우
            else
            {
                // float velocity = 0f;

                // 현재 타겟으로부터의 방향 계산
                Vector3 direction = currentLockOnTarget.position - transform.position;
                direction.Normalize();
                direction.y = 0f;

                // 방향에 따른 회전 설정
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;

                // 카메라 Pivot 방향 계산
                direction = currentLockOnTarget.position - cameraPivotTransform.position;
                direction.Normalize();

                // 방향에 따른 회전 설정
                targetRotation = Quaternion.LookRotation(direction);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0f;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        // 카메라 충돌 처리
        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition; // 타겟 위치를 기본 위치로 설정
            RaycastHit hit;
            // 방향 벡터 설정
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize(); // direction을 정규화하여 단위 벡터로 설정

            // 구체 Raycast 실행
            if (Physics.SphereCast(
                cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                // 충돌 거리 계산
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet); // 타겟 위치 업데이트
            }

            // 최소 충돌 offset
            if (Mathf.Abs(targetPosition) < minimumCollisisonOffSet)
            {
                targetPosition = -minimumCollisisonOffSet;
            }

            // 카메라 위치 선형 보간
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition; // 카메라 위치 업데이트
        }

        // 카메라 시점 고정 처리
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity; // 가장 가까운 거리
            float shortestDistanceOfLeftTarget = Mathf.Infinity; // 가장 가까운 왼쪽 타겟의 거리
            float shortestDistanceOfRightTarget = Mathf.Infinity; // 가장 가까운 오른쪽 타겟의 거리

            // 타겟 범위(20) 내의 충돌체 검색
            Collider[] collider = Physics.OverlapSphere(targetTransform.position, 20);

            for (int i = 0; i < collider.Length; i++)
            {
                // 충돌체 배열에서 CharacterManager 컴포넌트를 가진 객체 찾기
                CharacterManager character = collider[i].GetComponent<CharacterManager>();

                // CharacterManager 컴포넌트가 있는 경우 + 체력이 있는 경우(살아있는 상태)
                if (character != null && enemyStats.currentHealth > 0)
                {
                    // 타겟에서 character까지의 방향 계산
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    // 타겟에서 character까지의 거리 계산
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    // 카메라 방향과 character 방향 사이의 각도 계산
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;
                    
                    // 대상이 루트 객체가 다르고, 시야 각도가 50도 이내이며, 최대 거리 내일 경우
                    if (character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        // 리스트에 추가
                        availableTarget.Add(character);

                        // LockOnEnvironmet E.26
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            // PlayerManager의 lockOnTransform와 character의 lockOnTransform 사이에 선을 그리낟
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            // 충돌한 객체의 Layer가 environmentLayer와 일치하는지 확인
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {

                            }
                            else
                            {
                                // environmentLayer에 속하지 않는 경우, 리스트에 추가
                                availableTarget.Add(character);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < availableTarget.Count; i++)
            {
                // 가장 가까운 타겟 찾기
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget[i].transform.position);

                // 현재 가장 가까운 거리보다 작은 경우
                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget; // 가장 가까운 거리 거리를 설정
                    nearestLockOnTarget = availableTarget[i].lockOnTransform; // 가장 가까운 타겟 설정
                }

                // 시점이 고정되었을 경우
                if (inputHandler.lockOnFlag)
                {
                    // 현재 시점 고정 타겟에 대한 적의 위치 계산
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTarget[i].transform.position);
                    // 현재 시점 고정 타겟과 왼쪽 타겟 사이의 거리 계산
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTarget[i].transform.position.x;
                    // 현재 시점 고정 타겟과 오른쪽 타겟 사이의 거리 계산
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTarget[i].transform.position.x;

                    // 적이 현재 시점 고정 타겟의 왼쪽에 있고, 왼쪽 타겟 증 가장 가까운 거리를 가진 경우
                    if (relativeEnemyPosition.x > 0.00f && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget; // 왼쪽 타겟의 가장 가까운 거리 설정
                        leftLockTarget = availableTarget[i].lockOnTransform; // 왼쪽 타겟 설정
                    }

                    // 적이 현재 시점 고정 타겟의 오른쪽에 있고, 오른쪽 타겟 중 가장 가까운 거리를 가진 경우
                    if (relativeEnemyPosition.x < 0.00f && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget; // 오른쪽 타겟의 가장 가까운 거리 설정
                        rightLockTarget = availableTarget[i].lockOnTransform; // 오른쪽 타겟 설정
                    }
                }
            }
        }

        // 타겟 초기화
        public void ClearLockOnTarget()
        {
            // 사용 가능한 타겟 리스트 비우기
            availableTarget.Clear();
            // 가장 가까운 고정 타겟 초기화
            nearestLockOnTarget = null;
            // 현재 고정 타겟 초기회
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            // 속도를 저장할 Vector3 변수 초기화
            Vector3 velocity = Vector3.zero;
            // 카메라 Pivot의 새로운 위치 정의
            Vector3 newLockedPosition = new Vector3(0, lockPivotPosition);
            // 카메라 Pivot의 해제 위치 정의
            Vector3 newUnlockedPosition = new Vector3(0, unlockPivotPosition);

            // 현재 시점 고정 타겟이 있는지 확인
            if (currentLockOnTarget != null)
            {
                // 시점 고정 타겟이 있으면, 카메라 Pivot의 현재 위치를 newLockedPosition으로 부드럽게 전환
                // 시간이 지남에 따라 목표 위치로 부드럽게 변환
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                // 시점 고정 대상이 없으면, 카메라 Pivot의 현재 위치를 newUnlockedPositionm으로 부드럽게 전환
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}