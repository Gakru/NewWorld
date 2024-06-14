using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class AttackState : State
    {
        // CombatStanceState 참조
        public CombatStanceState combatStanceState;
        // DeathState 참조
        public DeathState deathState;

        // EnemyAttackAction(배열) 참조
        public EnemyAttackAction[] enemyAttacks;
        // EnemyAttackAction(현재 공격) 참조
        public EnemyAttackAction currentAttack;

        // 상태 업데이트
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // 현재 타겟 방향 계산
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            // 현재 타겟까지의 거리 계산
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // 시야 각도 계산
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            // 현재 액션 중이라면 전투 상태로 전환
            if (enemyManager.isPreformingAction) return combatStanceState;

            // 현재 공격이 존재하는 경우
            if (currentAttack != null && enemyStats.currentHealth > 0)
            {
                // 타겟까지의 거리가 최소 공격 거리보다 짧은 경우
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                // 타겟까지의 거리가 최대 공격 각도보다 작은 경우
                else if (distanceFromTarget < currentAttack.maximumAttackAngle)
                {
                    // 시야 각도가 최대 공격 각도 이하이거나 시야 각도가 최소 공격 각도 이상일 경우
                    if (viewableAngle <= currentAttack.maximumAttackAngle &&
                        viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        // 공격 대기 시간이 0 이하이고, 액션 중이 아닐 경우
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                        {
                            // Animator의 "Vertical" 파라미터를 설정하여 이동 애니메이션을 재생
                            enemyAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                            // Animator의 "Horizontal" 파라미터를 설정하여 이동 애니메이션을 재생
                            enemyAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                            // 지정된 애니메이션 실행
                            enemyAnimatorManager.PlayAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPreformingAction = true; // 액션 중으로 설정
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime; // 공격 대기 시간 설정
                            currentAttack = null; // 현재 공격을 null로 설정
                            return combatStanceState; // 전투 상태로 전환
                        }
                    }
                }
            }
            // 공격 중 사망할 경우
            else if (enemyStats.currentHealth <= 0)
            {
                // 사망 상태로 전환
                return deathState;
            }
            else
            {
                // 새로운 공격 선택
                EnemyNewAttack(enemyManager);
            }

            // 전투 상태로 전환
            return combatStanceState;
        }

        // 새로운 공격 처리
        private void EnemyNewAttack(EnemyManager enemyManager)
        {
            // 타겟 방향 계산
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            // 시야 각도 계산
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            // 타겟까지의 거리 계산
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0; // 최대 점수 초기화

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 현재 공격 액션 가져오기
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // 타겟까지의 거리가 최대 공격 필요 거리 이하이거나 최소 공격 필요 거리 이상일 경우
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && 
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    // 시야 각도가 최대 공격 각도 이하이거나 시야 각도가 최소 공격 각도 이상일 경우
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && 
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore; // 공격 누적
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore); // 랜덤 값 생성
            int temporaryScore = 0; // 임시 점수 초기화

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 현재 공격 액션 가져오기
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // 타겟까지의 거리가 최대 공격 필요 거리 이하이거나 최소 공격 필요 거리 이상일 경우
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && 
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    // 시야 각도가 최대 공격 각도 이하이거나 시야 각도가 최소 공격 각도 이상일 경우
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && 
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        // 이미 공격 액션이 설정되어 있으면 종료
                        if (currentAttack != null) return;

                        temporaryScore += enemyAttackAction.attackScore; // 공격 누적

                        // 임시 점수가 랜덤 값보다 큰 경우
                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction; // 현재 공격 실행
                        }
                    }
                }
            }
        }

        // 타겟 방향으로 회전 처리
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
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
