using UnityEngine;
using System.Collections.Generic;
using static Assets.Scripts.Utilites;

namespace Assets.Scripts
{
    public static class FurnitureRandomizer
    {
        public enum ElementType
        {
            Sitting,
            Hanging,
            Sideways
        }

        public static int GenerateSeed(int maxDigits)
        {
            System.Random random = new System.Random();
            int seedOperation = 10 ^ maxDigits;

            if (seedOperation > int.MaxValue)
            {
                return random.Next(Mathf.RoundToInt(int.MaxValue / 1000));
            }

            return random.Next(seedOperation);
        }

        public static void AddFurnitureElements(GameObject parent, GameObject[] types, ElementType elementType, Sorting.SortType sortingType, Vector3 offset, float surfacePercentage, int amount, float sidewaysHeightOffset = 0f)
        {
            Collider surfaceCollider = parent.GetComponent<Collider>();
            Vector3 surfaceExtents = surfaceCollider.bounds.extents;
            surfacePercentage *= 0.01f;

            List<GameObject> generatedElements = new List<GameObject>();

            for (int i = 0; i < types.Length; i++)
            {
                Vector3 position;
                Vector3 typeExtents = types[i].GetComponent<Collider>().bounds.extents;

                float typeOffsetY = typeExtents.y;

                if (elementType == ElementType.Sitting || elementType == ElementType.Hanging)
                {
                    Vector2 restrictions = new Vector2(surfaceExtents.x, surfaceExtents.z) * surfacePercentage;

                    for (int j = 0; j < amount; j++)
                    {
                        GameObject element = GameObject.Instantiate(types[i], null);
                        position = Sorting.SortingHelper(sortingType, element, restrictions, amount, j);
                        element.transform.position = new Vector3(position.x + parent.transform.position.x, parent.transform.position.y + (surfaceExtents.y + typeOffsetY) * ElementDirection(elementType), position.y + parent.transform.position.z) + offset;
                        generatedElements.Add(element);
                    }
                }


                /*if (elementType == ElementType.Sideways)
                {
                    int halfAmount = amount / 2;
                    int rand;
                    float randAxis;
                    float side;
                    float typeOffsetX;
                    float typeOffsetZ;

                    for (int j = 0; j < halfAmount; j++)
                    {
                        rand = Random.Range(0, 2);
                        randAxis = Random.Range(-surfaceExtents.x, surfaceExtents.x);
                        side = rand == 0 ? -surfaceExtents.z : surfaceExtents.z;
                        typeOffsetZ = rand == 0 ? -typeExtents.z : typeExtents.z;

                        position = new Vector3(randAxis * surfacePercentage, parent.transform.position.y + sidewaysHeightOffset, side + typeOffsetZ);

                        generatedElements.Add(GameObject.Instantiate(types[i], position, Quaternion.identity));
                    }

                    for (int j = 0; j < (amount - halfAmount); j++)
                    {
                        rand = Random.Range(0, 2);
                        randAxis = Random.Range(-surfaceExtents.z, surfaceExtents.z);
                        side = rand == 0 ? -surfaceExtents.x : surfaceExtents.x;
                        typeOffsetX = rand == 0 ? -typeExtents.x : typeExtents.x;

                        position = new Vector3(side + typeOffsetX, parent.transform.position.y + sidewaysHeightOffset, randAxis * surfacePercentage);

                        generatedElements.Add(GameObject.Instantiate(types[i], position, Quaternion.identity));
                    }
                }*/
            }

            // IMPLEMENT SCRIPTABLE OBJECT FEATURES FOR EXTRA FEATURES (ex. Naming).
            for (int i = 0; i < generatedElements.Count; i++)
            {
                Transform parentGameObject = parent.transform;
                generatedElements[i].transform.parent = parentGameObject;
            }

            static int ElementDirection(ElementType type)
            {
                if (type == ElementType.Hanging)
                {
                    return -1;
                }

                return 1;
            }
        }

        public static void AddFurnitureElementsFormally(GameObject parent, GameObject[] types)
        {

        }
    }
}
