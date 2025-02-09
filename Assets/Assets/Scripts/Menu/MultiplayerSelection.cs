using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MultiplayerSelection : MonoBehaviour
{
    UIDocument selection;
    [SerializeField] UIDocument menu;
    [SerializeField] private NetworkObject m_PlayerObj;
    [SerializeField] private LevelManager levelManager;
    
    private void OnEnable()
    {
        selection = GetComponent<UIDocument>();

        Button host = selection.rootVisualElement.Q<Button>("BTN_Host");
        host.RegisterCallback<ClickEvent>(handleHostButton);

        Button join = selection.rootVisualElement.Q<Button>("BTN_Join");
        join.RegisterCallback<ClickEvent>(handleJoinButton);

        Button back = selection.rootVisualElement.Q<Button>("BTN_Back");
        back.RegisterCallback<ClickEvent>(handleBackButton);
    }

    void handleHostButton(ClickEvent data)
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.OnLoadComplete += levelManager.OnSceneLoaded;
        
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    void handleJoinButton(ClickEvent data)
    {
        selection.gameObject.SetActive(false);
        NetworkManager.Singleton.StartClient();
    }

    void handleBackButton(ClickEvent data)
    {
        menu.gameObject.SetActive(true);
        selection.gameObject.SetActive(false);
    }
    

    [Rpc(SendTo.Server)]
    private void SpawnPlayerRPC(ulong PlayerID)
    {
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn
        (
            m_PlayerObj,
            PlayerID,
            false,
            true,
            false,
            new Vector3(0, 0, 0),
            quaternion.identity
        );
    }
}
