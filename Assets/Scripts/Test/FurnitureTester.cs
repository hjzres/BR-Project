using UnityEngine;
using NaughtyAttributes;
using static Assets.Scripts.FurnitureHelper;
using static Assets.Scripts.Utilites;

namespace Assets.Scripts.Test
{
    public class FurnitureTester : MonoBehaviour
    {
        public GameObject parentFurniture;

        [Header("Furniture Element Tester")]
        public GameObject[] elements = new GameObject[1];
        [Min(1)] public int elementAmount;
        [Range(1, 100)] public float surfacePercentage = 100f;
        public Vector3 elementOffset;
        public ElementType elementType;
        public Sorting.SortType sortingType;
        public WallSide wallSide; // For sideways elements

        [Button]
        public void TestFurniture()
        {
            AddFurnitureElements(parentFurniture, elements, elementAmount, surfacePercentage, elementOffset, elementType, sortingType, wallSide);
        }

        [Button]
        public void ClearGeneratedElements()
        {
            if (parentFurniture.transform.childCount == 0)
            {
                return;
            }

            foreach (Transform child in parentFurniture.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
