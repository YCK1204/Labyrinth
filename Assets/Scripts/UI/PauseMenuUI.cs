using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject exitUI;
    public GameObject settingUI;

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
        Manager.UI.HidePauseMenuUI
    }
}
