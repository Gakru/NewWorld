using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class IdleState : State
    {
        // PursueTargetState ����
        public TraceTargetState traceTargetState;
        // DeathState ����
        public DeathState deathState;
        // LayerMask(Ž�� ���̾�) ����
        public LayerMask detectionLayer;

        // ���� ������Ʈ
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            // Player Ž��
            // OverlapSphere ���� �浹ü �˻�
            Collider[] collider = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < collider.Length; i++)
            {
                // �浹ü�� CharacterStats ������Ʈ ��������
                CharacterStats characterStats = collider[i].transform.GetComponent<CharacterStats>();

                // CharacterStats ������Ʈ�� �����ϴ� ���
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

                    // Ÿ�� ���� ���
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    // �þ� ���� ���
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    // �þ� ���� ���� ���� ���
                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        // ���� Ÿ�� ����
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Switching Next State
            // ���� Ÿ���� �����ϴ� ���
            if (enemyManager.currentTarget != null)
            {
                // ���� ���·� ��ȯ
                return traceTargetState;
            }
            // ���Ļ��� ���� ����� ���
            else if (enemyStats.currentHealth <= 0)
            {
                // ��� ���·� ��ȯ
                return deathState;
            }
            else
            {
                // �׷��� ������(���� Ÿ���� ���� ���) ���� ����(Idle) ����
                return this;
            }
            #endregion
        }
    }
}
