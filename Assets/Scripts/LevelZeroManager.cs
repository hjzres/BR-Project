using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts
{
    public class LevelZeroManager : GameManager
    {
        public List<PlaneChunk> planeChunks = new List<PlaneChunk>();

        private Vector2 currentPlayerPosition;

        public int chunkDistanceRadius = 500;

        private void Awake()
        {
            currentPlayerPosition = new Vector2();
        }

        private void Update()
        {
            currentPlayerPosition = tPlayer[0].position;

            for (int x = -chunkDistanceRadius; x <= chunkDistanceRadius; x++)
            {
                for (int z = -chunkDistanceRadius; z <= chunkDistanceRadius; z++)
                {
                    
                }
            }
        }
    }
}
