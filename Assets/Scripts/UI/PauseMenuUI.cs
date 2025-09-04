using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public TextMeshProUGUI atkCountText;
    public TextMeshProUGUI atkSpdCountText;
    public TextMeshProUGUI defCountText;
    public TextMeshProUGUI hpCountText;

    private PlayerDataUI _playerDataUI;
    private bool _isPaused = false;

    private void Start()
    {
        _playerDataUI = PlayerDataUI.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.HidePauseMenuUI();
        }
    }

    public void OnResumeButtonClicked()
    {
        Manager.UI.HidePauseMenuUI();
    }

    public void OnSettingButtonClicked()
    {
        Manager.UI.ShowSettingUI();
    }

    public void OnExitButtonClicked()
    {
        Manager.UI.ShowExitUI();
    }

    private void UpdatePlayerStats()
    {
        if (_playerDataUI == null) return;

        atkCountText.text = _playerDataUI.AttackPoint.ToString();
        atkSpdCountText.text = _playerDataUI.AttackSpeed.ToString();
        defCountText.text = _playerDataUI.DefensePoint.ToString();
        hpCountText.text = _playerDataUI.CurrentHP.ToString();
    }
}
