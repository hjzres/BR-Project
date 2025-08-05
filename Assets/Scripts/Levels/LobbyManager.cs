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

        [Header("MAZE PROPERTIES")]
        [Range(0, 1)] public float perlinThreshold;
        public int maxStartPoints = 5;

        [Header("TESTING CONDITIONS")]
        public bool drawGizmos;
        public bool useChunking = true;
        public bool useManualSpecialization = false;
        public ChunkType testType;
        public Material white;

        public Dictionary<Vector2, Chunk> loadedChunks = new Dictionary<Vector2, Chunk>();
        public System.Random prng;

        #region CHUNK-RELATED

        public class Chunk
        {
            public GameObject meshObject;
            public Material material;
            public ChunkType id;

            public Chunk(Vector2 position, int size)
            {
                meshObject = CreateChunk(position, size);
                material = meshObject.GetComponent<MeshRenderer>().sharedMaterial;
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

            public void UpdateChunkVisibility() // TODO: check if player is too far from chunk edge ig
            {

            }

            public Vector2 RetrieveDimensionsInWorldSpace(bool getWidth, bool getHeight, int chunkSize)
            {
                if (this == null)
                {
                    Debug.LogError("Cannot calculate dimensions as the chunk is not generated!");
                    return Vector2.zero;
                }

                int halfSize = chunkSize / 2;

                Vector3 chunkPosition = meshObject.transform.position;
                float min = getWidth ? chunkPosition.x - halfSize : chunkPosition.y - halfSize;
                float max = getWidth ? chunkPosition.x + halfSize : chunkPosition.y + halfSize;

                return new Vector2(min, max);
            }
        }

        #endregion

        private void Awake()
        {
            loadedChunks.Clear(); // TODO: Hold data in save file instead of clearing.
            chunkParent = GameObject.Find("Chunks").transform;

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            prng = new System.Random(GameManager.Instance.seed);

            GenerateSpawnArea();
        }

        private void LateUpdate()
        {
            UpdateChunks();
        }

        private void GenerateSpawnArea()
        {
            playerChunkCoord = Vector2.zero;
            HandleChunking();
        }

        // TODO: remove dependence on raycast and do proper math to obtain correct player chunk coordinate.
        private void UpdateChunks()
        {
            if (Physics.Raycast(playerTransform.position, Vector3.down, out chunkHit, rayDistance))
            {
                playerChunkCoord = new Vector2(chunkHit.collider.transform.position.x, chunkHit.collider.transform.position.z);
                HandleChunking();
            }

            return;
        }

        private void HandleChunking()
        {
            if (useChunking)
            {
                for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
                {
                    for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
                    {
                        Vector2 coord = new Vector2(playerChunkCoord.x + (chunkSize * x), playerChunkCoord.y + (chunkSize * y));

                        if (!loadedChunks.ContainsKey(coord))
                        {
                            Chunk newChunk = new Chunk(coord, chunkSize);
                            newChunk.id = SelectChunkType();
                            newChunk.meshObject.GetComponent<MeshRenderer>().sharedMaterial = SelectMaterial(newChunk.id);

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
        }

        private ChunkType SelectChunkType()
        {
            if (useManualSpecialization)
            {
                return testType;
            }

            int randNum = prng.Next(1, 100);

            return randNum < 85 ? ChunkType.Maze : (randNum >= 85 && randNum < 95 ? ChunkType.Pitfalls : (randNum >= 95 && randNum <= 98 ? ChunkType.Grid : ChunkType.Distorted)); // TODO: use some actual algorithm.
        }

        private Material SelectMaterial(ChunkType chunkType)
        {
            if (useManualSpecialization)
            {
                return white;
            }

            return chunkType == ChunkType.Maze ? GameManager.Instance.white : (chunkType == ChunkType.Pitfalls ? GameManager.Instance.red : (chunkType == ChunkType.Grid ? GameManager.Instance.blue : GameManager.Instance.green));
        }

        #region TEST FUNCTIONS

        [Button]
        public void GenerateChunk()
        {
            Chunk chunk = new Chunk(Vector2.zero, chunkSize);
            chunk.material = white;
            SpecializeChunk(chunk);
        }

        [Button]
        public void GeneratePerlinMap()
        {
            Chunk chunk = new Chunk(Vector2.zero, chunkSize);

            /*Texture2D texture = new Texture2D(chunkSize, chunkSize);
            Color[] colourMap = new Color[chunkSize * chunkSize];

            for (int x = 0; x < chunkSize; x++) 
            { 
                for (int y = 0; y < chunkSize; y++)
                {
                    // All are assigned the same value and idk why.
                    colourMap[x * chunkSize + y] = Color.Lerp(Color.black, Color.white, Mathf.PerlinNoise(x, y));
                }
            }

            texture.SetPixels(colourMap);
            texture.Apply();

            MeshRenderer renderer = chunk.meshObject.GetComponent<MeshRenderer>();

            perlinMaterial.mainTexture = texture;
            renderer.sharedMaterial = perlinMaterial;*/
        }

        [Button]
        public void FloorCoordinate()
        {
            Debug.Log(Mathf.Floor(playerTransform.position.x / (chunkSize * viewDistanceInChunks)));
            Debug.Log(Mathf.Floor(playerTransform.position.z / (chunkSize * viewDistanceInChunks)));
        }

        #endregion

        private void SpecializeChunk(Chunk chunkData)
        {
            if (chunkData == null)
                return;

            switch (chunkData.id)
            {
                case ChunkType.Maze:
                    GenerateMaze(chunkData);
                    break;

                case ChunkType.Pitfalls:
                    GeneratePitfalls(chunkData);
                    break;

                case ChunkType.Grid:
                    GenerateGrid(chunkData);
                    break;

                case ChunkType.Distorted:
                    GenerateDistortedMaze(chunkData);
                    break;

                default:
                    Debug.LogError("Chunk " + chunkData.meshObject.name + "could not specialize! Make sure the Chunk ID is defined.");
                    return;
            }
        }

        private void GenerateMaze(Chunk chunkData)
        {
            
        }

        private void GeneratePitfalls(Chunk chunkData)
        {

        }

        private void GenerateGrid(Chunk chunkData)
        {

        }

        private void GenerateDistortedMaze(Chunk chunkData)
        {

        }

        #region MAZE METHODS

        private struct Point
        {
            public Vector3 position;
            public Vector3 left;
            public Vector3 up;
            public Vector3 right;
            public Vector3 down;


            public Point(Vector3 position)
            {
                this.position = position;

                left = -Vector3.right;
                up = Vector3.forward;
                right = Vector3.right;
                down = -Vector3.forward;
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (GameManager.Instance != null && drawGizmos)
            {

            }
        }
    }
}
