using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace KHC
{
    public class PlayerStats : CharacterStats
    {
        // PlayerManager 참조
        PlayerManager playerManager;

        // ShieldBar & HealthBar & StaminaBar 참조
        public ShieldBar shieldBar;
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        // AnimatorHandler 참조
        PlayerAnimatorManager animatorHandler;

        // 쉴드 재생
        public float shieldRegenAmount = 1f;
        public float shieldRegenTimer = 0f;
        // 스태미나 재생
        public float staminaRegenAmount = 1f;
        public float staminaRegenTimer = 0f;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>(); // AnimatorHandler 컴포넌트 초기화
        }

        void Start()
        {
            // 최대 체력 설정
            maxHealth = SetMaxHealthFromHealthLevel();
            // 현재 체력 설정
            currentHealth = maxHealth;
            healthBar.MaxHealth(maxHealth); // 체력 바에 최대 체력을 설정
            healthBar.CurrentHealth(currentHealth); // 체력 바에 현재 체력을 설정

            // 최대 쉴드 설정
            maxShield = SetMaxShieldFromShieldLevel();
            // 현재 쉴드 설정
            currentShield = maxShield;
            shieldBar.MaxShield(maxShield); // 실드 바에 최대 실드를 설정
            shieldBar.CurrentShield(currentShield); // 실드 바에 현재 실드를 설정 

            // 최대 스태미나 설정
            maxStamina = SetMaxStaminaFromHealthLevel();
            // 현재 스태미나 설정
            currentStamina = maxStamina;
            staminaBar.MaxStamina(maxStamina); // 스태미나 바에 최대 스태미나를 설정
            staminaBar.CurrentStamina(currentStamina); // 스태미나 바에 현재 스태미나를 설정
        }
        
        // 레벨 별 쉴드 설정
        private float SetMaxShieldFromShieldLevel()
        {
            maxShield = shieldLevel * 10; // 최대 레벨에 따른 최대 쉴드 설정
            return maxShield;
        }

        // 레벨 별 체력 설정
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10; // 최대 레벨에 따른 최대 체력 설정
            return maxHealth;
        }

        // 레벨 별 스태미나 설정
        private float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10; // 최대 레벨에 따른 최대 스태미나 설정
            return maxStamina;
        }


        // 데미지 처리
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

        // 데미지 처리(쉴드)
        public void TakeShieldDamage(int damage)
        {
            Debug.Log("받은 데미지(실드): " + damage);

            currentShield = currentShield - damage; // 현재 쉴드에서 데미지만큼 쉴드 감소

            shieldBar.CurrentShield(currentShield); // 쉴드 바의 현재 쉴드 업데이트

            animatorHandler.PlayAnimation("Damaged", true); // 피격 애니메이션 재생
        }

        // 데미지 처리(체력)
        public void TakeHealthDamage(int damage)
        {
            Debug.Log("받은 데미지(체력): " + damage);

            currentHealth = currentHealth - damage; // 현재 체력에서 데미지만큼 체력 감소

            healthBar.CurrentHealth(currentHealth); // 체력 바의 현재 체력 업데이트

            animatorHandler.PlayAnimation("Damaged", true); // 피격 애니메이션 재생

            // 체력이 0 이하일 경우
            if (currentHealth <= 0)
            {
                currentHealth = 0; // 체력을 0으로 설정
                animatorHandler.PlayAnimation("Death", true); // 사망 애니메이션 재생
                
                isDead = true;
            }
        }

        // 스태미나 처리
        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.CurrentStamina(currentStamina);
        }
        
        // 실드 재생
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

        // 스태미나 재생
        public void RegenerateStamina()
        {
            //Debug.Log($"플레이어 행동 상태 : {playerManager.isInteracting}");

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

            //Debug.Log($"플레이어 행동 상태 : {playerManager.isInteracting}");
            //if (!playerManager.isInteracting)
            //{
            //    currentStamina += staminaRegenAmount * Time.deltaTime;

            //    int stamina = Mathf.Clamp(Mathf.RoundToInt(currentStamina), 0, 40);

            //    staminaBar.CurrentStamina(stamina);
            //}
        }
    }
}
