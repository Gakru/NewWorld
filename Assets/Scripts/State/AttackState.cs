using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class AttackState : State
    {
        // CombatStanceState ����
        public CombatStanceState combatStanceState;
        // DeathState ����
        public DeathState deathState;

        // EnemyAttackAction(�迭) ����
        public EnemyAttackAction[] enemyAttacks;
        // EnemyAttackAction(���� ����) ����
        public EnemyAttackAction currentAttack;

        // ���� ������Ʈ
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            // ���� Ÿ�� ���� ���
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            // ���� Ÿ�ٱ����� �Ÿ� ���
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // �þ� ���� ���
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            // ���� �׼� ���̶�� ���� ���·� ��ȯ
            if (enemyManager.isPreformingAction) return combatStanceState;

            // ���� ������ �����ϴ� ���
            if (currentAttack != null && enemyStats.currentHealth > 0)
            {
                // Ÿ�ٱ����� �Ÿ��� �ּ� ���� �Ÿ����� ª�� ���
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                // Ÿ�ٱ����� �Ÿ��� �ִ� ���� �������� ���� ���
                else if (distanceFromTarget < currentAttack.maximumAttackAngle)
                {
                    // �þ� ������ �ִ� ���� ���� �����̰ų� �þ� ������ �ּ� ���� ���� �̻��� ���
                    if (viewableAngle <= currentAttack.maximumAttackAngle &&
                        viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        // ���� ��� �ð��� 0 �����̰�, �׼� ���� �ƴ� ���
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                        {
                            // Animator�� "Vertical" �Ķ���͸� �����Ͽ� �̵� �ִϸ��̼��� ���
                            enemyAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                            // Animator�� "Horizontal" �Ķ���͸� �����Ͽ� �̵� �ִϸ��̼��� ���
                            enemyAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                            // ������ �ִϸ��̼� ����
                            enemyAnimatorManager.PlayAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPreformingAction = true; // �׼� ������ ����
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime; // ���� ��� �ð� ����
                            currentAttack = null; // ���� ������ null�� ����
                            return combatStanceState; // ���� ���·� ��ȯ
                        }
                    }
                }
            }
            // ���� �� ����� ���
            else if (enemyStats.currentHealth <= 0)
            {
                // ��� ���·� ��ȯ
                return deathState;
            }
            else
            {
                // ���ο� ���� ����
                EnemyNewAttack(enemyManager);
            }

            // ���� ���·� ��ȯ
            return combatStanceState;
        }

        // ���ο� ���� ó��
        private void EnemyNewAttack(EnemyManager enemyManager)
        {
            // Ÿ�� ���� ���
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            // �þ� ���� ���
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            // Ÿ�ٱ����� �Ÿ� ���
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0; // �ִ� ���� �ʱ�ȭ

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // ���� ���� �׼� ��������
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // Ÿ�ٱ����� �Ÿ��� �ִ� ���� �ʿ� �Ÿ� �����̰ų� �ּ� ���� �ʿ� �Ÿ� �̻��� ���
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && 
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    // �þ� ������ �ִ� ���� ���� �����̰ų� �þ� ������ �ּ� ���� ���� �̻��� ���
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && 
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore; // ���� ����
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore); // ���� �� ����
            int temporaryScore = 0; // �ӽ� ���� �ʱ�ȭ

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // ���� ���� �׼� ��������
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // Ÿ�ٱ����� �Ÿ��� �ִ� ���� �ʿ� �Ÿ� �����̰ų� �ּ� ���� �ʿ� �Ÿ� �̻��� ���
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && 
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    // �þ� ������ �ִ� ���� ���� �����̰ų� �þ� ������ �ּ� ���� ���� �̻��� ���
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && 
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        // �̹� ���� �׼��� �����Ǿ� ������ ����
                        if (currentAttack != null) return;

                        temporaryScore += enemyAttackAction.attackScore; // ���� ����

                        // �ӽ� ������ ���� ������ ū ���
                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction; // ���� ���� ����
                        }
                    }
                }
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
