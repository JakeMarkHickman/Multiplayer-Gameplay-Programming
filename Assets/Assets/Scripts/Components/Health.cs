using System;
using Unity.Netcode;
using UnityEngine;

public struct HealthChangeStruct
{
    public HealthChangeStruct(float damage, float changeInHealth, string dealer)
    {
        Damage = damage;
        ChangeInHealth = changeInHealth;
        DamageDealer = dealer;
    }

    float Damage;
    float ChangeInHealth;
    string DamageDealer;
}

public struct MaxHealthChangeStruct
{
    public MaxHealthChangeStruct(float damage, float changeInHealth, string dealer)
    {
        Damage = damage;
        ChangeInHealth = changeInHealth;
        DamageDealer = dealer;
    }

    float Damage;
    float ChangeInHealth;
    string DamageDealer;
}

public struct DeathStruct
{
    public DeathStruct(string dealer)
    {
        DamageDealer = dealer;
    }

    string DamageDealer;
}

public class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> m_MaxHealth = new NetworkVariable<float>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    [SerializeField] private NetworkVariable<float> m_Health = new NetworkVariable<float>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public event Action<HealthChangeStruct> onHealthChanged;
    public event Action<MaxHealthChangeStruct> onMaxHealthChanged;
    public event Action<DeathStruct> onDeath;

    #region Server
    [Rpc(SendTo.Server)]
    public void TakeDamageRPC(DamageTypeEnum damageType, float damage, string dealer)
    {
        Resistance resistanceComp = gameObject.GetComponent<Resistance>();

        if (resistanceComp) // Resistance comp only exists if there is an active resistance
        {
            if (resistanceComp.GetReistance() == damageType)
                return;
        }

        Armour armourComp = gameObject.GetComponent<Armour>();

        float armourStat = 0;

        if (armourComp && damageType != DamageTypeEnum.Magic) // Magic is able to go through armour
            armourStat = armourComp.GetArmour();
            
        float changeInHealth = damage/armourStat;

        float health = GetHealth() - changeInHealth;

        if(health<=0)
        {
            DeathRPC(dealer);
            gameObject.GetComponent<NetworkObject>().Despawn(true);
        }

        SetHealthRPC(health, dealer);
    }
    #endregion

    #region Client
    [Rpc(SendTo.ClientsAndHost)]

    private void HealthChangedClientRPC(float damage, float changeInHealth, string dealer)
    {
        onHealthChanged?.Invoke(new HealthChangeStruct(damage, changeInHealth, dealer));
        Debug.Log(gameObject.name + " has been damaged by " + dealer + " for " + damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void MaxHealthChangedClientRPC(float damage, float changeInHealth, string dealer)
    {
        onMaxHealthChanged?.Invoke(new MaxHealthChangeStruct(damage, changeInHealth, dealer));
        Debug.Log(gameObject.name + " has had max health damaged by " + dealer + " for " + damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void DeathRPC(string dealer)
    {
        onDeath?.Invoke(new DeathStruct(dealer));
    }
    #endregion

    #region Getters

    public float GetMaxHealth()
    {
        return m_MaxHealth.Value;
    }

    public float GetHealth()
    {
        return m_Health.Value;
    }
    #endregion

    #region Setters
    [Rpc(SendTo.Server)]
    public void SetMaxHealthRPC(float value, string dealer)
    {
        float preMax = GetMaxHealth();

        float healthPercent = GetHealth() / preMax;

        m_MaxHealth.Value = value;
        MaxHealthChangedClientRPC(value, preMax, dealer);

        SetHealthRPC(healthPercent * GetMaxHealth(), dealer);
    }

    [Rpc(SendTo.Server)]
    public void SetHealthRPC(float value, string dealer)
    {
        float preHealth = GetHealth();

        m_Health.Value = value;
        HealthChangedClientRPC(value, preHealth - value, dealer);
    }
    #endregion
}
