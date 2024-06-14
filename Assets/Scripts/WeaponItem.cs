using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        // GameObject(모델프리팹) 참조
        public GameObject modelPrefab;
        public bool isUnarmed; // 무장(비무장) 상태 여부

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamage = 4;

        // 대기 애니메이션
        [Header("Idle Animations")]
        public string rightHandIdle; // 오른손 대기 애니메이션
        public string leftHandIdle; // 왼손 대기 애니메이션

        // 공격 애니메이션
        [Header("Attack Animations")]
        public string OneHandLightAttack1; // 약공격 1
        public string OneHandLightAttack2; // 약공격 2
        public string OneHandLightAttack3; // 강공격 1
        public string OneHandLightAttack4;
        public string OneHandLightAttack5;
        public string OneHandLightAttack6;
        public string OneHandLightAttack7;

        // EP.16
        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackDecreaseStamina;
        public float heavyAttackDecreaseStamina;
    }
}