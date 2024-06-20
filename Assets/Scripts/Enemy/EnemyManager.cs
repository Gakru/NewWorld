using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class EnemyManager : CharacterManager
    {
        

        // EnemyLocalMotionManager 참조
        EnemyLocomotionManager enemyLocalMotionManager;
        // EnemyAnimatorManager 참조
        EnemyAnimatorManager enemyAnimatorManager;
        // EnemyStats 참조
        EnemyStats enemyStats;

        // State 참조
        public State currentState;
        // CharacterStats 참조
        public CharacterStats currentTarget;
        // NavMeshAgent 참조
        public NavMeshAgent navMeshAgent;
        // Rigidbody 참조
        public Rigidbody enemyRigidbody;

        public bool isPreformingAction; // 액션 수행 여부
        public bool isInteracting; // 상호작용 여부
        public float rotationSpeed = 15f; // 회전 속도
        public float maximumAttackRange = 1.5f; // 최대 공격 사거리

        [Header("A.I Settings")] // AI 설정
        public float detectionRadius = 20; // 탐지 변경 범위
        public float maximumDetectionAngle = 50; // 최대 탐지 시야 각도
        public float minimumDetectionAngle = -50; // 최소 탐지 시야 각도
        //public float viewableAngle; // 탐지 시야 각도

        public float currentRecoveryTime = 0f; // 회복 시간

        private void Awake()
        {
            enemyLocalMotionManager = GetComponent<EnemyLocomotionManager>(); // EnemyLocalMotionManager 컴포넌트 초기화
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>(); // EnemyAnimatorManager 컴포넌트 초기화
            enemyStats = GetComponent<EnemyStats>(); // EnemyStats 컴포넌트 초기화
            enemyRigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 초기화
            navMeshAgent = GetComponentInChildren<NavMeshAgent>(); // NavMeshAgent 컴포넌트 초기화
            navMeshAgent.enabled = false; // NavMeshAgent 비활성화
        }

        private void Update()
        {
            // 공격 대기 시간 처리 실행
            HandleRecoveryTimer();

            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            // 현재 액션 처리 실행
            HandleStateMachine();
        }

        // 현재 액션 처리
        public void HandleStateMachine()
        {
            if (currentState != null)
            {
                // 현재 상태 업데이트
                State nextState = currentState.StateUpdate(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    // 다음 상태로 전화
                    SwitchNextState(nextState);
                }
            }
        }

        // 다음 상태로 전환 처리
        private void SwitchNextState(State state)
        {
            currentState = state; // 현재 상태를 nextState로 업데이트
        }

        // 공격 대기 시간 회복 처리
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
