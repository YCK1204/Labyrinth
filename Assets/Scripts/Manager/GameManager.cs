using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;
    public PlayerController Player => _pc;

    private void Awake()
    {
        var existing = FindObjectsOfType<GameManager>();
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        if (_pc == null)
            _pc = FindObjectOfType<PlayerController>();

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
}
