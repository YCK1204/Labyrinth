using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class ShopNPC : MonoBehaviour
{
    [Header("Player Detect")]
    [SerializeField] private string playerTag = "Player";

    [Header("Shop UI")]
    [SerializeField] private GameObject uiRoot;
    [SerializeField] private Button closeButton; // ✨ 닫기 버튼

    public bool IsOpen { get; private set; }

    bool _playerInRange;

    void Awake()
    {
        if (uiRoot) uiRoot.SetActive(false);

        if (closeButton)
            closeButton.onClick.AddListener(CloseShop);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        _playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        _playerInRange = false;
    }

    void Update()
    {
        if (_playerInRange && !IsOpen && Input.GetKeyDown(KeyCode.F))
            OpenShop();

        if (IsOpen && Input.GetKeyDown(KeyCode.Escape))
            CloseShop();
    }

    public void OpenShop()
    {
        if (IsOpen) return;
        IsOpen = true;

        if (uiRoot) uiRoot.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        if (!IsOpen) return;
        IsOpen = false;

        if (uiRoot) uiRoot.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1f;
    }
}
