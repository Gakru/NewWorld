using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25; // Player가 받는 데미지

        private void OnTriggerEnter(Collider other)
        {
            // PlayerStats 컴포넌트 참조
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // PlayerStats 컴포넌트가 존재하면
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage); // Player에게 대미지 부여
            }
        }
    }
}
