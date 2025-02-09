using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfBounds : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;
        
        collision.GetComponent<NetworkObject>().Despawn(true);
    }
}
