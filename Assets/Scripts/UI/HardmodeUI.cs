using UnityEngine;

public class HardmodeUI : MonoBehaviour
{
    [SerializeField] private GameObject hardModePanel;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (hardModePanel != null)
        {
            if (GameManager.Instance.PlayerData.IsHardMode)
                hardModePanel.SetActive(true);
            else
                hardModePanel.SetActive(false);
        }
    }
}
