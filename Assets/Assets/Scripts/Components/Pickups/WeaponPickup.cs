using System;
using Unity.Netcode;
using UnityEngine;

public class WeaponPickup : NetworkBehaviour
{
    [SerializeField] NetworkObject item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.TryGetComponent<MainHand>(out MainHand mainComp))
        {
            mainComp.SetItem(item);
        }
    }
}
