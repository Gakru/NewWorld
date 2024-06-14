using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class TraceTargetState : State
    {
        // CombatStanceState 참조
        public CombatStanceState combatStanceState;
        // DeathState 참조
        public DeathState deathState;

        // 상태 업데이트
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // 액션 중이고 정지 거리보다 멀리 있다면 현재 상태(추적상태) 유지
            if (enemyManager.isPreformingAction) //&& enemyManager.distanceFromTarget > enemyManager.stoppingDistance
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                return this;
            }

            // 현재 타겟 방향 계산
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            // 현재 타겟까지의 거리 계산
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // 시야 각도 계산
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            // 정지 거리보다 멀리 있는 경우
            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                // Animator의 "Vertical" 파라미터를 설정하여 이동 애니메이션을 재생
                enemyAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
            }

            // 타겟 방향으로 회전 처리 실행
            HandleRotateTowardsTarget(enemyManager, enemyStats);

            // 현재 위치의 x 좌표와 NavMeshAgent의 y 좌표를 사용하여 새로운 위치를 계산
            transform.position = new Vector3(transform.position.x, enemyManager.navMeshAgent.transform.position.y, transform.position.z);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero; // NavMeshAgent 위치 초기화
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity; // NavMeshAgent 회전 초기화

            // 타겟까지의 거리가 최대 공격 사거리 이내인 경우
            if (distanceFromTarget <= enemyManager.maximumAttackRange && enemyStats.currentHealth > 0)
            {
                // 전투 상태로 전환
                return combatStanceState;
            }
            // 추적 중 사망할 경우
            else if (enemyStats.currentHealth <= 0)
            {
                // 사망 상태로 전환
                return deathState;
            }
            else
            {
                // 그렇지 않으면 현재 상태(추적상태) 유지
                return this;
            }
        }

        // 타겟 방향으로 회전 처리
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, EnemyStats enemyStats)
        {
            if (enemyStats.currentHealth <= 0) return;

            // 액션 중일 때 수동으로 회전
            if (enemyManager.isPreformingAction)
            {
                // 타겟 방향 계산
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0f; // y축 제거
                direction.Normalize(); // 방향 벡터 정규화

                // 방향 벡터가 0인 경우
                if (direction == Vector3.zero)
                {
                    direction = transform.forward; // 방향 벡터 설정
                }

                // 회전 Quaternion 계산
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // 부드러운 회전 적용
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                //// 타겟 방향 계산
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                //// 타겟 속도 설정
                //Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                //enemyManager.navMeshAgent.enabled = true; // NavMeshAgent 활성화
                //enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position); // 목적지 설정
                //enemyManager.enemyRigidbody.velocity = targetVelocity; // Rigidbody 속도 설정
                //// 부드러운 회전 적용
                //enemyManager.transform.rotation =
                //Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);

                // 현재 타겟까지의 거리 계산
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                enemyManager.navMeshAgent.enabled = true;
                //Debug.Log(enemyManager.navMeshAgent.speed);
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                if (enemyManager.navMeshAgent.desiredVelocity != Vector3.zero)
                {
                    float rotationToApplyToDynamicEnemy =
                        Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                    if (distanceFromTarget > 5) enemyManager.navMeshAgent.angularSpeed = 500f;
                    else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navMeshAgent.angularSpeed = 50f;
                    else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navMeshAgent.angularSpeed = 500f;
                }

                Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


                if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    enemyManager.navMeshAgent.updateRotation = false;
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                    Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                }
                else
                {
                    enemyManager.transform.rotation = 
                        Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                }
            }
        }


    }
}
