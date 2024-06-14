using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    // ������ ���� ����
    public class Item : ScriptableObject
    {
        [Header("Item Informations")]
        public Sprite itemIcon;
        public string itemName;
    }
}