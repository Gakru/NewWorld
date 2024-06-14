using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        // EnemyManager 참조
        EnemyManager enemyManager;
        // EnemyStats 참조
        EnemyStats enemyStats;

        private void Awake()
        {
            animator = GetComponent<Animator>(); // Animator 컴포넌트 초기화
            enemyManager = GetComponentInParent<EnemyManager>(); // EnemyManager 컴포넌트 초기화
            enemyStats = GetComponentInParent<EnemyStats>(); // EnemyStats 컴포넌츠 초기화
        }

        private void Start()
        {
            
        }

        private void OnAnimatorMove()
        {
            //Debug.Log(enemyManager.enemyRigidbody.velocity);
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0; // 드래그 값 초기화
            Vector3 deltaPosition = animator.deltaPosition; // animator의 위치 변화 계산
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition/ delta; // 속도 계산
            enemyManager.enemyRigidbody.velocity = velocity; // Rigidbody의 속도 설정
        }
    }
}
