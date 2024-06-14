using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class CombatStanceState : State
    {
        // AttackState ����
        public AttackState attackState;
        // PursueTargetState ����
        public TraceTargetState traceTargetState;
        // DeathState ����
        public DeathState deathState;

        // ���� ������Ʈ
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // ���� Ÿ�ٱ����� �Ÿ� ���
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isPreformingAction)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
            }

            // ���� ��� �ð��� 0�̰� Ÿ�ٱ����� �Ÿ��� �ִ� ���� ��Ÿ� �̳��� ���
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                // ���� ���� ��ȯ
                return attackState;
            }
            // Ÿ�ٱ����� �Ÿ��� �ִ� ���� ��Ÿ����� �ָ� �ִ� ���
            else if (distanceFromTarget > enemyManager.maximumAttackRange && enemyStats.currentHealth > 0)
            {
                // ���� ���·� ��ȯ
                return traceTargetState;
            }
            else if (enemyStats.currentHealth <= 0)
            {
                // ��� ���·� ��ȯ
                return deathState;
            }
            else
            {
                return this;
            }
        }

        // Ÿ�� �������� ȸ�� ó��
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            // �׼� ���� �� �������� ȸ��
            if (enemyManager.isPreformingAction)
            {
                // Ÿ�� ���� ���
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0f; // y�� ����
                direction.Normalize(); // ���� ���� ����ȭ

                // ���� ���Ͱ� 0�� ���
                if (direction == Vector3.zero)
                {
                    direction = transform.forward; // ���� ���� ����
                }

                // ȸ�� Quaternion ���
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // �ε巯�� ȸ�� ����
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                //// Ÿ�� ���� ���
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                //// Ÿ�� �ӵ� ����
                //Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                //enemyManager.navMeshAgent.enabled = true; // NavMeshAgent Ȱ��ȭ
                //enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position); // ������ ����
                //enemyManager.enemyRigidbody.velocity = targetVelocity; // Rigidbody �ӵ� ����
                //// �ε巯�� ȸ�� ����
                //enemyManager.transform.rotation =
                //Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);

                // ���� Ÿ�ٱ����� �Ÿ� ���
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
