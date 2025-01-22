using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    UIDocument menu;
    [SerializeField] UIDocument MultiplayerSelection;

    private void OnEnable()
    {
        menu = GetComponent<UIDocument>();

        Button play = menu.rootVisualElement.Q<Button>("BTN_Play");
        play.RegisterCallback<ClickEvent>(handlePlayButton);

        Button exit = menu.rootVisualElement.Q<Button>("BTN_Exit");
        exit.RegisterCallback<ClickEvent>(handleExitButton);
    }

    void handlePlayButton(ClickEvent data)
    {
        MultiplayerSelection.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }

    void handleExitButton(ClickEvent data)
    {
        Application.Quit();
    }
}
