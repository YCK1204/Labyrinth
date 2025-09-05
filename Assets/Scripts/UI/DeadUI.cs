using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadUI : MonoBehaviour
{
    [SerializeField] private GameObject deadUI;
    [SerializeField] private Button returnButton;
    [SerializeField] private string lobbySceneName = "LobbyScene";
    [SerializeField] private PlayerData playerData;

    void Awake()
    {
        if (deadUI == null)
            deadUI = transform.Find("UI_Dead")?.gameObject;

        if (returnButton == null && deadUI != null)
            returnButton = deadUI.transform.Find("ReturnButton")?.GetComponent<Button>();

        if (playerData == null)
            playerData = Resources.Load<PlayerData>("New Player Data"); 

        if (deadUI) deadUI.SetActive(false);
        if (returnButton) returnButton.onClick.AddListener(OnReturnClicked);
    }

    public void Show()
    {
        if (deadUI)
        {
            deadUI.SetActive(true);
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
