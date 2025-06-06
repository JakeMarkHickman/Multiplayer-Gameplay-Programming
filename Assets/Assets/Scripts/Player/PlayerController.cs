using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] Movement moveScript;
    [SerializeField] MainHand handScript;
    InputSystem_Actions m_PlayerActions;

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

        if (!IsOwner && gameObject.TryGetComponent<UIDocument>(out UIDocument uiDocumentComp) && gameObject.TryGetComponent<HUD>(out HUD hudComp))
        {
            uiDocumentComp.enabled = true;
            hudComp.enabled = true;
        }
        
        m_PlayerActions.Player.Move.performed += moveScript.Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled += moveScript.Handle_MoveCancelled;
        m_PlayerActions.Player.Attack.performed += handScript.Handle_OnAttack;
    }

    private void OnDisable()
    {
        m_PlayerActions.Player.Move.performed -= moveScript.Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled -= moveScript.Handle_MoveCancelled;
        m_PlayerActions.Player.Attack.performed -= handScript.Handle_OnAttack;
    }
}
