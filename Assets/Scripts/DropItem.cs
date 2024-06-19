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
            public string name; //�������̸�
            public int value;  //���
        }

        public List<STAT> stats = new List<STAT>();

        public int maxStack; //�ִ� ���� ����
        public int price; //����

        
        public Transform prefab; //���Ͱ� �����ڸ��� ������ �������� ǥ��

        [Multiline]
        public string description; //������ ����


    }
}
