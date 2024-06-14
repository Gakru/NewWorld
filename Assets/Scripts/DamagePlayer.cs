using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25; // Player�� �޴� ������

        private void OnTriggerEnter(Collider other)
        {
            // PlayerStats ������Ʈ ����
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // PlayerStats ������Ʈ�� �����ϸ�
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage); // Player���� ����� �ο�
            }
        }
    }
}
