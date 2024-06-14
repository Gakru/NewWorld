using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace KHC
{
    public class PlayerStats : CharacterStats
    {
        // PlayerManager ����
        PlayerManager playerManager;

        // ShieldBar & HealthBar & StaminaBar ����
        public ShieldBar shieldBar;
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        // AnimatorHandler ����
        PlayerAnimatorManager animatorHandler;

        // ���� ���
        public float shieldRegenAmount = 1f;
        public float shieldRegenTimer = 0f;
        // ���¹̳� ���
        public float staminaRegenAmount = 1f;
        public float staminaRegenTimer = 0f;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler ������Ʈ �ʱ�ȭ
        }

        void Start()
        {
            // �ִ� ü�� ����
            maxHealth = SetMaxHealthFromHealthLevel();
            // ���� ü�� ����
            currentHealth = maxHealth;
            healthBar.MaxHealth(maxHealth); // ü�� �ٿ� �ִ� ü���� ����
            healthBar.CurrentHealth(currentHealth); // ü�� �ٿ� ���� ü���� ����

            // �ִ� ���� ����
            maxShield = SetMaxShieldFromShieldLevel();
            // ���� ���� ����
            currentShield = maxShield;
            shieldBar.MaxShield(maxShield); // �ǵ� �ٿ� �ִ� �ǵ带 ����
            shieldBar.CurrentShield(currentShield); // �ǵ� �ٿ� ���� �ǵ带 ���� 

            // �ִ� ���¹̳� ����
            maxStamina = SetMaxStaminaFromHealthLevel();
            // ���� ���¹̳� ����
            currentStamina = maxStamina;
            staminaBar.MaxStamina(maxStamina); // ���¹̳� �ٿ� �ִ� ���¹̳��� ����
            staminaBar.CurrentStamina(currentStamina); // ���¹̳� �ٿ� ���� ���¹̳��� ����
        }
        
        // ���� �� ���� ����
        private float SetMaxShieldFromShieldLevel()
        {
            maxShield = shieldLevel * 10; // �ִ� ������ ���� �ִ� ���� ����
            return maxShield;
        }

        // ���� �� ü�� ����
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10; // �ִ� ������ ���� �ִ� ü�� ����
            return maxHealth;
        }

        // ���� �� ���¹̳� ����
        private float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10; // �ִ� ������ ���� �ִ� ���¹̳� ����
            return maxStamina;
        }


        // ������ ó��
        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable) return;
            if (isDead) return;

            if (currentShield > 0)
            {
                TakeHealthDamage(damage);
            }
            else
            {
                TakeHealthDamage(damage);
            }
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        // ������ ó��(����)
        public void TakeShieldDamage(int damage)
        {
            Debug.Log("���� ������(�ǵ�): " + damage);

            currentShield = currentShield - damage; // ���� ���忡�� ��������ŭ ���� ����

            shieldBar.CurrentShield(currentShield); // ���� ���� ���� ���� ������Ʈ

            animatorHandler.PlayAnimation("Damaged", true); // �ǰ� �ִϸ��̼� ���
        }

        // ������ ó��(ü��)
        public void TakeHealthDamage(int damage)
        {
            Debug.Log("���� ������(ü��): " + damage);

            currentHealth = currentHealth - damage; // ���� ü�¿��� ��������ŭ ü�� ����

            healthBar.CurrentHealth(currentHealth); // ü�� ���� ���� ü�� ������Ʈ

            animatorHandler.PlayAnimation("Damaged", true); // �ǰ� �ִϸ��̼� ���

            // ü���� 0 ������ ���
            if (currentHealth <= 0)
            {
                currentHealth = 0; // ü���� 0���� ����
                animatorHandler.PlayAnimation("Death", true); // ��� �ִϸ��̼� ���
                
                isDead = true;
            }
        }

        // ���¹̳� ó��
        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.CurrentStamina(currentStamina);
        }
        
        // �ǵ� ���
        public void RegenrateShield()
        {
            //if (playerManager.isInteracting)
            //{
            //    staminaRegenTimer = 0f;
            //}
            //else
            //{
            //    staminaRegenTimer += Time.deltaTime;

            //    if (currentShield <= maxShield && shieldRegenTimer > 1f)
            //    {
            //        currentShield += shieldRegenAmount * Time.deltaTime;
            //        shieldBar.CurrentShield(Mathf.RoundToInt(currentShield));
            //    }
            //}
            
            //if (!playerManager.isInteracting)
            //{
            //    currentShield += shieldRegenAmount * Time.deltaTime;
            //    shieldBar.CurrentShield(Mathf.RoundToInt(currentShield));
            //}
        }

        // ���¹̳� ���
        public void RegenerateStamina()
        {
            //Debug.Log($"�÷��̾� �ൿ ���� : {playerManager.isInteracting}");

            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0f;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                //Debug.Log($"staminaRegenTimer : {staminaRegenTimer}");

                if (currentStamina <= maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenAmount * Time.deltaTime;
                    staminaBar.CurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }

            //Debug.Log($"�÷��̾� �ൿ ���� : {playerManager.isInteracting}");
            //if (!playerManager.isInteracting)
            //{
            //    currentStamina += staminaRegenAmount * Time.deltaTime;

            //    int stamina = Mathf.Clamp(Mathf.RoundToInt(currentStamina), 0, 40);

            //    staminaBar.CurrentStamina(stamina);
            //}
        }
    }
}
