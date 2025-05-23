using UnityEngine;
using Unity.Netcode;

namespace Assets.Scripts.Network
{
    public class CameraFollow : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                gameObject.SetActive(false);
            }
        }
    }
}