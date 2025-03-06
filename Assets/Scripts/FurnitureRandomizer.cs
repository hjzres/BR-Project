using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

        public static void AddFurnitureElementsRandomly(GameObject parentSurface, GameObject[] types, ElementType elementType, float percentage, int maxItemSpawns)
        {
            percentage *= 0.01f;
            Collider surfaceCollider = parentSurface.GetComponent<Collider>();
            float surfaceOffset = surfaceCollider.bounds.extents.y; 

            for (int i = 0; i < types.Length; i++)
            {
                int amount = Random.Range(0, maxItemSpawns);
                float typeOffset;
                Vector2 randomPos;
                Vector3 position;

                if (elementType == ElementType.Sitting || elementType == ElementType.Hanging)
                {
                    float restrictionX = surfaceCollider.bounds.extents.x * percentage;
                    float restrictionZ = surfaceCollider.bounds.extents.z * percentage;
                    typeOffset = types[i].GetComponent<Collider>().bounds.extents.y;

                    for (int j = 0; j < amount; j++)
                    {
                        randomPos = new Vector2(Random.Range(-restrictionX, restrictionX), Random.Range(-restrictionZ, restrictionZ));
                        position = new Vector3(randomPos.x, parentSurface.transform.position.y + ((surfaceOffset + typeOffset) * ElementDirection(elementType)), randomPos.y);

                        GameObject.Instantiate(types[i], position, Quaternion.identity);
                    }
                }

                else if (elementType == ElementType.Sideways)
                {
                    Vector3 surfaceExtents = surfaceCollider.bounds.extents;
                    Vector3 typeExtents = types[i].GetComponent<Collider>().bounds.extents;

                    int halfAmount = amount / 2;

                    for (int j = 0; j < halfAmount; j++)
                    {
                        int randZ = Random.Range(0, 2);
                        float randomZ = Random.Range(-surfaceExtents.x, surfaceExtents.x);
                        float sideZ = randZ == 0 ? -surfaceExtents.z : surfaceExtents.z;

                        position = new Vector3(randomZ, parentSurface.transform.position.y, sideZ);

                        GameObject.Instantiate(types[i], position, Quaternion.identity);
                    }

                    for (int j = 0; j < (amount - halfAmount); j++)
                    {
                        int randX = Random.Range(0, 2);
                        float randomX = Random.Range(-surfaceExtents.z, surfaceExtents.z);
                        float sideX = randX == 0 ? -surfaceExtents.x : surfaceExtents.x;

                        position = new Vector3(sideX, parentSurface.transform.position.y, randomX);

                        GameObject.Instantiate(types[i], position, Quaternion.identity);
                    }
                }
            }

            static int ElementDirection(ElementType type)
            {
                if (type == ElementType.Hanging)
                {
                    return -1;
                }

                if (type == ElementType.Sideways)
                {

                }

                return 1;
            }
        }

        public static void AddFurnitureElementsFormally()
        {

        }
    }
}
