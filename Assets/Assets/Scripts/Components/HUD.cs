using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : NetworkBehaviour
{
    [SerializeField] private UIDocument m_Hud;
    [SerializeField] private WaveSpawner _spawner;

    private Health m_HealthComp;

    [SerializeField] private NetworkVariable<int> m_Wave = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    
    [SerializeField] private NetworkVariable<int> m_Round = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    [Rpc(SendTo.Server)]
    private void IncrementRoundRPC()
    {
        m_Round.Value++;
    }
    
    [Rpc(SendTo.Server)]
    private void IncrementWaveRPC()
    {
        m_Wave.Value++;
    }

    private void OnEnable()
    {
        if (GameObject.Find("WaveSpawner").TryGetComponent<WaveSpawner>(out WaveSpawner _spawner))
        {
            _spawner.onRoundCompletedEvent += OnRoundCompleted;
            _spawner.onWaveCompletedEvent += OnWaveCompleted;
        }
        
        m_HealthComp = gameObject.GetComponent<Health>();
        m_HealthComp.onHealthChanged += OnHealthChanged;
    }
    
    private void OnDisable()
    {
        m_HealthComp.onHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(HealthChangeStruct data)
    {
        VisualElement healthBarRoot = m_Hud.rootVisualElement.Q<VisualElement>("Bar_Health");
        ProgressBar healthBar = healthBarRoot.Q<ProgressBar>("healthBar");
        float HealthPercent = m_HealthComp.GetHealth() / m_HealthComp.GetMaxHealth();

        healthBar.value = HealthPercent;
        healthBar.title = m_HealthComp.GetHealth() + " / " + m_HealthComp.GetMaxHealth();
    }
    
    private void OnRoundCompleted()
    {
        if (!IsServer)
            return;
        
        IncrementRoundRPC();
        UpdateWaveRoundDataRPC();
    }
    
    private void OnWaveCompleted()
    {
        if (!IsServer)
            return;
        
        IncrementWaveRPC();
        UpdateWaveRoundDataRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateWaveRoundDataRPC()
    {
        Label waveRoundData = m_Hud.rootVisualElement.Q<Label>("RoundWave_Lable");
        waveRoundData.text = "Round: " + m_Round.Value + "     Wave: " + m_Wave.Value;
    }
}
