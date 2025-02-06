using Unity.Netcode;
using UnityEngine;

public class AiController : NetworkBehaviour
{
    [SerializeField] Movement moveScript;
    [SerializeField] MainHand hand;

    private void OnEnable()
    {
        
    }
}
