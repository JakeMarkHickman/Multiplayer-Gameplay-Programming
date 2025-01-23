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

    public void SetMinDamage(float value)
    {
        if (!IsServer)
            return;

        m_MinDamage.Value = value;
    }

    public void SetMaxDamage(float value)
    {
        if (!IsServer)
            return;

        m_MaxDamage.Value = value;
    }

    public void SetDamageMultiplier(float value)
    {
        if (!IsServer)
            return;

        m_DamageMultiplier.Value = value;
    }
}
