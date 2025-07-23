using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ChunkChecker : MonoBehaviour
    {
        [Header("TEST USE ONLY")]
        [Header("Raycast Properties")]
        public float rayDistance;
        public RaycastHit hit;

        private void FixedUpdate()
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z), Vector3.down, out hit, rayDistance))
            {
                Debug.Log("Hit: " + hit.collider.transform.position);
            }

            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z), Vector3.down, Color.red);
        }
    }
}
