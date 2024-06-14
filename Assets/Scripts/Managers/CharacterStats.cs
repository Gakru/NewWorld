using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class CharacterStats : MonoBehaviour
    {
        public int shieldLevel = 10; // ���� ����
        public float maxShield; // �ִ� ����
        public float currentShield; // ���� ����

        public int healthLevel = 10; // ü�� ����
        public int maxHealth; // �ִ� ü��
        public int currentHealth; // ���� ü��

        public int staminaLevel = 10; // ���¹̳� ����
        public float maxStamina; // �ִ� ���¹̳�
        public float currentStamina; // ���� ���¹̳�

        public bool isDead;
    }
}
