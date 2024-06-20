using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class EnemyManager : CharacterManager
    {
        

        // EnemyLocalMotionManager ����
        EnemyLocomotionManager enemyLocalMotionManager;
        // EnemyAnimatorManager ����
        EnemyAnimatorManager enemyAnimatorManager;
        // EnemyStats ����
        EnemyStats enemyStats;

        // State ����
        public State currentState;
        // CharacterStats ����
        public CharacterStats currentTarget;
        // NavMeshAgent ����
        public NavMeshAgent navMeshAgent;
        // Rigidbody ����
        public Rigidbody enemyRigidbody;

        public bool isPreformingAction; // �׼� ���� ����
        public bool isInteracting; // ��ȣ�ۿ� ����
        public float rotationSpeed = 15f; // ȸ�� �ӵ�
        public float maximumAttackRange = 1.5f; // �ִ� ���� ��Ÿ�

        [Header("A.I Settings")] // AI ����
        public float detectionRadius = 20; // Ž�� ���� ����
        public float maximumDetectionAngle = 50; // �ִ� Ž�� �þ� ����
        public float minimumDetectionAngle = -50; // �ּ� Ž�� �þ� ����
        //public float viewableAngle; // Ž�� �þ� ����

        public float currentRecoveryTime = 0f; // ȸ�� �ð�

        private void Awake()
        {
            enemyLocalMotionManager = GetComponent<EnemyLocomotionManager>(); // EnemyLocalMotionManager ������Ʈ �ʱ�ȭ
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>(); // EnemyAnimatorManager ������Ʈ �ʱ�ȭ
            enemyStats = GetComponent<EnemyStats>(); // EnemyStats ������Ʈ �ʱ�ȭ
            enemyRigidbody = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �ʱ�ȭ
            navMeshAgent = GetComponentInChildren<NavMeshAgent>(); // NavMeshAgent ������Ʈ �ʱ�ȭ
            navMeshAgent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
        }

        private void Update()
        {
            // ���� ��� �ð� ó�� ����
            HandleRecoveryTimer();

            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            // ���� �׼� ó�� ����
            HandleStateMachine();
        }

        // ���� �׼� ó��
        public void HandleStateMachine()
        {
            if (currentState != null)
            {
                // ���� ���� ������Ʈ
                State nextState = currentState.StateUpdate(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    // ���� ���·� ��ȭ
                    SwitchNextState(nextState);
                }
            }
        }

        // ���� ���·� ��ȯ ó��
        private void SwitchNextState(State state)
        {
            currentState = state; // ���� ���¸� nextState�� ������Ʈ
        }

        // ���� ��� �ð� ȸ�� ó��
        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }
    }
}
