using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class EquipmentUI : MonoBehaviour
    {
        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;

        // HandEquipmentSlot 참조
        HandEquipmentSlot[] handEquipmentSlots;

        private void Start()
        {
            handEquipmentSlots = GetComponentsInChildren<HandEquipmentSlot>(); // HandEquipmentSlot 컴포넌트 참조
        }

        /* EP.22
        public void LoadWeaponsOnEquipmentSlot(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlots.Length; i++)
            {
                if (handEquipmentSlots[i].rightHandSlot01)
                {
                    handEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (handEquipmentSlots[i].rightHandSlot02)
                {
                    handEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipmentSlots[i].rightHandSlot03)
                {
                    handEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[2]);
                }
            }
        }
        */

        public void SelectRightHandSlot01()
        {
            rightHandSlot01 = true;
        }
        public void SelectRightHandSlot02()
        {
            rightHandSlot02 = true;
        }
        public void SelectRightHandSlot03()
        {
            rightHandSlot03 = true;
        }
    }
}
