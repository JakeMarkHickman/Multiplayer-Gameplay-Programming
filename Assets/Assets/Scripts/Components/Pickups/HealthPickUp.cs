using Unity.Netcode;
using UnityEngine;

public class HealthPickUp : NetworkBehaviour
{
    [SerializeField] float HealthToGive = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        if(collision.gameObject.TryGetComponent<Health>(out Health healthComp))
        {
            healthComp.SetHealthRPC(healthComp.GetHealth() + HealthToGive, gameObject.tag);
            DestroyPickupRPC();
        }
    }

    [Rpc(SendTo.Server)]

    private void DestroyPickupRPC()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
