using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopButtonUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        if (!pauseButton) pauseButton = transform.Find("PauseButton")?.GetComponent<Button>();

        if (pauseButton != null )
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }
    }

    private void OnPauseButtonClicked()
    {
        Manager.UI.ShowPauseMenuUI();
    }
}
