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

        private void Awake()
        {
            gameManager = GetComponent<GameManager>();
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
            int wallChainAmount = Random.Range(13, 18);

            for (int i = 0; i <= wallChainAmount; i++)
            {
                float posX = (float)Random.Range(-chunkExtents.x, chunkExtents.x); 
                float posZ = (float)Random.Range(-chunkExtents.z, chunkExtents.z);
                Vector3 chunkOffset = chunk.transform.position;
                List<GameObject> walls = new List<GameObject>();

                GameObject chainWall = NewWall();
                chainWall.transform.localScale = new Vector3(Random.Range(minWallWidth, maxWallWidth), 5f, wallThickness);
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

                // Change parents AFTER positions are set to prevent stupid problems
                foreach (GameObject wall in walls)
                {
                    wall.transform.parent = chunk.transform;
                }
            }
        }

        private Vector3 NextWallPosition(Transform nextWall, Transform previousWall)
        {
            // 0 = forward, 1 = left, 2 = right
            int direction = Random.Range(1, 10) < 3 ? 0 : (Random.Range(1, 10) > 5 ? 1 : 2);
            Vector3 scale = direction == 0 ? new Vector3(Random.Range(minWallWidth, maxWallWidth), 5f, wallThickness) : new Vector3(wallThickness, 5f, Random.Range(minWallWidth, maxWallWidth));
            nextWall.transform.localScale = scale;

            float offsetX = direction == 1 || direction == 2 ? (previousWall.localScale.x / 2f) - (nextWall.localScale.x / 2f) : (previousWall.localScale.x / 2f) + (nextWall.localScale.x / 2f);
            float offsetZ = direction == 0 ? 0 : (direction == 1 ? (previousWall.localScale.z / 2f) + (nextWall.localScale.z / 2f) : -(nextWall.localScale.z / 2f) - (previousWall.localScale.z / 2f));

            return new Vector3(previousWall.position.x + offsetX, previousWall.position.y, previousWall.position.z + offsetZ);
        }

        private void CleanUpIntersectingWalls(List<GameObject> a)
        {

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

            return wall;
        }

        // Will contain preset rooms
        /*public void GenerateMaze(Chunk chunk)
        {
            // Generate consecutive wall chains (From ends)
            // PER CHUNK 
            int chainAmounts = Random.Range(1, 5);
            Vector3 chunkExtents = chunk.gameObject.GetComponent<BoxCollider>().bounds.extents;

            for (int i = 0; i <= chainAmounts; i++)
            {                
                int chainLength = Random.Range(5, 10);

                float randomX = Random.Range(-chunkExtents.x, chunkExtents.x);
                float randomZ = Random.Range(-chunkExtents.z, chunkExtents.z);

                Vector3 chunkOffset = chunk.transform.parent.position;
                Vector3 startPosition = new Vector3(randomX, 0, randomZ) * chunkLength + chunkOffset;
                GameObject startWall = CreateWall();

                startWall.transform.parent = chunk.transform;
                startWall.transform.position = startPosition + new Vector3(0, startWall.transform.position.y + (startWall.transform.localScale.y / 2f), 0);

                Vector3 startExtents = startWall.GetComponent<BoxCollider>().bounds.extents;
                Vector3 nextWallPosition = startWall.transform.position + new Vector3(startWall.transform.localScale.x * 2, 0, 0) + chunkOffset;

                do
                {
                    GameObject nextWall = CreateWall();
                    Vector3 nextWallExtents = nextWall.GetComponent<BoxCollider>().bounds.extents;

                    nextWall.transform.parent = chunk.transform;
                    nextWall.transform.position = nextWallPosition + new Vector3(nextWall.transform.localScale.x, 0, 0);
                    nextWallPosition = nextWall.transform.position + new Vector3(nextWall.transform.localScale.x, 0, 0);

                    //GameObject nextWall = CreateWall();
                    //nextWall.transform.parent = chunk.transform;

                    //Vector3 nextWallExtents = nextWall.GetComponent<BoxCollider>().bounds.extents;

                    //nextWall.transform.position = nextWallPosition + new Vector3(nextWallExtents.x, 0, 0);
                    //nextWallPosition = nextWall.transform.position;

                    chainLength -= 1;
                }

                while (chainLength > 0);
            }
        }

        private GameObject CreateWall()
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Transform wallTransform = wall.transform;

            float length = 5;
            float rotationY = Random.Range(0, 10) <= 5 ? 90 : 0;
            wallTransform.localScale = new Vector3(length, 5.0f, wallTransform.localScale.z);
            wallTransform.rotation = new Quaternion(0, rotationY, 0, 0);
            //wall.GetComponent<MeshRenderer>().material = null;

            return wall;
        }*/
    }
}
