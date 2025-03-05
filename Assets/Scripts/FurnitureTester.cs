using UnityEngine;
using NaughtyAttributes;

namespace Assets.Scripts
{
    public class FurnitureTester : MonoBehaviour
    {
        public GameObject table;
        public GameObject appliance;

        [Button]
        public void Test()
        {
            int randomLength = Random.Range(0, 10);
            GameObject[] appliances = new GameObject[]
            {
                appliance
            };

            FurnitureRandomizer.AddFurnitureElements(table, appliances, 1f, 2);
        }
    }
}
