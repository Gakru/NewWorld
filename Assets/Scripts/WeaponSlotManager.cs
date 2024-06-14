using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class WeaponSlotManager : MonoBehaviour
    {
        // PlayerManager 참조
        PlayerManager playerManager;
        // PlayerInvenetory 참조
        PlayerInventory playerInventory;

        // WeaponItem 참조
        public WeaponItem attackingWeapon;

        // WeaponHolderSlot 참조
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        // DamageCollider 참조
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        // Animator 참조
        Animator animator;

        // QuickSlotsUI 참조
        QuickSlotsUI quickSlotsUI;

        // PlayerStats 참조
        PlayerStats playerStats;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>(); // PlayerManager 컴포넌트 초기화
            playerInventory = GetComponentInParent<PlayerInventory>(); // PlayerInventory 컴포넌트 초기화
            animator = GetComponent<Animator>(); // Animator 컴포넌트 초기화
            playerStats = GetComponentInParent<PlayerStats>(); // PlayerStats 컴포넌트 초기화
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>(); // QuickSlotsUI 컴포넌트 참조

            // WeaponHolderSlot 컴포넌트를 자식 오브젝트에서 가져오기
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                // 왼손 슬롯인지 오른손 슬롯인지 확인하여 설정
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        // 무기 로드 슬롯 처리
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                // 왼손 슬롯에 무기 모델 로드
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuaickSlotUI(weaponItem);

                #region Handle Left Weapon Idle Animation
                // 왼손 무기 대기 애니메이션 설정
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.leftHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                // 오른손 슬롯에 무기 모델 로드
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuaickSlotUI(weaponItem);

                #region Handle Right Weapon Idle Animation
                // 오른손 무기 대기 애니메이션 설정
                if (weaponItem != null)
                {
                    //animator.CrossFade(weaponItem.rightHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
        }

        #region Handle Weapon's Damage Collider
        // 무기의 대미지 콜라이더 처리
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currrentWeaponModel.GetComponentInChildren<DamageCollider>();
            // leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currrentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        }

        // 무기 데미지 콜라이더 활성화
        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        // 무기 데미지 콜라이더 비활성화
        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        // 무기 공격 시 스태미나 감소
        #region Weapon's Stamina Damage
        public void DecreaseStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina - attackingWeapon.lightAttackDecreaseStamina));
        }
        public void DecreaseStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina - attackingWeapon.heavyAttackDecreaseStamina));
        }
        #endregion
    }
}