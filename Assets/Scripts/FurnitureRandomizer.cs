using UnityEngine;

namespace Assets.Scripts
{
    public static class FurnitureRandomizer
    {
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

        public static void AddFurnitureElements(GameObject parentSurface, GameObject[] types, float percentage, int maxItemSpawns)
        {
            Collider surfaceCollider = parentSurface.GetComponent<Collider>();
            float surfaceOffset = surfaceCollider.bounds.extents.y;
            
            float restrictionX = surfaceCollider.bounds.extents.x * percentage;
            float restrictionZ = surfaceCollider.bounds.extents.z * percentage;

            for (int i = 0; i < types.Length; i++)
            {
                int amount = Random.Range(0, maxItemSpawns);
                float typeOffset = types[i].GetComponent<Collider>().bounds.extents.y;

                for (int j = 0; j < amount; j++)
                {
                    Vector2 randomPos = new Vector2(Random.Range(-restrictionX, restrictionX), Random.Range(-restrictionZ, restrictionZ));
                    GameObject.Instantiate(types[0], new Vector3(randomPos.x, parentSurface.transform.position.y + surfaceOffset + typeOffset, randomPos.y), Quaternion.identity);
                }
            }
        }
    }
}
