using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startScreenUIPrefeb;
    [SerializeField] private GameObject settingUIPrefeb;
    [SerializeField] private GameObject exitUIPrefeb;
    [SerializeField] private GameObject shopUIPrefeb;
    [SerializeField] private GameObject upgradeUIPrefeb;
    [SerializeField] private GameObject playerStatusUIPrefeb;
    [SerializeField] private GameObject topButtonUIPrefeb;
    [SerializeField] private GameObject pauseMenuUIPrefeb;

    private GameObject startScreenUIInstance;
    private GameObject settingUIInstance;
    private GameObject exitUIInstance;
    private GameObject shopUIInstance;
    private GameObject upgradeUIInstance;
    private GameObject playerStatusUIInstance;
    private GameObject topButtonUIInstance;
    private GameObject pauseMenuUIInstance;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Manager.UI = this;
        SetSceneUI(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetSceneUI(scene.name);
    }

    public void SetSceneUI(string sceneName)
    {
        DestroyAllUI();

        switch (sceneName)
        {
            case "StartScene":
                startScreenUIInstance = Instantiate(startScreenUIPrefeb);
                break;
            case "LobbyScene":
                playerStatusUIInstance = Instantiate(playerStatusUIPrefeb);
                topButtonUIInstance = Instantiate(topButtonUIPrefeb);
                break;
            case "DungeonScene":
                playerStatusUIInstance = Instantiate(playerStatusUIPrefeb);
                topButtonUIInstance = Instantiate(topButtonUIPrefeb);
                break;
        }
    }

    public void DestroyAllUI()
    {
        if (startScreenUIInstance != null) Destroy(startScreenUIInstance);
        if (settingUIInstance != null) Destroy(settingUIInstance);
        if (exitUIInstance != null) Destroy(exitUIInstance);
        if (shopUIInstance != null) Destroy(shopUIInstance);
        if (upgradeUIInstance != null) Destroy(upgradeUIInstance);
        if (playerStatusUIInstance != null) Destroy(playerStatusUIInstance);
        if (topButtonUIInstance != null) Destroy(topButtonUIInstance);
        if (pauseMenuUIInstance != null) Destroy(pauseMenuUIInstance);
    }

    // startScreenUIInstance
    public void ShowStartScreenUI() => startScreenUIInstance?.SetActive(true);
    public void HideStartScreenUI() => startScreenUIInstance?.SetActive(false);

    // playerStatusUIInstance
    public void ShowPlayerStatusUI() => playerStatusUIInstance?.SetActive(true);
    public void HidePlayerStatusUI() => playerStatusUIInstance?.SetActive(false);

    // topButtonUIInstance
    public void ShowTopButtonUI() => topButtonUIInstance?.SetActive(true);
    public void HideTopButtonUI() => topButtonUIInstance?.SetActive(false);

    // settingUIInstance
    public void ShowSettingUI()
    {
        if (settingUIInstance == null)
        {
            settingUIInstance = Instantiate(settingUIPrefeb);
        }
        settingUIInstance.SetActive(true);
    }
    public void HideSettingUI() => settingUIInstance?.SetActive(false);

    // exitUIInstance
    public void ShowExitUI()
    {
        if (exitUIInstance == null)
        {
            exitUIInstance = Instantiate(exitUIPrefeb);
        }
        exitUIInstance.SetActive(true);
    }
    public void HideExitUI() => exitUIInstance?.SetActive(false);

    // shopUIInstance
    public void ShowShopUI()
    {
        if (shopUIInstance == null)
        {
            shopUIInstance = Instantiate(shopUIPrefeb);
        }
        shopUIInstance.SetActive(true);
    }
    public void HideShopUI() => shopUIInstance?.SetActive(false);

    // upgradeUIInstance
    public void ShowUpgradeUI()
    {
        if (upgradeUIInstance == null)
        {
            upgradeUIInstance = Instantiate(upgradeUIPrefeb);
        }
        upgradeUIInstance.SetActive(true);
    }
    public void HideUpgradeUI() => upgradeUIInstance?.SetActive(false);

    // pauseMenuUIInstance
    public void ShowPauseMenuUI()
    {
        if (pauseMenuUIInstance == null)
        {
            pauseMenuUIInstance = Instantiate(pauseMenuUIPrefeb);
        }
        Time.timeScale = 0f;
        pauseMenuUIInstance.SetActive(true);
    }
    public void HidePauseMenuUI()
    {
        if (pauseMenuUIInstance != null)
        {
            pauseMenuUIInstance.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
