using System;
using Unity.Netcode;
using UnityEngine;

public struct UseItemStruct
{
    public UseItemStruct(NetworkObject user)
    {
        User = user;
    }

    NetworkObject User;
}

public struct PickUpItemStruct
{

}

public class Item : NetworkBehaviour
{
    public event Action<UseItemStruct> onUseItem;
    public event Action<PickUpItemStruct> onPickUpItem;

    public void Use()
    {
        Debug.Log("Using Item");
        onUseItem?.Invoke(new UseItemStruct());
    }

    public void PickUp()
    {
        Debug.Log("Pickup Item");
        onPickUpItem?.Invoke(new PickUpItemStruct());
    }
}
