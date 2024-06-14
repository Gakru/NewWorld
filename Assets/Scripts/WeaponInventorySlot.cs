using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        // Image 참조
        public Image icon;
        // WeaponItem 참조
        WeaponItem item;

        // 아이템 추가(생성) 처리
        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        // 아이템 삭제(비우기) 처리
        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }


    }
}
