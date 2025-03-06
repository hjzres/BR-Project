using UnityEngine;
using System.Collections.Generic;

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

        public enum SortingType
        {
            Horizontal,
            Vertical,
            Grid,
            Alternating
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

        public static void AddFurnitureElementsRandomly(GameObject parent, GameObject[] types, ElementType elementType, float surfacePercentage, int maxItemSpawns, float sidewaysHeightOffset = 0f)
        {
            Collider surfaceCollider = parent.GetComponent<Collider>();
            Vector3 surfaceExtents = surfaceCollider.bounds.extents;
            surfacePercentage *= 0.01f;

            List<GameObject> generatedElements = new List<GameObject>();

            for (int i = 0; i < types.Length; i++)
            {
                int amount = Random.Range(0, maxItemSpawns);
                Vector2 randomPos;
                Vector3 position;

                Vector3 typeExtents = types[i].GetComponent<Collider>().bounds.extents;

                if (elementType == ElementType.Sitting || elementType == ElementType.Hanging)
                {
                    float restrictionX = surfaceExtents.x * surfacePercentage;
                    float restrictionZ = surfaceExtents.z * surfacePercentage;
                    float typeOffsetY = types[i].GetComponent<Collider>().bounds.extents.y;

                    for (int j = 0; j < amount; j++)
                    {
                        randomPos = new Vector2(Random.Range(-restrictionX, restrictionX), Random.Range(-restrictionZ, restrictionZ));
                        position = new Vector3(randomPos.x, parent.transform.position.y + ((surfaceExtents.y + typeOffsetY) * ElementDirection(elementType)), randomPos.y);

                        generatedElements.Add(GameObject.Instantiate(types[i], position, Quaternion.identity));
                    }
                }

                else if (elementType == ElementType.Sideways)
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
                }
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
