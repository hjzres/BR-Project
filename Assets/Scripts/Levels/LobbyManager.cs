using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public enum ChunkType
{
    Maze,
    Pitfalls,
    Grid,
    Distorted
}

namespace Assets.Scripts.Levels
{
    public class LobbyManager : MonoBehaviour
    {
        public Transform playerTransform;

        [Header("CHUNKING PROPERTIES")]
        [Range(3, 10)] public int viewDistanceInChunks = 3;
        public int chunkSize = 150;
        [Min(1f)] public float rayDistance = 1;
        private Vector2 playerChunkCoord;
        private RaycastHit chunkHit = new RaycastHit();
        private Transform chunkParent;

        [Header("TESTING CONDITIONS")]
        public bool useChunking = true;

        public Dictionary<Vector2, Chunk> loadedChunks = new Dictionary<Vector2, Chunk>();
        private System.Random prng;

        public class Chunk
        {
            public GameObject meshObject;
            public readonly ChunkType id;

            public Chunk(Vector2 position, int size, System.Random prng, bool manualSpecialization = false, ChunkType chunkType = ChunkType.Maze)
            {
                meshObject = CreateChunk(position, size);

                if (!manualSpecialization)
                {
                    int rand = prng.Next(1, 100); // TODO: fix seeding cause it's not fully working and idfk why...
                    id = rand < 85 ? ChunkType.Maze : (rand >= 85 && rand < 95 ? ChunkType.Pitfalls : (rand >= 95 && rand <= 98 ? ChunkType.Grid : ChunkType.Distorted)); // TODO: use some actual algorithm.
                    meshObject.GetComponent<MeshRenderer>().material = SelectMaterial();
                }

                else
                {
                    id = chunkType;
                }
            }

            public GameObject CreateChunk(Vector2 position, int size)
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
                meshObject.GetComponent<MeshCollider>().sharedMesh = mesh;

                return meshObject;
            }

            private Material SelectMaterial() // Testing chunk type ONLY
            {
                return id == ChunkType.Maze ? GameManager.Instance.white : (id == ChunkType.Pitfalls ? GameManager.Instance.red : (id == ChunkType.Grid ? GameManager.Instance.green : GameManager.Instance.blue));
            }

            public void UpdateChunkVisibility() // TODO: check if player is too far from chunk edge ig
            {

            }
        }

        private void Awake()
        {
            loadedChunks.Clear(); // TODO: Hold data in save file instead of clearing.
            chunkParent = GameObject.Find("Chunks").transform;

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            prng = new System.Random(GameManager.Instance.seed);

            if (useChunking)
            {
                GenerateSpawnArea();
            }
        }

        private void LateUpdate()
        {
            if (useChunking)
            {
                UpdateChunks();   
            }
        }

        private void GenerateSpawnArea()
        {
            playerChunkCoord = Vector2.zero;

            for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
            {
                for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
                {
                    Vector2 coord = new Vector2(playerChunkCoord.x + (chunkSize * x), playerChunkCoord.y + (chunkSize * y));
                    Chunk newChunk = new Chunk(coord, chunkSize, prng);

                    loadedChunks.Add(coord, newChunk);
                    newChunk.meshObject.transform.parent = chunkParent;
                }
            }
        }

        // TODO: remove dependence on raycast and do proper math to obtain correct player chunk coordinate.
        private void UpdateChunks()
        {
            if (Physics.Raycast(playerTransform.position, Vector3.down, out chunkHit, rayDistance))
            {
                playerChunkCoord = new Vector2(chunkHit.collider.transform.position.x, chunkHit.collider.transform.position.z);

                for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
                {
                    for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
                    {
                        Vector2 coord = new Vector2(playerChunkCoord.x + (chunkSize * x), playerChunkCoord.y + (chunkSize * y));

                        if (!loadedChunks.ContainsKey(coord))
                        {
                            Chunk newChunk = new Chunk(coord, chunkSize, prng);

                            loadedChunks.Add(coord, newChunk);
                            newChunk.meshObject.transform.parent = chunkParent;
                            SpecializeChunk(newChunk);
                        }

                        else
                        {
                            loadedChunks[coord].UpdateChunkVisibility(); // Has to be implemented
                        }
                    }
                }
            }

            else
            {
                return;
            }
        }

        #region TEST FUNCTIONS

        [Button]
        public void GenerateChunk()
        {
            Chunk chunk = new Chunk(Vector2.zero, chunkSize, prng, true, ChunkType.Maze);
            SpecializeChunk(chunk);
            
        }

        #endregion

        private void SpecializeChunk(Chunk chunkData)
        {
            if (chunkData == null)
                return;

            switch (chunkData.id)
            {
                case ChunkType.Maze:
                    Debug.Log("Cased");
                    GenerateMaze();
                    break;

                case ChunkType.Pitfalls:
                    GeneratePitfalls();
                    break;

                case ChunkType.Grid:
                    GenerateGrid();
                    break;

                case ChunkType.Distorted:
                    GenerateDistortedMaze();
                    break;

                default:
                    Debug.LogError("Chunk " + chunkData.meshObject.name + "could not specialize.");
                    return;
            }
        }

        private void GenerateMaze()
        {
            Debug.Log("Working");
        }

        private void GeneratePitfalls()
        {

        }

        private void GenerateGrid()
        {

        }

        private void GenerateDistortedMaze()
        {

        }
    }
}
