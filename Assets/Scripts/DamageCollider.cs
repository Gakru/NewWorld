using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class DamageCollider : MonoBehaviour
    {
        // Collider 참조
        Collider damagerCollider;

        public int currentWeaponDamage = 25; // 현재 가하는 데미지

        private void Awake()
        {
            damagerCollider = GetComponent<Collider>(); // Collider 컴포넌트 초기화
            damagerCollider.gameObject.SetActive(true); // Collider가 활성화된 gameObject 설정
            damagerCollider.isTrigger = true; // Collider -> Trigger로 변경
            damagerCollider.enabled = false; // Collider 비활성화
        }

        // 데미지 활성화
        public void EnableDamageCollider()
        {
            damagerCollider.enabled = true; // Collider 활성화
        }
        // 데미지 비활성화
        public void DisableDamageCollider()
        {
            damagerCollider.enabled = false; // Collider 비활성화
        }

        private void OnTriggerEnter(Collider collision)
        {
            // 충돌한 객체의 태그가 "Player"인 경우
            if (collision.tag == "Player")
            {
                Debug.Log("받은 데미지: " + currentWeaponDamage);
                // PlayerStats 컴포넌트 참조
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
                // PlayerStats 컴포넌트가 존재하면
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage); // Player에게 데미지 부여
                }
            }

            // 충돌한 객체의 태그가 "Enemy"인 경우
            if (collision.tag == "Enemy")
            {
                Debug.Log("적용된 데미지: " + currentWeaponDamage); // 부여된 데미지 표시
                // EnemyStats 컴포넌트 참조
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                // EnemyStats 컴포넌트가 존재하면
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage); // Enemy에게 데미지 부여
                }
            }
        }
    }
}
