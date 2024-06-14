using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class WeaponSlotManager : MonoBehaviour
    {
        // PlayerManager ����
        PlayerManager playerManager;
        // PlayerInvenetory ����
        PlayerInventory playerInventory;

        // WeaponItem ����
        public WeaponItem attackingWeapon;

        // WeaponHolderSlot ����
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        // DamageCollider ����
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        // Animator ����
        Animator animator;

        // QuickSlotsUI ����
        QuickSlotsUI quickSlotsUI;

        // PlayerStats ����
        PlayerStats playerStats;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>(); // PlayerManager ������Ʈ �ʱ�ȭ
            playerInventory = GetComponentInParent<PlayerInventory>(); // PlayerInventory ������Ʈ �ʱ�ȭ
            animator = GetComponent<Animator>(); // Animator ������Ʈ �ʱ�ȭ
            playerStats = GetComponentInParent<PlayerStats>(); // PlayerStats ������Ʈ �ʱ�ȭ
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>(); // QuickSlotsUI ������Ʈ ����

            // WeaponHolderSlot ������Ʈ�� �ڽ� ������Ʈ���� ��������
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                // �޼� �������� ������ �������� Ȯ���Ͽ� ����
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

        // ���� �ε� ���� ó��
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                // �޼� ���Կ� ���� �� �ε�
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuaickSlotUI(weaponItem);

                #region Handle Left Weapon Idle Animation
                // �޼� ���� ��� �ִϸ��̼� ����
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
                // ������ ���Կ� ���� �� �ε�
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuaickSlotUI(weaponItem);

                #region Handle Right Weapon Idle Animation
                // ������ ���� ��� �ִϸ��̼� ����
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
        // ������ ����� �ݶ��̴� ó��
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

        // ���� ������ �ݶ��̴� Ȱ��ȭ
        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        // ���� ������ �ݶ��̴� ��Ȱ��ȭ
        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        // ���� ���� �� ���¹̳� ����
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