using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RenderHelper
    {
        public Dictionary<float, Chunk> loadedChunks = new Dictionary<float, Chunk>();

        private RaycastHit hit;

        public struct Chunk
        {
            public GameObject meshObject;

            public Chunk(Vector2 position, Material material, int size)
            {
                meshObject = CreateChunk(position, material, size);
            }

            public static GameObject CreateChunk(Vector2 position, Material material, int size = 1)
            {
                // Specifically set for optimization, two triangles is enough
                Vector3[] vertices = new Vector3[4];
                Vector2[] uvs = new Vector2[vertices.Length];
                int[] triangles = new int[6];
                float halfSize = size * 0.5f; // Used to center GameObject with position cursor.

                GameObject meshObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
                Vector3 meshPosition = meshObject.transform.position;
                meshObject.transform.position = new Vector3(position.x, 0, position.y);

                vertices[0] = new Vector3(meshPosition.x - halfSize, 0, meshPosition.z + size - halfSize);
                vertices[1] = new Vector3(meshPosition.x + size - halfSize, 0, meshPosition.z + size - halfSize);
                vertices[2] = new Vector3(meshPosition.x - halfSize, 0, meshPosition.z - halfSize);
                vertices[3] = new Vector3(meshPosition.x + size - halfSize, 0, meshPosition.z - halfSize);

                for (int i = 0; i < vertices.Length; i++)
                {
                    uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                }
                
                triangles[0] = 0;
                triangles[1] = 1;
                triangles[2] = 2;
                triangles[3] = 2;
                triangles[4] = 1;
                triangles[5] = 3;

                Mesh mesh = new Mesh();
                mesh.vertices = vertices;
                mesh.uv = uvs;
                mesh.triangles = triangles;
                mesh.RecalculateNormals();

                meshObject.GetComponent<MeshFilter>().mesh = mesh;
                meshObject.GetComponent<MeshRenderer>().material = material;

                return meshObject;
            }

            public void SetVisibility(bool visible)
            {
                meshObject.SetActive(visible);
            }
        }

        public void UpdateChunks(Vector3 focusPosition)
        {
            Vector2 localXZPos = new Vector2(focusPosition.x, focusPosition.z);
            GameManager instance = GameManager.instance;

            float playerToChunkCoordX = PlayerToChunkCoordinates(focusPosition);
            Debug.Log(playerToChunkCoordX);

            for (int i = -instance.viewDistanceInChunks; i <= instance.viewDistanceInChunks; i++)
            {
                float chunkCoordX = playerToChunkCoordX + (instance.size * i);

                if (!loadedChunks.ContainsKey(chunkCoordX))
                {
                    loadedChunks.Add(chunkCoordX, new Chunk(new Vector2(chunkCoordX, 0), instance.white, instance.size));
                } 
            }
        }

        // TODO: Change to a mathematical checker instead of raycasts
        private float PlayerToChunkCoordinates(Vector3 localFocusPosition)
        {
            if (Physics.Raycast(localFocusPosition, Vector3.up, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
            
            return 0f; 
        }























        /*
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

        public void UpdateChunking(Transform reference)
        {
            // TODO
        }
        */
    }
}
