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

    public event Action<HealthChangeStruct> healthChanged;

    #region Server
    [Rpc(SendTo.Server)]
    public void TakeDamageRPC(float damage, string dealer)
    {
        Armour armour = gameObject.GetComponent<Armour>();

        float armourStat = 0;

        if (armour)
            armourStat = armour.GetArmour();

        float changeInHealth = damage/armourStat;

        m_Health.Value -= changeInHealth;

        TakeDamageClientRPC(damage, changeInHealth, dealer);
    }

    #endregion

    #region Client
    [Rpc(SendTo.ClientsAndHost)]
    private void TakeDamageClientRPC(float damage, float changeInHealth, string dealer)
    {
        healthChanged?.Invoke(new HealthChangeStruct(damage, changeInHealth, dealer));
        Debug.Log(gameObject.name + " has been damaged by " + dealer + " for " + damage);
    }

    #endregion
}
