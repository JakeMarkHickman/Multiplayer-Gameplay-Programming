using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MultiplayerSelection : MonoBehaviour
{
    UIDocument selection;
    [SerializeField] UIDocument menu;

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
        selection.gameObject.SetActive(false);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    void handleJoinButton(ClickEvent data)
    {
        selection.gameObject.SetActive(false);
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    void handleBackButton(ClickEvent data)
    {
        menu.gameObject.SetActive(true);
        selection.gameObject.SetActive(false);
    }
}
