using Unity.Netcode;
using UnityEngine;

public class Armour : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> m_Armour = new NetworkVariable<float>(
            1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public float GetArmour()
    {
        return m_Armour.Value;
    }

    [Rpc(SendTo.Server)]
    public void SetArmourRPC(float value)
    {
        m_Armour.Value = value;
    }
}
