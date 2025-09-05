using UnityEngine;
using UnityEngine.SceneManagement;

public class HardmodeUI : MonoBehaviour
{
    [SerializeField] private GameObject hardModePanel;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateUI();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (hardModePanel != null)
            hardModePanel.SetActive(Manager.Game.PlayerData.IsHardMode);
    }
}
