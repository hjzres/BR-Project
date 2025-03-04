using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RenderHelper
    {
        private static readonly int chunkSize = 250;

        public struct PlaneChunk
        {
            public List<GameObject> chunkGameObjects;

            public Vector2 position;

            public GameObject gameObject;

            public Transform transform;

            private Transform parent;

            public PlaneChunk(Vector2 position)
            {
                this.chunkGameObjects = new List<GameObject>();
                this.position = position;
                this.gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                this.parent = new GameObject("Level Chunk").transform;
                this.gameObject.transform.parent = this.parent;
                this.transform = gameObject.transform;
                gameObject.transform.localScale = new Vector3(chunkSize, 0, chunkSize);
            }

            public void AddGeneratedObjectsToChunk(List<GameObject> gameObjects)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] == null)
                    {
                        gameObjects.Remove(gameObjects[i]);
                        Debug.Log("The GameObject " + gameObjects[i] + " is null, now removed from list and destroyed. Script: RenderHelper");
                        GameObject.Destroy(gameObjects[i]);
                    }

                    gameObjects[i].transform.parent = parent;
                    chunkGameObjects.Add(gameObjects[i]);
                    gameObjects.Remove(gameObjects[i]);
                }
            }

            public void SetVisiblity(bool condition)
            {
                bool visibility = condition ? false : true;
                for (int i = 0; i < parent.childCount; i++)
                {
                    parent.GetChild(i).gameObject.SetActive(visibility);
                }
            }
        }    
    }
}
