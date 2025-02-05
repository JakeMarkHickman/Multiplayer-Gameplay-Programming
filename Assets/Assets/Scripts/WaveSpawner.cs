using System.Collections;
using Unity.Hierarchy;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
struct WaveData
{
    [SerializeField] public NetworkObject[] ObjectsToSpawn;
    [SerializeField] public int ObjectAmount;
    [SerializeField] public float SpawnCooldown;
}


public class WaveSpawner : NetworkBehaviour
{
    [SerializeField] float MaxX;
    [SerializeField] float MaxY;
    [SerializeField] float MinX;
    [SerializeField] float MinY;

    [SerializeField] WaveData[] Waves;
    [SerializeField] float WaveCooldown;

    private bool m_OnCooldown = false;

    private int m_ObjectsLeft = 0;
    private int m_CurrentWave = 0;
    private int m_MaxWave = 0;

    private Coroutine c_ObjectSpawn;
    private Coroutine c_WaveCooldown;

    private void Start()
    {
        m_MaxWave = Waves.Length;
        StartWave();
    }

    public void StartWave()
    {
        if (!IsServer)
            return;

        if (m_OnCooldown)
            return;

        c_ObjectSpawn = StartCoroutine(c_ObjectCooldown());
    }

    private IEnumerator c_ObjectCooldown()
    {
        for (int i = 0; i < Waves[m_CurrentWave].ObjectAmount; i++)
        {
            int random = Random.Range(0, Waves[m_CurrentWave].ObjectsToSpawn.Length);

            float SpawnX = Random.Range(MinX, MaxX);
            float SpawnY = Random.Range(MinY, MaxY);

            NetworkObject netObj = NetworkManager.SpawnManager.InstantiateAndSpawn
            (
                Waves[m_CurrentWave].ObjectsToSpawn[random],
                0, 
                false, 
                false, 
                false, 
                new Vector3
                (
                    SpawnX,
                    SpawnY, 
                    0.0f
                ),
                default
            );

            netObj.OnDeferredDespawnComplete += ObjectDestroyed;

            yield return new WaitForSecondsRealtime(Waves[m_CurrentWave].SpawnCooldown);
        }

        yield return null;
    }

    private IEnumerator c_WaveFinished()
    {
        m_CurrentWave++;

        yield return new WaitForSecondsRealtime(WaveCooldown);

        StartWave();
    }

    private void ObjectDestroyed(float tick)
    {
       
    }
}
