using Unity.Netcode;
using UnityEngine;

public class Damage : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> m_MinDamage = new NetworkVariable<float>(
            1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );
    [SerializeField]
    private NetworkVariable<float> m_MaxDamage = new NetworkVariable<float>(
            2,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );
    [SerializeField]
    private NetworkVariable<float> m_DamageMultiplier = new NetworkVariable<float>(
            1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public float GetDamage()
    {
        float damageDelt;

        damageDelt = Random.Range(m_MinDamage.Value, m_MaxDamage.Value);
        damageDelt *= m_DamageMultiplier.Value;

        return damageDelt;
    }

    [Rpc(SendTo.Server)]
    public void SetMinDamageRPC(float value)
    {
        if (!IsServer)
            return;

        m_MinDamage.Value = value;
    }

    [Rpc(SendTo.Server)]
    public void SetMaxDamageRPC(float value)
    {
        if (!IsServer)
            return;

        m_MaxDamage.Value = value;
    }

    [Rpc(SendTo.Server)]
    public void SetDamageMultiplierRPC(float value)
    {
        if (!IsServer)
            return;

        m_DamageMultiplier.Value = value;
    }
}
