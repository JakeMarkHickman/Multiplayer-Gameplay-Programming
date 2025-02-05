using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Movement moveScript;
    [SerializeField] MainHand handScript;
    InputSystem_Actions m_PlayerActions;

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

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
