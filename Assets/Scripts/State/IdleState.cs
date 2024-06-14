using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class IdleState : State
    {
        // PursueTargetState 참조
        public TraceTargetState traceTargetState;
        // DeathState 참조
        public DeathState deathState;
        // LayerMask(탐지 레이어) 참조
        public LayerMask detectionLayer;

        // 상태 업데이트
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            // Player 탐지
            // OverlapSphere 내의 충돌체 검색
            Collider[] collider = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < collider.Length; i++)
            {
                // 충돌체의 CharacterStats 컴포넌트 가져오기
                CharacterStats characterStats = collider[i].transform.GetComponent<CharacterStats>();

                // CharacterStats 컴포넌트가 존재하는 경우
                if (characterStats != null)
                {
                    if (collider[i].gameObject.CompareTag("Player") && characterStats != null)
                    {
                        if (characterStats.currentHealth > enemyStats.currentHealth)
                        {
                            enemyManager.currentTarget = characterStats;
                            return traceTargetState;
                        }
                    }

                    // 타겟 방향 계산
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    // 시야 각도 계산
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    // 시야 각도 범위 내인 경우
                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        // 현재 타겟 설정
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Switching Next State
            // 현재 타겟이 존재하는 경우
            if (enemyManager.currentTarget != null)
            {
                // 추적 상태로 전환
                return traceTargetState;
            }
            // 유후상태 도중 사망할 경우
            else if (enemyStats.currentHealth <= 0)
            {
                // 사망 상태로 전환
                return deathState;
            }
            else
            {
                // 그렇지 않으면(현재 타겟이 없는 경우) 현재 상태(Idle) 유지
                return this;
            }
            #endregion
        }
    }
}
