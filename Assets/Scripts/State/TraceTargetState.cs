using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class TraceTargetState : State
    {
        // CombatStanceState ����
        public CombatStanceState combatStanceState;
        // DeathState ����
        public DeathState deathState;

        // ���� ������Ʈ
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // �׼� ���̰� ���� �Ÿ����� �ָ� �ִٸ� ���� ����(��������) ����
            if (enemyManager.isPreformingAction) //&& enemyManager.distanceFromTarget > enemyManager.stoppingDistance
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                return this;
            }

            // ���� Ÿ�� ���� ���
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            // ���� Ÿ�ٱ����� �Ÿ� ���
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // �þ� ���� ���
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            // ���� �Ÿ����� �ָ� �ִ� ���
            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                // Animator�� "Vertical" �Ķ���͸� �����Ͽ� �̵� �ִϸ��̼��� ���
                enemyAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
            }

            // Ÿ�� �������� ȸ�� ó�� ����
            HandleRotateTowardsTarget(enemyManager, enemyStats);

            // ���� ��ġ�� x ��ǥ�� NavMeshAgent�� y ��ǥ�� ����Ͽ� ���ο� ��ġ�� ���
            transform.position = new Vector3(transform.position.x, enemyManager.navMeshAgent.transform.position.y, transform.position.z);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero; // NavMeshAgent ��ġ �ʱ�ȭ
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity; // NavMeshAgent ȸ�� �ʱ�ȭ

            // Ÿ�ٱ����� �Ÿ��� �ִ� ���� ��Ÿ� �̳��� ���
            if (distanceFromTarget <= enemyManager.maximumAttackRange && enemyStats.currentHealth > 0)
            {
                // ���� ���·� ��ȯ
                return combatStanceState;
            }
            // ���� �� ����� ���
            else if (enemyStats.currentHealth <= 0)
            {
                // ��� ���·� ��ȯ
                return deathState;
            }
            else
            {
                // �׷��� ������ ���� ����(��������) ����
                return this;
            }
        }

        // Ÿ�� �������� ȸ�� ó��
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, EnemyStats enemyStats)
        {
            if (enemyStats.currentHealth <= 0) return;

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
