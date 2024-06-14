using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        DamageCollider rightHandDamageCollider;
        DamageCollider leftHandDamageCollider;

        private void Awake()
        {
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
            }
        }

        private void Start()
        {
            LoadWeaponsOnHands();
        }

        public void LoadWeaponOnSlot(WeaponItem weapon)
        {
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider();
        }

        public void LoadWeaponsOnHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon);
            }
        }

        public void LoadWeaponsDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currrentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
    }
}
