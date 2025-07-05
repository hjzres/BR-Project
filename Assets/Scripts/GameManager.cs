using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.RenderHelper;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform player;
        public bool updateChunks = true;

        private void Awake()
        {
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
