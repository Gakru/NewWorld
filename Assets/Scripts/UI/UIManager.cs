using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class UIManager : MonoBehaviour
    {
        // PlayerInventory ����
        public PlayerInventory playerInventory;
        // EquipmentUI ����
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
            equipmentUI = FindObjectOfType<EquipmentUI>(); // EquipmentUI ������Ʈ ����
        }

        private void Start()
        {
            // WeaponInventorySlot ������Ʈ �ʱ�ȭ
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            // equipmentUI.LoadWeaponsOnEquipmentSlot(playerInventory); // EP.22
        }

        // UI ������Ʈ ó��
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
