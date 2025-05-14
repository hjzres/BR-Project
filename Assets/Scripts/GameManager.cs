using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public List<Chunk> chunks = new List<Chunk>();
        public bool updateChunks = true;

        private void Awake()
        {
            chunks = new List<Chunk>();
            //StartCoroutine(UpdateChunks());
        }

        private IEnumerator UpdateChunks()
        {
            while (updateChunks)
            {


                yield return new WaitForSeconds(1f);
            }
        }
    }
}
