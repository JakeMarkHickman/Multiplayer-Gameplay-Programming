using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Speed : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> m_Speed = new NetworkVariable<float>(
            10,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    [SerializeField]
    private NetworkVariable<float> m_Acceleration = new NetworkVariable<float>(
            0.1f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public float GetSpeed()
    {
        return m_Speed.Value;
    }

    public float GetAcceleration()
    {
        return m_Acceleration.Value;
    }

    [Rpc(SendTo.Server)]
    public void SetSpeedRPC(float value)
    {
        m_Speed.Value = value; 
    }

    [Rpc(SendTo.Server)]
    public void SetAccelerationRPC(float value)
    {
        m_Acceleration.Value = value;
    }
}
