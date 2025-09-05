using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private PlayerController _pc;
    public PlayerController Player => _pc;

    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (_pc == null)
            _pc = FindObjectOfType<PlayerController>();

        if (_playerData == null && _pc != null)
            _playerData = _pc.PlayerData;

        Manager.Game = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_pc == null)
            _pc = FindObjectOfType<PlayerController>();
    }
    public void LoadLobbyScene()
    {
        if (_playerData)
        {
            _playerData.equippedWeapon = null;
            _playerData.equippedArmor = null;
        }
        SceneManager.LoadScene("LobbyScene");
    }
}
