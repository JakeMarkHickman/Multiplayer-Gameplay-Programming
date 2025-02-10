using Unity.Netcode;
using UnityEngine;

public class ResistancePickup : NetworkBehaviour
{
    [SerializeField] DamageTypeEnum m_Resistance;
    [SerializeField] bool RemoveResistances;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.TryGetComponent<Resistance>(out Resistance resistanceComp))
        {
            if(!RemoveResistances)
            {
                resistanceComp.enabled = true;
                resistanceComp.SetResistanceRPC(m_Resistance);
                DestroyPickupRPC();
            }
            else
            {
                resistanceComp.enabled = false;
                DestroyPickupRPC();
            }
        }
    }

    [Rpc(SendTo.Server)]

    private void DestroyPickupRPC()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
