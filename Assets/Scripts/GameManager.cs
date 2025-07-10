using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Assets.Scripts
{
    // Handles entity count, multiplayer changes + syncing
    public class GameManager : MonoBehaviour
    {
        public Transform player;
        public bool updateChunks = true;
        public RenderHelper renderHelper;
        public bool drawGizmos = false;

        [Header("Test Materials")]
        public Material green;
        public Material red;
        public Material white;

        [Button]
        public void GenerateMesh()
        {
            renderHelper = new RenderHelper();
            GameObject mesh = RenderHelper.Chunk.CreateChunk(new Vector2(0, 0), white, 100f);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {

            }
        }
    }
}
