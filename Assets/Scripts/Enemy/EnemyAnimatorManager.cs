using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        // EnemyManager ����
        EnemyManager enemyManager;
        // EnemyStats ����
        EnemyStats enemyStats;

        private void Awake()
        {
            animator = GetComponent<Animator>(); // Animator ������Ʈ �ʱ�ȭ
            enemyManager = GetComponentInParent<EnemyManager>(); // EnemyManager ������Ʈ �ʱ�ȭ
            enemyStats = GetComponentInParent<EnemyStats>(); // EnemyStats �������� �ʱ�ȭ
        }

        private void Start()
        {
            
        }

        private void OnAnimatorMove()
        {
            //Debug.Log(enemyManager.enemyRigidbody.velocity);
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0; // �巡�� �� �ʱ�ȭ
            Vector3 deltaPosition = animator.deltaPosition; // animator�� ��ġ ��ȭ ���
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition/ delta; // �ӵ� ���
            enemyManager.enemyRigidbody.velocity = velocity; // Rigidbody�� �ӵ� ����
        }
    }
}
