using Unity.Netcode;
using UnityEngine;

public class PickupSpawn : NetworkBehaviour
{
    [SerializeField] bool SpawnOnSpawn = true;
    [SerializeField] NetworkObject[] spawnObjects;

    private void Start()
    {
        if (!IsServer)
            return;

        if (!SpawnOnSpawn)
            return;

        SpawnPickupRPC();
    }

    [Rpc(SendTo.Server)]
    public void SpawnPickupRPC()
    {

        int spawnObj = Random.Range(0, spawnObjects.Length);

        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn
        (
            spawnObjects[spawnObj],
            0,
            true,
            false,
            false,
            gameObject.transform.position,
            Quaternion.identity
        );

        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
