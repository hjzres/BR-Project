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

        [Header("Chunking")]
        public int size = 150;

        [Header("Test Materials")]
        public Material green;
        public Material red;
        public Material white;

        private void Awake()
        {
            renderHelper = new RenderHelper();
        }

        private void Update()
        {
            
        }

        [Button]
        public void GenerateMesh()
        {
            renderHelper = new RenderHelper();
            RenderHelper.Chunk chunk = new RenderHelper.Chunk(new Vector2(0, 0), white, size);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {

            }
        }
    }
}
