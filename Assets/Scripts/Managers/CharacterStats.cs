using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class CharacterStats : MonoBehaviour
    {
        public int shieldLevel = 10; // 쉴드 레벨
        public float maxShield; // 최대 쉴드
        public float currentShield; // 현재 쉴드

        public int healthLevel = 10; // 체력 레벨
        public int maxHealth; // 최대 체력
        public int currentHealth; // 현재 체력

        public int staminaLevel = 10; // 스태미나 레벨
        public float maxStamina; // 최대 스태미나
        public float currentStamina; // 현재 스태미나

        public bool isDead;
    }
}
