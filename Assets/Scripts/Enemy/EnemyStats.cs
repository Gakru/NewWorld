using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KHC
{
    public class EnemyStats : CharacterStats
    {
        //���������
        public ItemDropTable dropTable;
        // Animator ����
        Animator animator;

        // UIEnemyHealthBar ����
        public UIEnemyHealthBar enemyHealthBar;

        public Rigidbody enemyRigidbody;
        public CapsuleCollider enemyCapsuleCollider;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>(); // Animator ������Ʈ �ʱ�ȭ
            enemyCapsuleCollider = GetComponent<CapsuleCollider>();
        }

        void Start()
        {
            // �ִ� ü�� ����
            maxHealth = SetMaxHealthFromHealthLevel();
            // ���� ü�� ����
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10; // �ִ� ������ ���� �ִ� ü�� ����
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage; // ���� ü�¿��� ��������ŭ ü�� ����

            enemyHealthBar.SetHealth(currentHealth); // Slider���� ����

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            Debug.Log("����� ������: " + damage);

            currentHealth = currentHealth - damage; // ���� ü�¿��� ��������ŭ ü�� ����
            enemyHealthBar.SetHealth(currentHealth); // Slider���� ����

            animator.Play("Damaged"); // �ǰ� �ִϸ��̼� ���

            // ü���� 0 ������ ���
            if (currentHealth <= 0)
            {
                currentHealth = 0; // ü���� 0���� ����
                animator.Play("Death"); // ��� �ִϸ��̼� ���
                isDead = true;
                dropTable.ItemDrop(transform.position);
                // Handle Palyer Death
                enemyRigidbody.useGravity = false;
                enemyCapsuleCollider.enabled = false;
            }
        }
    }
}
