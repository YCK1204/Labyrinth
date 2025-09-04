using UnityEngine;
using UnityEngine.UI;

public class TopButtonUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    private static TopButtonUI instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (!pauseButton)
            pauseButton = transform.Find("PauseButton")?.GetComponent<Button>();

        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        if (Manager.UI != null)
            Manager.UI.ShowPauseMenuUI();
    }
}
