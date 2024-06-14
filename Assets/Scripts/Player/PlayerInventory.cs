using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class PlayerInventory : MonoBehaviour
    {
        // WeaponSlotManager 참조
        WeaponSlotManager weaponSlotManager;

        // WeaponItem 참조
        public WeaponItem leftWeapon; // 왼손 무기
        public WeaponItem rightWeapon; // 오른손 무기

        private void Awake()
        {
            weaponSlotManager = GetComponent<WeaponSlotManager>(); // WeaponSlotManager 컴포넌트 초기화
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }       
    }
}