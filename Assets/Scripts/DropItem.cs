using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KHC
{
    [CreateAssetMenu]
    public class DropItem : ScriptableObject
    {

        public string itemName;
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

        public Sprite icon;
        public GameObject prefab; //���Ͱ� �����ڸ��� ������ �������� ǥ��

        [Multiline]
        public string description; //������ ����


    }
}
