using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class PlayerInputHandler : MonoBehaviour
{
    Rigidbody2D m_RB;
    InputSystem_Actions m_PlayerActions;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

        m_PlayerActions.Player.Jump.performed += Handle_JumpPressed;
    }

    private void OnDisable()
    {
        m_PlayerActions.Player.Jump.performed -= Handle_JumpPressed;
    }

    void Handle_JumpPressed(InputAction.CallbackContext ctx)
    {
        m_RB.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}