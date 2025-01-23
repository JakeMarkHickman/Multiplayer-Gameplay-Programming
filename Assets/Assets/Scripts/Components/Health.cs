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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == gameObject.tag) // Dont Fire Damage Event
            return;

        GameObject Damager = collision.gameObject;
        Damage damageComp = Damager.GetComponent<Damage>();
        float dmg = damageComp.GetDamage();

        Armour defenceComp = gameObject.GetComponent<Armour>();



        if (IsServer)
        {
            
        }

        healthChanged.Invoke(new HealthChangeEvent {  });
    }

    public void TakeDamage(float damage, GameObject dealer)
    {

    }
}
