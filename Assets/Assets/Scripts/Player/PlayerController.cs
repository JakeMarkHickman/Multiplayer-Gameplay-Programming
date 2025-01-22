using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Movement moveScript;
    InputSystem_Actions m_PlayerActions;

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

        m_PlayerActions.Player.Move.performed += moveScript.Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled += moveScript.Handle_MoveCancelled;
    }

    private void OnDisable()
    {
        m_PlayerActions.Player.Move.performed -= moveScript.Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled -= moveScript.Handle_MoveCancelled;
    }
}
