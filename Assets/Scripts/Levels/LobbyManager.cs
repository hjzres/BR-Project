using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts.Levels
{
    public class LobbyManager : MonoBehaviour
    {
        public Transform playerTransform;

        [Header("CHUNKING PROPERTIES")]
        public int viewDistanceInChunks = 3;
        public int chunkSize = 150;
        [Min(1f)] public float rayDistance = 1;
        private Vector2 playerChunkCoord;
        private RaycastHit chunkHit = new RaycastHit();
        private Transform chunkParent;

        // Scripts & Components
        private RenderHelper renderHelper;

        private void Awake()
        {
            renderHelper = new RenderHelper();
            renderHelper.loadedChunks.Clear(); // TODO: Hold data in save file instead of clearing.
            chunkParent = GameObject.Find("Chunks").transform;

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            GenerateSpawnArea();
        }

        private void LateUpdate()
        {
            UpdateChunks();
        }

        private void GenerateSpawnArea()
        {
            playerChunkCoord = Vector2.zero;

            for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
            {
                for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
                {
                    Vector2 coord = new Vector2(playerChunkCoord.x + (chunkSize * x), playerChunkCoord.y + (chunkSize * y));
                    Chunk newChunk = new Chunk(coord, chunkSize, GameManager.Instance.white);

                    renderHelper.loadedChunks.Add(coord, newChunk);
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
                        
                        if (!renderHelper.loadedChunks.ContainsKey(coord))
                        {
                            Chunk newChunk = new Chunk(coord, chunkSize, GameManager.Instance.white);

                            renderHelper.loadedChunks.Add(coord, newChunk);
                            newChunk.meshObject.transform.parent = chunkParent;
                        }

                        else
                        {
                            renderHelper.loadedChunks[coord].UpdateChunkVisibility(); // Has to be implemented
                        }
                    }
                }
            }

            else
            {
                return;
            }
        }
    }
}
