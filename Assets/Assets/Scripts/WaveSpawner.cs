using System.Collections;
using Unity.Hierarchy;
using Unity.Netcode;
using Unity.VisualScripting;
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
        int waveToSpawn = m_CurrentWave % m_MaxWave;

        m_ObjectsLeft = Waves[waveToSpawn].ObjectAmount;

        for (int i = 0; i < Waves[waveToSpawn].ObjectAmount; i++)
        {
            int random = Random.Range(0, Waves[waveToSpawn].ObjectsToSpawn.Length);

            float SpawnX = Random.Range(MinX, MaxX);
            float SpawnY = Random.Range(MinY, MaxY);

            NetworkObject netObj = NetworkManager.SpawnManager.InstantiateAndSpawn
            (
                Waves[waveToSpawn].ObjectsToSpawn[random],
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

            Health healthcomp = netObj.GetComponent<Health>();

            if(healthcomp)
            {
                healthcomp.onDeath += ObjectDestroyed;
            }
            else
            {
                ObjectDestroyed(new DeathStruct("none"));
            }

            yield return new WaitForSecondsRealtime(Waves[waveToSpawn].SpawnCooldown);
        }

        yield return null;
    }

    private IEnumerator c_WaveFinished()
    {
        m_CurrentWave++;

        yield return new WaitForSecondsRealtime(WaveCooldown);

        StartWave();
    }

    private void ObjectDestroyed(DeathStruct data)
    {
        m_ObjectsLeft--;

        Debug.Log("Objects Left = " + m_ObjectsLeft);

        if(m_ObjectsLeft <= 0)
        {
            Debug.Log("All Objects Destroyed");
            c_WaveCooldown = StartCoroutine(c_WaveFinished());
        }
    }
}
