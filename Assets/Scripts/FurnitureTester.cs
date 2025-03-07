using UnityEngine;
using NaughtyAttributes;
using static Assets.Scripts.FurnitureRandomizer;
using static Assets.Scripts.Utilites;

namespace Assets.Scripts
{
    public class FurnitureTester : MonoBehaviour
    {
        [Header("Table Properties")]
        public float surfacePercentage = 100f;
        public float heightOffset = 0f;
        public Vector3 offset;
        [Min(1)] public int length;
        public GameObject table;
        public GameObject appliance1;
        public GameObject appliance2;
        public ElementType elementType;
        public Sorting.SortType sortingType;

        [Button]
        public void Test()
        {
            GameObject[] appliances = new GameObject[]
            {
                appliance1
            };

            AddFurnitureElements(table, appliances, elementType, sortingType, offset, surfacePercentage, length);
        }
    }
}
