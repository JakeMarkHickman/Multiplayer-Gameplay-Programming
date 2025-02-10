using Mono.Cecil;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainHand : NetworkBehaviour
{
    [SerializeField] private NetworkObject m_Item;

    public void Handle_OnAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;
        
        UseRPC();
    }

    [Rpc(SendTo.Server)]
    private void UseRPC()
    {
        var inst = NetworkManager.SpawnManager.InstantiateAndSpawn(m_Item);
        Item item = inst.GetComponent<Item>();

        if (!item)
            return;

        item.Use(gameObject.GetComponent<NetworkObject>());
    }

    public void SetItem(NetworkObject value)
    {
        m_Item = value;
    }
}
