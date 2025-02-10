using System;
using Unity.Netcode;
using UnityEngine;

public class ArmourPickUp : NetworkBehaviour
{
    [SerializeField] float armourStat = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.TryGetComponent<Armour>(out Armour armourComp))
        {
            armourComp.SetArmourRPC(armourStat);
            DestroyPickupRPC();
        }
    }

    [Rpc(SendTo.Server)]

    private void DestroyPickupRPC()
    {
       this.gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
