using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class Movement : NetworkBehaviour
{
    [SerializeField] bool m_CanMove = true;
    [SerializeField] Rigidbody2D m_RB;

    public NetworkVariable<Vector2> m_LastDirection = new NetworkVariable<Vector2>(
            new Vector2(0, 0),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

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

        Move(context.ReadValue<Vector2>());
    }

    public void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;

        Move(context.ReadValue<Vector2>());
        
    }

    public void Move(Vector2 Direction)
    {
        m_Direction = Direction;

        if(m_Direction != new Vector2(0.0f, 0.0f))
        {
            m_moving = true;
            m_LastDirection.Value = m_Direction;
            c_move = StartCoroutine(c_MovementUpdate());
        }
        else
        {
            m_moving = false;
            StopCoroutine(c_move);
            m_RB.linearVelocity = new Vector2(0.0f, 0.0f);
        }
    }

    IEnumerator c_MovementUpdate()
    {
        Speed speedComp = gameObject.GetComponent<Speed>();
        float speed = 10f;
        float acceleration = 0.1f;

        if (speedComp)
        {
            speed = speedComp.GetSpeed();
            acceleration = speedComp.GetAcceleration();
        }

        while (m_moving)
        {
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, speed, acceleration);
            m_RB.linearVelocity = new Vector2(m_Direction.x * m_currentSpeed, m_Direction.y * m_currentSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}
