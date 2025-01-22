using System;
using Unity.Netcode;
using UnityEngine;

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

    private struct HealthChangeEvent
    {
        float damage;
        float changeInHealth;
        GameObject DamageDealer;
    }

    event Action<HealthChangeEvent> healthChanged;

    public override void OnNetworkSpawn()
    {

    }

    public void TakeDamage(float damage, GameObject dealer)
    {

    }
}
