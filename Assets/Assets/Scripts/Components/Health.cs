using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public struct HealthChangeStruct
{
    public HealthChangeStruct(float damage, float changeInHealth, string tag)
    {
        Damage = damage;
        ChangeInHealth = changeInHealth;
        DamageTag = tag;
    }

    float Damage;
    float ChangeInHealth;
    string DamageTag;
}

public struct MaxHealthChangeStruct
{
    public MaxHealthChangeStruct(float damage, float changeInHealth, string tag)
    {
        Damage = damage;
        ChangeInHealth = changeInHealth;
        DamageTag = tag;
    }

    float Damage;
    float ChangeInHealth;
    string DamageTag;
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

    [SerializeField] private AudioClip[] m_DamageAudio;
    [SerializeField] private AudioClip[] m_DeathAudio;
    
    public event Action<HealthChangeStruct> onHealthChanged;
    public event Action<MaxHealthChangeStruct> onMaxHealthChanged;
    public event Action<DeathStruct> onDeath;

    #region Server
    [Rpc(SendTo.Server)]
    public void TakeDamageRPC(DamageTypeEnum damageType, float damage, string tag)
    {
        if (gameObject.tag == tag)
            return;
        
        Resistance resistanceComp = gameObject.GetComponent<Resistance>();

        if (resistanceComp && resistanceComp.isActiveAndEnabled) // Resistance comp only exists if there is an active resistance
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
            DeathRPC(tag);
            gameObject.GetComponent<NetworkObject>().Despawn(true);
        }

        SetHealthRPC(health, tag);
    }
    #endregion

    #region Client
    [Rpc(SendTo.ClientsAndHost)]

    private void HealthChangedClientRPC(float damage, float changeInHealth, string tag)
    {
        if(gameObject.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
        {
            ps.Play();
        }
        
        int randomSound = Random.Range(0, m_DamageAudio.Length);
        AudioManager.Instance.PlayAudio(m_DamageAudio[randomSound], false);
        onHealthChanged?.Invoke(new HealthChangeStruct(damage, changeInHealth, tag));
        Debug.Log(gameObject.name + " has been damaged by " + tag + " for " + damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void MaxHealthChangedClientRPC(float damage, float changeInHealth, string tag)
    {
        onMaxHealthChanged?.Invoke(new MaxHealthChangeStruct(damage, changeInHealth, tag));
        Debug.Log(gameObject.name + " has had max health damaged by " + tag + " for " + damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void DeathRPC(string dealer)
    {
        
        
        int randomSound = Random.Range(0, m_DeathAudio.Length);
        AudioManager.Instance.PlayAudio(m_DeathAudio[randomSound], false);
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
    public void SetMaxHealthRPC(float value, string tag)
    {
        float preMax = GetMaxHealth();

        float healthPercent = GetHealth() / preMax;

        m_MaxHealth.Value = value;
        MaxHealthChangedClientRPC(value, preMax, tag);

        SetHealthRPC(healthPercent * GetMaxHealth(), tag);
    }

    [Rpc(SendTo.Server)]
    public void SetHealthRPC(float value, string tag)
    {
        float preHealth = GetHealth();

        if(value > GetMaxHealth())
            value = GetMaxHealth();

        m_Health.Value = value;
        HealthChangedClientRPC(value, preHealth - value, tag);
    }
    #endregion
}
