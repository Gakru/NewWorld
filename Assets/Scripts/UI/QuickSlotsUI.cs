using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class QuickSlotsUI : MonoBehaviour
    {
        // Image ����
        public Image rightWeaponIcon;

        // ���� ���� ������Ʈ ó��
        public void UpdateWeaponQuaickSlotUI(WeaponItem weapon)
        {
            // ���� �����ܿ� ���� �������� ����
            rightWeaponIcon.sprite = weapon.itemIcon;
            // ���� ������ Ȱ��ȭ
            rightWeaponIcon.enabled = true;

            if (weapon.itemIcon == null)
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
    }
}
