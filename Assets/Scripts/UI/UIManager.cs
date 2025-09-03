using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startScreenUI;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject playerStatusUI;
    [SerializeField] private GameObject pauseMenuUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Manager.UI = this;
    }

    // 특정 UI 패널을 활성화하는 함수들
    public void ShowStartScreen() => startScreenUI.SetActive(true);
    public void ShowSettingPanel() => settingPanel.SetActive(true);
    public void ShowExitPanel() => exitPanel.SetActive(true);
    public void ShowShopUI() => shopUI.SetActive(true);
    public void ShowUpgradeUI() => upgradeUI.SetActive(true);
    public void ShowPlayerStatusUI() => playerStatusUI.SetActive(true);
    public void ShowPauseMenu() => pauseMenuUI.SetActive(true);
    // ------------------------------------------------------------
    

    // 특정 UI 패널을 비활성화하는 함수들
    public void HideStartScreen() => startScreenUI.SetActive(false);
    public void HideSettingPanel() => settingPanel.SetActive(false);
    public void HideExitPanel() => exitPanel.SetActive(false);
    public void HideShopUI() => shopUI.SetActive(false);
    public void HideUpgradeUI() => upgradeUI.SetActive(false);
    public void HidePlayerStatusUI() => playerStatusUI.SetActive(false);
    public void HidePauseMenu() => pauseMenuUI.SetActive(false);
    // ---------------------------------------------------------
    

}
