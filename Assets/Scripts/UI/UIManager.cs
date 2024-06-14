using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class UIManager : MonoBehaviour
    {
        // PlayerInventory 참조
        public PlayerInventory playerInventory;
        // EquipmentUI 참조
        EquipmentUI equipmentUI;

        // UI
        [Header("UI")]
        public GameObject hudUI;
        public GameObject selectUI;
        public GameObject weaponInventoryUI;

        // Weapon Inventory
        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Awake()
        {
            equipmentUI = FindObjectOfType<EquipmentUI>(); // EquipmentUI 컴포넌트 참조
        }

        private void Start()
        {
            // WeaponInventorySlot 컴포넌트 초기화
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            // equipmentUI.LoadWeaponsOnEquipmentSlot(playerInventory); // EP.22
        }

        // UI 업데이트 처리
        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            /* ~EP.21
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInvnetory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            */

            #endregion
        }

        public void OpenSelectWindow()
        {
            selectUI.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectUI.SetActive(false);
        }

        public void CloseAllInventoryUI()
        {
            weaponInventoryUI.SetActive(false);
        }
    }
}
