using UnityEngine;
using NaughtyAttributes;
using static Assets.Scripts.FurnitureRandomizer;

namespace Assets.Scripts
{
    public class FurnitureTester : MonoBehaviour
    {
        [Header("Table Properties")]
        public float surfacePercentage = 100f;
        public GameObject table;
        public GameObject appliance;
        public ElementType elementType;

        [Button]
        public void Test()
        {
            int randomLength = Random.Range(0, 10);
            GameObject[] appliances = new GameObject[]
            {
                appliance
            };

            AddFurnitureElementsRandomly(table, appliances, elementType, surfacePercentage, randomLength);
        }
    }
}
