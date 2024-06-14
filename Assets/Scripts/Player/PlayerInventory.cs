using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class PlayerInventory : MonoBehaviour
    {
        // WeaponSlotManager ����
        WeaponSlotManager weaponSlotManager;

        // WeaponItem ����
        public WeaponItem leftWeapon; // �޼� ����
        public WeaponItem rightWeapon; // ������ ����

        private void Awake()
        {
            weaponSlotManager = GetComponent<WeaponSlotManager>(); // WeaponSlotManager ������Ʈ �ʱ�ȭ
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }       
    }
}