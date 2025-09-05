using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        if (!playButton)
            playButton = transform.Find("ButtonArea/PlayButton")?.GetComponent<Button>();
        if (!settingButton)
            settingButton = transform.Find("ButtonArea/SettingButton")?.GetComponent<Button>();
        if (!exitButton)
            exitButton = transform.Find("ButtonArea/ExitButton")?.GetComponent<Button>();

        if (playButton) playButton.onClick.AddListener(OnPlayClicked);
        if (settingButton) settingButton.onClick.AddListener(OnSettingClicked);
        if (exitButton) exitButton.onClick.AddListener(OnExitClicked);
    }
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void OnPlayClicked()
    {
        GameManager.Instance.LoadLobbyScene();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnSettingClicked()
    {
        if (Manager.UI != null)
        {
            Manager.UI.ShowSettingUI();
        }
    }

    void OnExitClicked()
    {
        if (Manager.UI != null)
        {
            Manager.UI.ShowExitUI();
        }
    }
}
