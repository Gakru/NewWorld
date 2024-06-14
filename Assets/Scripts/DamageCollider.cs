using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class DamageCollider : MonoBehaviour
    {
        // Collider ����
        Collider damagerCollider;

        public int currentWeaponDamage = 25; // ���� ���ϴ� ������

        private void Awake()
        {
            damagerCollider = GetComponent<Collider>(); // Collider ������Ʈ �ʱ�ȭ
            damagerCollider.gameObject.SetActive(true); // Collider�� Ȱ��ȭ�� gameObject ����
            damagerCollider.isTrigger = true; // Collider -> Trigger�� ����
            damagerCollider.enabled = false; // Collider ��Ȱ��ȭ
        }

        // ������ Ȱ��ȭ
        public void EnableDamageCollider()
        {
            damagerCollider.enabled = true; // Collider Ȱ��ȭ
        }
        // ������ ��Ȱ��ȭ
        public void DisableDamageCollider()
        {
            damagerCollider.enabled = false; // Collider ��Ȱ��ȭ
        }

        private void OnTriggerEnter(Collider collision)
        {
            // �浹�� ��ü�� �±װ� "Player"�� ���
            if (collision.tag == "Player")
            {
                Debug.Log("���� ������: " + currentWeaponDamage);
                // PlayerStats ������Ʈ ����
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
                // PlayerStats ������Ʈ�� �����ϸ�
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage); // Player���� ������ �ο�
                }
            }

            // �浹�� ��ü�� �±װ� "Enemy"�� ���
            if (collision.tag == "Enemy")
            {
                Debug.Log("����� ������: " + currentWeaponDamage); // �ο��� ������ ǥ��
                // EnemyStats ������Ʈ ����
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                // EnemyStats ������Ʈ�� �����ϸ�
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage); // Enemy���� ������ �ο�
                }
            }
        }
    }
}
