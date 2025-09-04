using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button closeButton;

    void Awake()
    {
        if (!upgradePanel)
            upgradePanel = transform.Find("UpgradePanel")?.gameObject;

        if (!closeButton)
            closeButton = transform.Find("UpgradePanel/CloseButton")?.GetComponent<Button>();

        if (closeButton) closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        if (upgradePanel) upgradePanel.SetActive(false);
        else gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }
}
