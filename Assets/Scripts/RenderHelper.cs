using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RenderHelper
    {
        private Vector2Int currentChunk;

        private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

        public struct Chunk
        {
            public GameObject gameObject;
            public Transform transform;
            public List<GameObject> elements;

            public Chunk(Vector2 position, int chunkLength)
            {
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                gameObject.GetComponent<MeshCollider>().enabled = false;
                gameObject.AddComponent<BoxCollider>();

                transform = gameObject.transform;
                elements = new List<GameObject>();

                transform.localScale = new Vector3(chunkLength, 1f, chunkLength);
                transform.position = new Vector3(position.x, 0, position.y);
            }
        }

        public void UpdateChunks(Transform reference, int chunkLength, int viewDistanceInChunks)
        {
            Debug.Log("Something");
            Vector2 pos = new Vector2(reference.position.x, reference.position.z);
            Vector2Int newChunk = new Vector2Int(Mathf.FloorToInt(pos.x / chunkLength), Mathf.FloorToInt(pos.y / chunkLength));

            if (newChunk != currentChunk)
            {
                currentChunk = newChunk;

                List<Vector2Int> generatedChunks = new List<Vector2Int>(chunks.Keys);

                for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
                {
                    for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
                    {
                        Vector2Int chunkCoordinate = new Vector2Int(x, y);

                        if (!chunks.ContainsKey(chunkCoordinate))
                        {
                            Vector2 position = new Vector2(chunkCoordinate.x, chunkCoordinate.y) * chunkLength * 10;
                            Chunk chunk = new Chunk(position, chunkLength);
                            chunks.Add(chunkCoordinate, chunk);
                        }

                        generatedChunks.Remove(chunkCoordinate);
                    }
                }
            }
        }
    }
}
