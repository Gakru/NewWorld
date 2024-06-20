using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KHC
{
    public class EnemyStats : CharacterStats
    {
        //드랍아이템
        public ItemDropTable dropTable;
        // Animator 참조
        Animator animator;

        // UIEnemyHealthBar 참조
        public UIEnemyHealthBar enemyHealthBar;

        public Rigidbody enemyRigidbody;
        public CapsuleCollider enemyCapsuleCollider;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 초기화
            enemyCapsuleCollider = GetComponent<CapsuleCollider>();
        }

        void Start()
        {
            // 최대 체력 설정
            maxHealth = SetMaxHealthFromHealthLevel();
            // 현재 체력 설정
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10; // 최대 레벨에 따른 최대 체력 설정
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage; // 현재 체력에서 데미지만큼 체력 감소

            enemyHealthBar.SetHealth(currentHealth); // Slider에도 적용

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            Debug.Log("적용된 데미지: " + damage);

            currentHealth = currentHealth - damage; // 현재 체력에서 데미지만큼 체력 감소
            enemyHealthBar.SetHealth(currentHealth); // Slider에도 적용

            animator.Play("Damaged"); // 피격 애니메이션 재생

            // 체력이 0 이하일 경우
            if (currentHealth <= 0)
            {
                currentHealth = 0; // 체력을 0으로 설정
                animator.Play("Death"); // 사망 애니메이션 재생
                isDead = true;
                dropTable.ItemDrop(transform.position);
                // Handle Palyer Death
                enemyRigidbody.useGravity = false;
                enemyCapsuleCollider.enabled = false;
            }
        }
    }
}
