using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : NetworkBehaviour
{
    [SerializeField] bool m_CanMove = true;
    [SerializeField] float m_MoveSpeed = 10.0f;
    [SerializeField] float m_Acceleration = 0.1f;
    [SerializeField] Rigidbody2D m_RB;

    private Coroutine c_move;
    private Vector2 m_Direction;
    private float m_currentSpeed;
    private bool m_moving;

    public void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        if(!IsOwner)
            return;

        if (!m_CanMove)
            return;

        m_Direction = context.ReadValue<Vector2>();
        m_moving = true;

        c_move = StartCoroutine(c_MovementUpdate());
    }

    public void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;

        m_Direction = context.ReadValue<Vector2>();
        m_moving = false;

        StopCoroutine(c_move);
        m_RB.linearVelocity = new Vector2(0.0f, 0.0f);
    }

    IEnumerator c_MovementUpdate()
    {
        while (m_moving)
        {
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_MoveSpeed, m_Acceleration);
            m_RB.linearVelocity = new Vector2(m_Direction.x * m_currentSpeed, m_Direction.y * m_currentSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}
