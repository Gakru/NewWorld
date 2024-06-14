using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    // 아이템 정보 저장
    public class Item : ScriptableObject
    {
        [Header("Item Informations")]
        public Sprite itemIcon;
        public string itemName;
    }
}