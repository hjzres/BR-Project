using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform[] tPlayer;

        public Vector3[] playerPosition;

        private void Awake()
        {
            tPlayer = Utilites.RetrieveTransformsByTag("Player");
            playerPosition = new Vector3[tPlayer.Length];
        }

        private void Update()
        {
            if (tPlayer.Length <= 0 || tPlayer == null)
            {
                Debug.Log("Player transform array is empty or null!");
                return;
            }   

            for (int i = 0; i < tPlayer.Length; i++)
            {
                playerPosition[i] = tPlayer[i].position;
            }
        }

        private void RetrieveSaveData()
        {

        }

        private void ApplySaveData()
        {

        }
    }
}
