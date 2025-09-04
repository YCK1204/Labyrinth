using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;
    public PlayerController Player => _pc;

    private void Awake()
    {
        if (_pc == null)
            _pc = FindObjectOfType<PlayerController>();

        Manager.Game = this;
    }
}
