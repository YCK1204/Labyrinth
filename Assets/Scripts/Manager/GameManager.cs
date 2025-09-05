using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;
    public PlayerController Player => _pc;

    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    private void Awake()
    {
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
    public void ResetPlayerData()
    {
        if (_playerData == null) return;

        _playerData.Level = 1;
        _playerData.Exp = 0;
        _playerData.HP = 100;

        _playerData.Power = 5;
        _playerData.AtkSpeed = 2f;
        _playerData.Armor = 5;
        _playerData.ArmorPen = 0;
        _playerData.Speed = 8;

        _playerData.Crit = 0f;
        _playerData.CritX = 1.5f;
        _playerData.KBResist = 0.2f;

        _playerData.Exp = 0;

        _playerData.equippedWeapon = null;
        _playerData.equippedArmor  = null;
    }
}
