using Unity.Netcode;
using UnityEngine;

public enum DamageTypeEnum
{
    Bludgeoning, //Normal attcks no special effects
    Magic, // Can peierce armour
    Slash // Causes bleed damage
}

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

    [SerializeField]
    private NetworkVariable<DamageTypeEnum> m_DamageType = new NetworkVariable<DamageTypeEnum>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
       );

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;
        
        NetworkObject obj = collision.gameObject.GetComponent<NetworkObject>();

        if (!obj)
            return;

        Health healthComp = obj.GetComponent<Health>();

        if (!healthComp)
            return;

        healthComp.TakeDamageRPC(GetDamageType(), GetDamage(), gameObject.tag);
    }

    public float GetMinDamage()
    {
        return m_MinDamage.Value;
    }
    
    public float GetMaxDamage()
    {
        return m_MaxDamage.Value;
    }
    
    public float GetDamage()
    {
        float damageDelt;

        damageDelt = Random.Range(m_MinDamage.Value, m_MaxDamage.Value);
        damageDelt *= m_DamageMultiplier.Value;

        return damageDelt;
    }

    public DamageTypeEnum GetDamageType()
    {
        return m_DamageType.Value;
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
