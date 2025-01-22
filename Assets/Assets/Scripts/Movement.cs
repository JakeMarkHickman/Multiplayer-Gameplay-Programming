using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] bool CanMove = true;

    void Handle_Movement(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        Vector2 Direction = context.ReadValue<Vector2>();

    }
}
