using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameObject clearUI;
    [SerializeField] private Button returnButton;
    [SerializeField] private string lobbySceneName = "LobbyScene";
    [SerializeField] private PlayerData playerData;

    void Awake()
    {
        if (clearUI == null)
            clearUI = transform.Find("UI_Clear")?.gameObject;

        if (returnButton == null && clearUI != null)
            returnButton = clearUI.transform.Find("ReturnButton")?.GetComponent<Button>();

        if (playerData == null)
            playerData = Resources.Load<PlayerData>("New Player Data"); 

        if (clearUI) clearUI.SetActive(false);
        if (returnButton) returnButton.onClick.AddListener(OnReturnClicked);
    }

    public void Show()
    {
        if (clearUI)
        {
            clearUI.SetActive(true);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnReturnClicked()
    {
        Manager.Game.ResetPlayerData();

        Manager.Audio.ClickBtn();
        Time.timeScale = 1f;
        Manager.Scene.LoadScene(lobbySceneName);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
