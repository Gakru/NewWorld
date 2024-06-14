using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        // GameObject(��������) ����
        public GameObject modelPrefab;
        public bool isUnarmed; // ����(����) ���� ����

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamage = 4;

        // ��� �ִϸ��̼�
        [Header("Idle Animations")]
        public string rightHandIdle; // ������ ��� �ִϸ��̼�
        public string leftHandIdle; // �޼� ��� �ִϸ��̼�

        // ���� �ִϸ��̼�
        [Header("Attack Animations")]
        public string OneHandLightAttack1; // ����� 1
        public string OneHandLightAttack2; // ����� 2
        public string OneHandLightAttack3; // ������ 1
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