using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class RenderHelper
    {
        public Dictionary<Vector2, Chunk> loadedChunks = new Dictionary<Vector2, Chunk>();

        public class Chunk
        {
            public GameObject meshObject;

            public Chunk(Vector2 position, int size, Material material)
            {
                meshObject = CreateChunk(position, size, material);
            }

            public static GameObject CreateChunk(Vector2 position, int size, Material material)
            {
                // Specifically set for optimization, two triangles is enough
                Vector3[] vertices = new Vector3[4];
                int[] triangles = new int[6];
                float halfSize = size * 0.5f; // Used to center GameObject with position cursor.

                GameObject meshObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                Vector3 meshPosition = meshObject.transform.position;
                meshObject.transform.position = new Vector3(position.x, 0, position.y);

                vertices[0] = new Vector3(meshPosition.x - halfSize, 0, meshPosition.z + size - halfSize);
                vertices[1] = new Vector3(meshPosition.x + size - halfSize, 0, meshPosition.z + size - halfSize);
                vertices[2] = new Vector3(meshPosition.x - halfSize, 0, meshPosition.z - halfSize);
                vertices[3] = new Vector3(meshPosition.x + size - halfSize, 0, meshPosition.z - halfSize);
                
                triangles[0] = 0;
                triangles[1] = 1;
                triangles[2] = 2;
                triangles[3] = 2;
                triangles[4] = 1;
                triangles[5] = 3;

                Mesh mesh = new Mesh();
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.RecalculateNormals();

                meshObject.GetComponent<MeshFilter>().mesh = mesh;
                meshObject.GetComponent<MeshRenderer>().material = material;
                meshObject.GetComponent<MeshCollider>().sharedMesh = mesh;

                return meshObject;
            }

            public void UpdateChunkVisibility() // TODO: check if player is too far from chunk edge ig
            {

            }
        }
    }
}
