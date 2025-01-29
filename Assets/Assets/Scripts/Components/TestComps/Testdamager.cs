using Unity.Netcode;
using UnityEngine;

public class Testdamager : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        Health health = collision.gameObject.GetComponent<Health>();
        Damage damage = gameObject.GetComponent<Damage>();

        if (!health || !damage)
            return;

        health.TakeDamageRPC(damage.GetDamageType(), damage.GetDamage(), gameObject.name);
    }
}
