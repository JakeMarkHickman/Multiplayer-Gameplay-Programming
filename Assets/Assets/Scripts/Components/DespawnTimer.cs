using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DespawnTimer : NetworkBehaviour
{
    [SerializeField] private float m_DespawnTimer = 1f;

    private void Start()
    {
        if (!IsServer)
            return;

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(m_DespawnTimer);

        DestoryRPC();
    }

    [Rpc(SendTo.Server)]
    private void DestoryRPC()
    {
        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
