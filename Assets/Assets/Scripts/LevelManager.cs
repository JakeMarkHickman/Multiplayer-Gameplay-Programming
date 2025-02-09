using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject m_PlayerObject;
    [SerializeField] public NetworkManager netManager;
    
    public void OnSceneLoaded(ulong clientID, string sceneName, LoadSceneMode sceneMode)
    {
        if (!IsServer)
            return;
        
        if(sceneName != "Game")
            return;
        
        SpawnPlayerRPC(clientID);
    }

    [Rpc(SendTo.Server)]
    private void SpawnPlayerRPC(ulong clientID)
    {
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn
        (
            m_PlayerObject,
            clientID,
            true,
            true,
            false,
            new Vector3(0, 0, 0),
            quaternion.identity
        );
    }
    
    
}
