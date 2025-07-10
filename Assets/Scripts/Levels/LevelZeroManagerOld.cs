using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;
using UnityEditor.ShaderGraph.Internal;

namespace Assets.Scripts.Levels
{
    public class LevelZeroManagerOld : MonoBehaviour
    {
        /*public GameManager gameManager;
        
        [Header("Chunk Properties")]
        public int chunkLength;
        public Transform player;
        
        [Header("Maze Properties")]
        public bool generateMaze = true;
        [Min(1)] public float minWallWidth;
        [Min(1)] public float maxWallWidth;
        [Min(1)] public float wallHeight;
        [Min(0.1f)] public float wallThickness;
        [Range(50, 100)] public float chunkPercentage;
        [Range(0, 1)] public float multiplier;
        public Vector2Int chainAmount;

        [Header("Test Assets")]
        public Material greenMat;

        private RenderHelper renderHelper;
        private int viewDistance = 3;
        private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();


        private void Awake()
        {
            gameManager = GetComponent<GameManager>();
            chunkPercentage *= 0.01f;
            renderHelper = new RenderHelper();
            //GenerateSpawnArea();
        }

        private void Update()
        {
            Vector2Int playerChunkCoord = new Vector2Int(Mathf.FloorToInt(player.position.x / chunkLength), Mathf.FloorToInt(player.position.z / chunkLength));
            HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

            for (int x = -viewDistance; x <= viewDistance; x++)
            {
                for (int z = -viewDistance; z <= viewDistance; z++)
                {
                    Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + z);
                    neededChunks.Add(chunkCoord);

                    if (!loadedChunks.ContainsKey(chunkCoord))
                    {
                        Vector3 chunkPosition = new Vector3(chunkCoord.x * chunkLength, 0, chunkCoord.y * chunkLength);
                        GameObject chunk = new GameObject();
                        loadedChunks.Add(chunkCoord, chunk);
                    }
                }
            }
        }

        [Button]
        public void GenerateSpawnArea()
        {
            // In the future, check if its already been generated via save file.
            int startAmount = 1;
            float offsetPosition = chunkLength * 10;
            GameObject parent = new GameObject("Spawn Chunks Parent");

            for (int i = 0; i < startAmount; i++)
            {
                for (int j = 0; j < startAmount; j++)
                {
                    Vector2 position = new Vector2(i, j) * chunkLength * 10;
                    Chunk chunk = new Chunk(position, chunkLength);
                    chunk.transform.parent = parent.transform;
                    SelectGenerationType(chunk);
                }
            }

            parent.transform.position = new Vector3(transform.position.x - offsetPosition, 0, transform.position.z - offsetPosition);
        }

        private void SelectGenerationType(Chunk chunk)
        {
            if (generateMaze)
            {
                Vector3 chunkExtents = chunk.gameObject.GetComponent<BoxCollider>().bounds.extents;
                GenerateMazeWalls(chunk, chunkExtents);
            }
        }

        private void GenerateMazeWalls(Chunk chunk, Vector3 chunkExtents)
        {
            // Per Chunk
            //int wallChainAmount = Random.Range(chainAmount.x, chainAmount.y);
            int wallChainAmount = 0;
            List<GameObject> walls = new List<GameObject>();

            for (int i = 0; i <= wallChainAmount; i++)
            {
                float posX = (float)Random.Range(-chunkExtents.x, chunkExtents.x);
                float posZ = (float)Random.Range(-chunkExtents.z, chunkExtents.z);
                Vector3 chunkOffset = chunk.transform.position;

                GameObject chainWall = NewWall();
                chainWall.transform.localScale = new Vector3(Random.Range(minWallWidth, maxWallWidth), wallHeight, wallThickness);
                chainWall.transform.position = new Vector3(posX, chainWall.transform.position.y, posZ) * chunkLength + chunkOffset;
                walls.Add(chainWall);

                int attempts = 5;
                Transform pw = chainWall.transform;
                pw.position = new Vector3(pw.position.x, pw.position.y + (wallHeight * 0.5f), pw.position.z);
                pw.GetComponent<MeshRenderer>().material = greenMat;

                do
                {
                    // nw = New Wall, pw = Previous Wall
                    GameObject nw = NewWall();
                    walls.Add(nw);
                    nw.transform.position = NextWallPosition(nw.transform, pw);
                    pw = nw.transform;

                    attempts--;
                }

                while (attempts >= 0);
            }

            //CleanUpIntersectingWalls(walls, chunk);
            
            // Change parents AFTER positions are set to prevent stupid position problems
            foreach (GameObject wall in walls)
            {
                
                wall.transform.parent = chunk.transform;
            }
        }

        private Vector3 NextWallPosition(Transform nextWall, Transform previousWall)
        {
            // 0 = forward, 1 = left, 2 = right
            int direction = Random.Range(1, 10) < 3 ? 0 : (Random.Range(1, 10) > 5 ? 1 : 2);
            Vector3 scale = direction == 0 ? new Vector3(Random.Range(minWallWidth, maxWallWidth), wallHeight, wallThickness) : new Vector3(wallThickness, wallHeight, Random.Range(minWallWidth, maxWallWidth));
            nextWall.transform.localScale = scale;

            //float offsetX = direction == 1 || direction == 2 ? (previousWall.localScale.x * 0.5f) + (nextWall.localScale.x * 0.5f) : (previousWall.localScale.x * 0.5f) + (nextWall.localScale.x * 0.5f);
            //float offsetZ = direction == 0 ? 0 : (direction == 1 ? (previousWall.localScale.z * 0.5f) + (nextWall.localScale.z * 0.5f) : -(nextWall.localScale.z * 0.5f) - (previousWall.localScale.z * 0.5f));

            float supplementaryZ = previousWall.localScale.z > previousWall.localScale.x ? 10: 0;

            float offsetX = (previousWall.localScale.x + nextWall.localScale.x) * 0.5f;
            float offsetZ = direction == 1 ? (nextWall.localScale.z - previousWall.localScale.z) * 0.5f + supplementaryZ : (direction == 2 ? -(nextWall.localScale.z - previousWall.localScale.z) * 0.5f : 0);

            // * (direction == 1 ? 1 : (direction == 2 ? -1 : 0)
            return new Vector3(previousWall.position.x + offsetX, previousWall.position.y, previousWall.position.z + offsetZ);
        }

        private void CleanUpIntersectingWalls(List<GameObject> chunkWalls, Chunk chunk)
        {
            List<GameObject> toDestroy = new List<GameObject>();
            Bounds chunkBounds = new Bounds(transform.TransformPoint(chunk.transform.position), chunk.transform.localScale * 10);

            for (int i = 0; i < chunkWalls.Count; i++)
            {
                if (toDestroy.Contains(chunkWalls[i]) || chunkWalls[i] == null) continue;

                Bounds boundsA = new Bounds(chunkWalls[i].transform.position, chunkWalls[i].transform.localScale);

                for (int j = i + 1; j < chunkWalls.Count; j++)
                {
                    if (toDestroy.Contains(chunkWalls[j]) || chunkWalls[j] == null) continue;

                    Bounds boundsB = new Bounds(chunkWalls[j].transform.position, chunkWalls[j].transform.localScale);

                    if (boundsA.Intersects(boundsB))
                    {
                        int randRange = Random.Range(0, 100);
                        toDestroy.Add(randRange < 20 ? chunkWalls[i] : (randRange >= 20 && randRange <= 60 ? chunkWalls[j] : null));
                    }
                }

                if (!boundsA.Intersects(chunkBounds))
                {
                    toDestroy.Add(chunkWalls[i]);
                }
            }

            foreach (GameObject wall in toDestroy)
            {
                chunkWalls.Remove(wall);
                DestroyImmediate(wall);
            }
        }

        private void GenerateGridWalls(Chunk chunk)
        {

        }

        private void GenerateHoles(Chunk chunk)
        {
            
        }

        private GameObject NewWall()
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshRenderer renderer = wall.GetComponent<MeshRenderer>();

            return wall;
        }*/
    }
}
