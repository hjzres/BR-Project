using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Assets.Scripts
{
    public static class Utilites
    {
        public struct Sorting
        {
            public enum SortType
            {
                Random,
                Horizontal,
                Vertical,
                Diagonal,
                Grid,
                Noise
            }

            public static Vector2 SortingHelper(SortType type, GameObject objectToSort, Vector2 restriction, int amount, int iteration = 0)
            {
                Vector2 position = new Vector2();
                Vector3 objExtents = objectToSort.GetComponent<Collider>().bounds.extents;

                if (type == SortType.Random)
                {
                    position = new Vector2(Random.Range(-restriction.x, restriction.x), Random.Range(-restriction.y, restriction.y));
                }

                else if (type == SortType.Horizontal || type == SortType.Vertical)
                {
                    int horizontalMultiplier = (type == SortType.Diagonal || type == SortType.Horizontal) ? 1 : 0; 
                    int verticalMultiplier = (type == SortType.Diagonal || type == SortType.Vertical) ? 1 : 0;
                        
                    float domain = ObtainRestrictionValue(restriction) * 2f;
                    float spacing = domain / amount;
                    float pos = (iteration - (amount - 1) / 2f) * spacing;

                    position = new Vector2(pos * horizontalMultiplier, pos * verticalMultiplier);
                }

                else if (type == SortType.Grid)
                {

                }

                else if (type == SortType.Noise)
                {

                }

                return position;
            }

            private static float ObtainRestrictionValue(Vector2 restriction)
            {
                if (restriction.x == 0)
                {
                    return restriction.y;
                }

                return restriction.x;
            }
        }

        public struct Easing
        {
            public enum EasingType
            {

            }
        }

        public static Transform[] RetrieveTransformsByTag(string tag)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            Transform[] transforms = new Transform[gameObjects.Length];

            if (gameObjects.Length <= 0 || gameObjects == null)
            {
                return null;
            }

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                {
                    transforms[i] = null;
                }

                transforms[i] = gameObjects[i].transform;
            }

            return transforms;
        }
    }
}
