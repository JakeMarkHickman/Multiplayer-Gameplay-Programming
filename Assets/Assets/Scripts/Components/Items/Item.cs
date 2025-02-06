using System;
using Unity.Netcode;
using UnityEngine;

public struct UseItemStruct
{
    public UseItemStruct(NetworkObject user, Vector3 mousePos)
    {
        User = user;
        MouseLocation = mousePos;
    }

    public NetworkObject User;
    public Vector3 MouseLocation;
}

public struct PickUpItemStruct
{

}

public class Item : NetworkBehaviour
{
    public event Action<UseItemStruct> onUseItem;
    public event Action<PickUpItemStruct> onPickUpItem;

    public void Use(NetworkObject user)
    {
        Debug.Log("Using Item");
        onUseItem?.Invoke(new UseItemStruct(user, Camera.main.WorldToScreenPoint(new Vector3())));
    }

    public void PickUp()
    {
        Debug.Log("Pickup Item");
        onPickUpItem?.Invoke(new PickUpItemStruct());
    }
}
