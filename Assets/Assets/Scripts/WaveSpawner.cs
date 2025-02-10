using System;
using System.Collections;
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

public struct WaveCompletedStruct
{
    
}

public struct RoundCompletedStruct
{
    
}


public class WaveSpawner : NetworkBehaviour
{
    [SerializeField] float MaxX;
    [SerializeField] float MaxY;
    [SerializeField] float MinX;
    [SerializeField] float MinY;

    [SerializeField] WaveData[] Waves;
    [SerializeField] float WaveCooldown;
    [SerializeField] private float RoundMultiplier;

    [SerializeField] private bool LoopLastRound;

    [SerializeField] private float waveDifficulty = 1;

    public event Action<WaveCompletedStruct> onWaveCompletedEvent;
    public event Action<RoundCompletedStruct> onRoundCompletedEvent;
    
    private bool m_OnCooldown = false;

    private int m_ObjectsLeft = 0;
    private int m_MaxWave = 0;

    private bool DoneOnce = false;
    private bool OneRoundComplete = false;

    private Coroutine c_ObjectSpawn;
    private Coroutine c_WaveCooldown;
    
    [SerializeField] public NetworkVariable<int> m_CurrentWave = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    
    [SerializeField] public NetworkVariable<int> m_CurrentRound = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

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
        int waveToSpawn = m_CurrentWave.Value % m_MaxWave;

        if (DoneOnce)
        {
            if (waveToSpawn == 0)
            {
                OneRoundComplete = true;
                m_CurrentRound.Value++;
                waveDifficulty *= RoundMultiplier;
                onRoundCompletedEvent?.Invoke(new RoundCompletedStruct());

                if(LoopLastRound)
                    waveToSpawn = m_MaxWave - 1;
            }
        }

        DoneOnce = true;
        
        m_ObjectsLeft = Waves[waveToSpawn].ObjectAmount;

        for (int i = 0; i < Waves[waveToSpawn].ObjectAmount; i++)
        {
            int random = UnityEngine.Random.Range(0, Waves[waveToSpawn].ObjectsToSpawn.Length);

            float SpawnX = UnityEngine.Random.Range(MinX, MaxX);
            float SpawnY = UnityEngine.Random.Range(MinY, MaxY);

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

            if (netObj.TryGetComponent<Damage>(out Damage dmgComp))
            {
                dmgComp.SetMaxDamageRPC(dmgComp.GetMaxDamage() + waveDifficulty);
                dmgComp.SetMinDamageRPC(dmgComp.GetMinDamage() + waveDifficulty);
            }

            if(healthcomp)
            {
                healthcomp.SetMaxHealthRPC(healthcomp.GetMaxHealth() + waveDifficulty, gameObject.tag);
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
        m_CurrentWave.Value++;

        yield return new WaitForSecondsRealtime(WaveCooldown);

        StartWave();
    }

    private void ObjectDestroyed(DeathStruct data)
    {
        m_ObjectsLeft--;

        if(m_ObjectsLeft <= 0)
        {
            c_WaveCooldown = StartCoroutine(c_WaveFinished());
            onWaveCompletedEvent?.Invoke(new WaveCompletedStruct());
        }
    }
}
