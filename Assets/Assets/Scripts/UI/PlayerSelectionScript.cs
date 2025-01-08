using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class PlayerSelectionScript : NetworkBehaviour
{
    [SerializeField] UIDocument document;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            document.enabled = true;
        }
    }
}
