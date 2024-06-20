using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace KHC
{
    [CreateAssetMenu]
    public class ItemDropTable : ScriptableObject
    {
        [System.Serializable]
        public class Items
        {
            public DropItem item;
            public WeaponItem weapon;
            public int weight;
        }

        public List<Items> items = new List<Items>();

        protected DropItem PickItem()
        {
            int sum = 0;
            foreach (var item in items)
            {
                sum += item.weight;

            }

            var rnd = Random.Range(0, sum);

            for(int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.weight > rnd) return items[i].item;
                else rnd -= item.weight;
            }

            return null;
        }

        public void ItemDrop(Vector3 pos)
        {
            var item = PickItem();
            if (item == null)
            {
                Debug.Log("노드랍");
                return;
            }

            Instantiate(item.prefab, pos, Quaternion.identity);
            Debug.Log("드랍");
        }
    }

}
