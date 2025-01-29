using Unity.Netcode;
using UnityEngine;

public class TestResistance : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        Resistance collisionResistanceComp = collision.gameObject.GetComponent<Resistance>();
        Resistance ResistanceComp = gameObject.GetComponent<Resistance>();

        DamageTypeEnum resistance;

        if (ResistanceComp)
            resistance = ResistanceComp.GetReistance();
        else
            resistance = DamageTypeEnum.Bludgeoning;

        if (!collisionResistanceComp)
        {
            AddCompRPC();
            collisionResistanceComp = collision.GetComponent<Resistance>();
        }

        if (!collisionResistanceComp)
            Debug.Log("lol ur shit not working");

        Debug.Log(collisionResistanceComp.GetReistance());

        collisionResistanceComp.SetResistanceRPC(resistance);

    }

    [Rpc(SendTo.Everyone)]
    void AddCompRPC()
    {
        GameObject playerToAddCompTo = NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject.gameObject;

        playerToAddCompTo.AddComponent<Resistance>();
    }
}
