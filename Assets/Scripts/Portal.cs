using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    [SerializeField] private string dungeonSceneName = "DungeonScene";

    [Header("Player Detect")]
    [SerializeField] private string playerTag = "Player";

    bool _playerInRange;

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
        if (_playerInRange)
            MoveDungeon();
    }

    public void MoveDungeon()
    {
        SceneManager.LoadScene(dungeonSceneName);
    }

}
