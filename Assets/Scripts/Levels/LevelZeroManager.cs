using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts.Levels
{
    public class LevelZeroManager : MonoBehaviour
    {
        public GameManager gameManager;
        public int chunkLength;

        [Header("Maze Properties")]
        [Min(1)] public float minWallWidth;
        [Min(1)] public float maxWallWidth;
        [Min(1)] public float wallHeight;
        [Min(0.1f)] public float wallThickness;
        [Range(50, 100)] public float chunkPercentage;
        [Range(0, 1)] public float multiplier;
        public Vector2Int chainAmount;

        [Header("Test Assets")]
        public Material greenMat;

        private void Awake()
        {
            gameManager = GetComponent<GameManager>();
            chunkPercentage *= 0.01f;
            GenerateSpawnArea();
        }

        [Button]
        public void GenerateSpawnArea()
        {
            // In the future, check if its already been generated via save file.
            int startAmount = 1;
            float offsetPosition = chunkLength * 10;
            GameObject parent = new GameObject("Spawn Chunks Parent");

            gameManager.chunks.Clear();

            for (int i = 0; i < startAmount; i++)
            {
                for (int j = 0; j < startAmount; j++)
                {
                    Vector2 position = new Vector2(i, j) * chunkLength * 10;
                    Chunk chunk = new Chunk(position, chunkLength);
                    chunk.transform.parent = parent.transform;
                    SelectGenerationType(chunk);
                    gameManager.chunks.Add(chunk);
                }
            }

            parent.transform.position = new Vector3(transform.position.x - offsetPosition, 0, transform.position.z - offsetPosition);
        }

        private void SelectGenerationType(Chunk chunk)
        {
            Vector3 chunkExtents = chunk.gameObject.GetComponent<BoxCollider>().bounds.extents;
            GenerateMazeWalls(chunk, chunkExtents);
        }

        private void GenerateMazeWalls(Chunk chunk, Vector3 chunkExtents)
        {
            // Per Chunk
            int wallChainAmount = Random.Range(chainAmount.x, chainAmount.y);
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

            CleanUpIntersectingWalls(walls, chunk);
            
            // Change parents AFTER positions are set to prevent stupid position problems
            foreach (GameObject wall in walls)
            {
                wall.transform.position = new Vector3(wall.transform.position.x, wall.transform.position.y + (wallHeight * 0.5f), wall.transform.position.z);
                wall.transform.parent = chunk.transform;
            }
        }

        private Vector3 NextWallPosition(Transform nextWall, Transform previousWall)
        {
            // 0 = forward, 1 = left, 2 = right
            int direction = Random.Range(1, 10) < 3 ? 0 : (Random.Range(1, 10) > 5 ? 1 : 2);
            Vector3 scale = direction == 0 ? new Vector3(Random.Range(minWallWidth, maxWallWidth), wallHeight, wallThickness) : new Vector3(wallThickness, wallHeight, Random.Range(minWallWidth, maxWallWidth));
            nextWall.transform.localScale = scale;

            float offsetX = direction == 1 || direction == 2 ? (previousWall.localScale.x * 0.5f) - (nextWall.localScale.x * 0.5f) : (previousWall.localScale.x * 0.5f) + (nextWall.localScale.x * 0.5f);
            float offsetZ = direction == 0 ? 0 : (direction == 1 ? (previousWall.localScale.z * 0.5f) + (nextWall.localScale.z * 0.5f) : -(nextWall.localScale.z * 0.5f) - (previousWall.localScale.z * 0.5f));

            return new Vector3(previousWall.position.x + offsetX, previousWall.position.y, previousWall.position.z + offsetZ);
        }

        private void CleanUpIntersectingWalls(List<GameObject> chunkWalls, Chunk chunk)
        {
            List<GameObject> toDestroy = new List<GameObject>();
            Bounds chunkBounds = new Bounds(transform.TransformPoint(chunk.transform.position), chunk.transform.localScale * 10);
            Debug.Log(chunkBounds);

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
        }
    }
}
