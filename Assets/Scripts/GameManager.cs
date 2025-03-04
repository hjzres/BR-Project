using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform[] tPlayer;

        private void Awake()
        {
            tPlayer = Utilites.RetrieveTransformsByTag("Player");
        }
    }
}
