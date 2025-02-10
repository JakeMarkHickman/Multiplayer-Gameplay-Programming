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
        }
    }
}
