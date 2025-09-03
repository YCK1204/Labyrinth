using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject exitPanel;
    public GameObject settingPanel;

    public TextMeshProUGUI atkCountText;
    public TextMeshProUGUI atkSpdCountText;
    public TextMeshProUGUI defCountText;
    public TextMeshProUGUI hpCountText;

    private PlayerDataUI playerDataUI;
    private bool isPaused = false;

    private void Start()
    {
        playerDataUI = PlayerDataUI.Instance;

        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {

            }
        }
    }

    
}
