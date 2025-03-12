using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts
{
    public class LevelZeroManager : GameManager
    {
        public int chunkSize = 10;

        public int viewDistance;

        private List<PlaneChunk> planeChunks = new List<PlaneChunk>();

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        [Button]
        public void Generate()
        {
            // Works only for this distance needs more work
            int distanceBetweenChunks = chunkSize * chunkSize;

            for (int x = -distanceBetweenChunks; x <= distanceBetweenChunks; x += distanceBetweenChunks)
            {
                for (int y = -distanceBetweenChunks; y <= distanceBetweenChunks; y += distanceBetweenChunks)
                {
                    planeChunks.Add(new PlaneChunk(new Vector2(x, y), chunkSize));
                }
            }
        }

        private struct LevelZeroGeneration
        {

        }
    }
}
