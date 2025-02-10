using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfBounds : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;
        
        if(collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamageRPC(DamageTypeEnum.Magic, health.GetMaxHealth(), "Out of Bounds!");
        }

        collision.GetComponent<NetworkObject>().Despawn(true);
    }
}
