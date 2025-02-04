using System.Collections;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
struct WaveData
{
    [SerializeField] public NetworkPrefab[] ObjectsToSpawn;
    [SerializeField] public int ObjectAmount;
    [SerializeField] public float SpawnCooldown;
}


public class WaveSpawner : NetworkBehaviour
{
    [SerializeField] WaveData[] Waves;
    [SerializeField] float WaveCooldown;

    private int m_CurrentWave = 0;
    private int m_MaxWave = 0;

    private void Start()
    {
        m_MaxWave = Waves.Length;
    }

    public void StartWave()
    {
       
    }

    private IEnumerator c_SpawnWave()
    {
        for (int i = 0; i < Waves[m_CurrentWave].ObjectAmount; i++)
        {

        }
        yield return null; 
    }

    private IEnumerator c_Cooldown()
    {
        yield return null;
    }
}
