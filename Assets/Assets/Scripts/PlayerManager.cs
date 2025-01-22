using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> m_PlayerCount = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    private TextMeshProUGUI m_PlayerCouterDisplay;

    private void Awake()
    {
        m_PlayerCouterDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                m_PlayerCount.Value++;
            };

            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                m_PlayerCount.Value--;
            };

            m_PlayerCount.OnValueChanged += (int previousValue, int newValue) =>
            {
                Debug.Log($"The current amount of players connected is {m_PlayerCount.Value}");
            };
        }
        else if (IsClient)
        {
            m_PlayerCount.OnValueChanged += (int previousValue, int newValue) =>
            {
                m_PlayerCouterDisplay.text = $"Current player count is {m_PlayerCount.Value}";
            };
        }
    }
}
