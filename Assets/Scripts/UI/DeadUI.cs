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
        if (deadUI) deadUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnReturnClicked()
    {
        ResetPlayerData();

        Time.timeScale = 1f;
        SceneManager.LoadScene(lobbySceneName);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void ResetPlayerData()
    {
        if (playerData == null) return;

        playerData.Level = 1;
        playerData.Exp = 0;
        playerData.HP = 100;

        playerData.Power = 5;
        playerData.AtkSpeed = 2f;
        playerData.Armor = 5;
        playerData.ArmorPen = 0;
        playerData.Speed = 8;
        playerData.Gold = 300;

        playerData.Crit = 0f;
        playerData.CritX = 1.5f;
        playerData.KBResist = 0.2f;
    }
}
