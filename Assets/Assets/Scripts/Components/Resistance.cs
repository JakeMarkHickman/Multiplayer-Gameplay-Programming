using Unity.Netcode;
using UnityEngine;

public class Resistance : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<DamageTypeEnum> m_Resistance = new NetworkVariable<DamageTypeEnum>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public DamageTypeEnum GetReistance()
    {
        return m_Resistance.Value;
    }

    [Rpc(SendTo.Server)]
    public void SetResistanceRPC(DamageTypeEnum value)
    {
        m_Resistance.Value = value;
    }
}
