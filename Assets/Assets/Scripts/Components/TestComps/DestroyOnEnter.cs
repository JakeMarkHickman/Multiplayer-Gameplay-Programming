using Unity.Netcode;
using UnityEngine;

public class DestroyOnEnter : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        NetworkObject NetworkObj = gameObject.GetComponent<NetworkObject>();

        Health health = NetworkObj.GetComponent<Health>();

        if(!health)
            return;

        health.TakeDamageRPC(DamageTypeEnum.Bludgeoning, 10, collision.gameObject.name);
    }
}
