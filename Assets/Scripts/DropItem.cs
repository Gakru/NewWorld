using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KHC
{
    [CreateAssetMenu(menuName = "Item/Drop Item")]
    public class DropItem : Item
    {
        
        public int level;

        [System.Serializable]
        public struct STAT
        {
            public string name; //아이템이름
            public int value;  //밸류
        }

        public List<STAT> stats = new List<STAT>();

        public int maxStack; //최대 소지 개수
        public int price; //가격

        
        public Transform prefab; //몬스터가 죽은자리에 떨어진 아이템을 표시

        [Multiline]
        public string description; //아이템 설명


    }
}
