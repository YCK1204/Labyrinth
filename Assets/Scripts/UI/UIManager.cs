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

    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;
    private PlayerController _playerController;
    private PlayerEquipment _playerEquipment;

    public int Gold => playerData ? playerData.Gold : 0;
    public int Gem => 0;
    public float CurrentHP => _playerController ? _playerController.hp : 0f;
    public float CurrentEnergy => _playerController ? _playerController.Energy : 0f;

    public float MaxHP => _playerEquipment ? _playerEquipment.Hp : (playerData ? playerData.HP : 100f);
    public float MaxEnergy => _playerEquipment ? _playerEquipment.Energy : (playerData ? playerData.Energy : 100f);

    public int Level => playerData ? playerData.Level : 1;
    public float CurrentEXP => playerData ? playerData.Exp : 0f;
    public float MaxEXP => playerData ? playerData.MaxExp : 100f;

    public float AttackPoint => _playerEquipment ? _playerEquipment.Power : (_playerController ? _playerController.power : 0f);
    public float AttackSpeed => _playerEquipment ? _playerEquipment.AtkSpeed : (_playerController ? _playerController.atkSpeed : 1f);
    public float DefensePoint => _playerEquipment ? _playerEquipment.Armor : (_playerController ? _playerController.armor : 0f);

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (playerData == null)
        {
            playerData = Resources.Load<PlayerData>("Data/ScriptableObject/New Player Data");
        }
    }

    private void Start()
    {
        Manager.UI = this;
        SetSceneUI(SceneManager.GetActiveScene().name);
        PlayerDataReference();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "LobbyScene" ||  currentScene == "DungeonScene")
            {
                if (pauseMenuUIInstance != null && pauseMenuUIInstance.activeSelf)
                {
                    HidePauseMenuUI();
                }
                else
                {
                    ShowPauseMenuUI();
                }
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetSceneUI(scene.name);
        PlayerDataReference();
    }

    public void PlayerDataReference()
    {
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController != null)
            _playerEquipment = _playerController.GetComponent<PlayerEquipment>();
    }

    public void SetSceneUI(string sceneName)
    {
        DestroyAllUI();

        switch (sceneName)
        {
            case "StartScene":
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
