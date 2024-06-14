using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHC
{
    public class QuickSlotsUI : MonoBehaviour
    {
        // Image 참조
        public Image rightWeaponIcon;

        // 무기 슬롯 업데이트 처리
        public void UpdateWeaponQuaickSlotUI(WeaponItem weapon)
        {
            // 무기 아이콘에 무기 아이콘을 설정
            rightWeaponIcon.sprite = weapon.itemIcon;
            // 무기 아이콘 활성화
            rightWeaponIcon.enabled = true;

            if (weapon.itemIcon == null)
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
    }
}
