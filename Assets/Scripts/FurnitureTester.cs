using UnityEngine;
using NaughtyAttributes;
using static Assets.Scripts.FurnitureHelper;
using static Assets.Scripts.Utilites;

namespace Assets.Scripts
{
    public class FurnitureTester : MonoBehaviour
    {
        [Header("Table Properties")]
        public float surfacePercentage = 100f;
        public Vector3 furnitureOffset;
        [Min(1)] public int amount;
        public GameObject table;
        public GameObject appliance1;
        public GameObject appliance2;
        public ElementType elementType;
        public Sorting.SortType sortingType;
        public Side side;

        [Button]
        public void TestFurniture()
        {
            GameObject[] appliances = new GameObject[]
            {
                appliance1
            };

            AddFurnitureElements(table, appliances, elementType, sortingType, furnitureOffset, surfacePercentage, amount, side);
        }
    }
}
