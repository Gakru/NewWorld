using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KHC
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        // EenmyManager ����
        EnemyManager enemyManager;
        // EnemyAnimatorManager ����
        EnemyAnimatorManager enemyAnimatorManager;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        // Ž�� LayerMask
        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>(); // EnemyManager ������Ʈ �ʱ�ȭ
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>(); // EnemyAnimatorManager ������Ʈ �ʱ�ȭ
        }

        private void Start()
        {
            
        }
    }
}
