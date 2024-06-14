using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        // EenmyManager 참조
        EnemyManager enemyManager;
        // EnemyAnimatorManager 참조
        EnemyAnimatorManager enemyAnimatorManager;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        // 탐지 LayerMask
        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>(); // EnemyManager 컴포넌트 초기화
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>(); // EnemyAnimatorManager 컴포넌트 초기화
        }

        private void Start()
        {
            
        }
    }
}
