using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Assets.Scripts.Player;

// Handles entity count, multiplayer changes + syncing
namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public int seed;

        [Header("TEST MATERIALS")]
        public Material green;
        public Material red;
        public Material white;
        public Material blue;

        [Header("GIZMOS")]
        public bool drawGizmos = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {

            }
        }
    }
}
