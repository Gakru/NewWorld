using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class HandEquipmentSlot : MonoBehaviour
    {
        // Image 참조
        public Image icon;
        // WeaponItem 참조
        WeaponItem weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;

        /* EP.22
        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        
        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
        */
    }
}
