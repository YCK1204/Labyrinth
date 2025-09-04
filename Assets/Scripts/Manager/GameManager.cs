using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private PlayerController pc;  
    public PlayerController PC => pc;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (pc == null)
            pc = FindObjectOfType<PlayerController>();

        DontDestroyOnLoad(gameObject);
    }
}
