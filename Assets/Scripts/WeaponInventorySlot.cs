using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        // Image ����
        public Image icon;
        // WeaponItem ����
        WeaponItem item;

        // ������ �߰�(����) ó��
        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        // ������ ����(����) ó��
        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }


    }
}
