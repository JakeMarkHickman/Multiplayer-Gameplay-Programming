using System;
using Unity.Netcode;
using UnityEngine;

public struct UseItemStruct
{
    public UseItemStruct(NetworkObject user)
    {
        User = user;
    }

    public NetworkObject User;
}

public struct PickUpItemStruct
{

}

public class Item : NetworkBehaviour
{
    public event Action<UseItemStruct> onUseItem;
    public event Action<PickUpItemStruct> onPickUpItem;

    public virtual void Use(NetworkObject user)
    {
        onUseItem?.Invoke(new UseItemStruct(user));
        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }

    public void PickUp()
    {
        Debug.Log("Pickup Item");
        onPickUpItem?.Invoke(new PickUpItemStruct());
    }
}
