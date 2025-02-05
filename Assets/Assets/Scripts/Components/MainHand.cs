using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainHand : NetworkBehaviour
{
    [SerializeField] public NetworkObject m_Item;

    public void Handle_OnAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;

        Use();
    }


    public void Use()
    {
        Item item = m_Item.GetComponent<Item>();

        if (!item)
            return;

        item.Use();
    }
}
