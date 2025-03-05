using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RenderHelper
    {
        public struct PlaneChunk
        {
            public List<GameObject> chunkGameObjects;

            public Vector2 position;

            public GameObject gameObject;

            public Transform transform;

            private Transform parent;

            public PlaneChunk(Vector2 position, int size)
            {
                this.chunkGameObjects = new List<GameObject>();
                this.position = position;
                this.gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                this.parent = new GameObject("Level Chunk").transform;
                this.gameObject.transform.parent = this.parent;
                this.transform = gameObject.transform;
                this.gameObject.transform.position = new Vector3(position.x, 0, position.y);
                gameObject.transform.localScale = new Vector3(size, 0, size);
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
