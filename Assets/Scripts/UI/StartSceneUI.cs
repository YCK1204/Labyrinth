using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject exitPanel;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private string lobbySceneName = "LobbyScene";

    void Awake()
    {
        if (!playButton)
            playButton = transform.Find("ButtonArea/PlayButton")?.GetComponent<Button>();
        if (!settingButton)
            settingButton = transform.Find("ButtonArea/SettingButton")?.GetComponent<Button>();
        if (!exitButton)
            exitButton = transform.Find("ButtonArea/ExitButton")?.GetComponent<Button>();

        if (!settingPanel)
            settingPanel = transform.parent.Find("SettingPanel")?.gameObject;
        if (!exitPanel)
            exitPanel = transform.parent.Find("ExitPanel")?.gameObject;

        if (!closeButton && settingPanel)
            closeButton = settingPanel.transform.Find("CloseButton")?.GetComponent<Button>();
        if (!yesButton && exitPanel)
            yesButton = exitPanel.transform.Find("YesButton")?.GetComponent<Button>();
        if (!noButton && exitPanel)
            noButton = exitPanel.transform.Find("NoButton")?.GetComponent<Button>();

        if (playButton)    playButton.onClick.AddListener(OnPlayClicked);
        if (settingButton) settingButton.onClick.AddListener(OnSettingClicked);
        if (exitButton)    exitButton.onClick.AddListener(OnExitClicked);

        if (closeButton)   closeButton.onClick.AddListener(OnCloseSetting);
        if (yesButton)     yesButton.onClick.AddListener(OnExitYes);
        if (noButton)      noButton.onClick.AddListener(OnExitNo);

        if (settingPanel) settingPanel.SetActive(false);
        if (exitPanel) exitPanel.SetActive(false);
    }

    void OnPlayClicked()
    {
        SceneManager.LoadScene(lobbySceneName);
    }

    void OnSettingClicked()
    {
        if (settingPanel) settingPanel.SetActive(true);
    }

    void OnExitClicked()
    {
        if (exitPanel) exitPanel.SetActive(true);
    }

    void OnCloseSetting()
    {
        if (settingPanel) settingPanel.SetActive(false);
    }

    void OnExitYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnExitNo()
    {
        if (exitPanel) exitPanel.SetActive(false);
    }
}
