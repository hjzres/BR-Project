using UnityEngine;

namespace Assets.Scripts
{
    public static class Utilites
    {
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
