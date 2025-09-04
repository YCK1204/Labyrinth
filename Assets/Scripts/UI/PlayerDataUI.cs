using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    public static PlayerDataUI Instance { get; private set; }

    [SerializeField] PlayerData PlayerData;

    private PlayerController _playerController;

    public int Gold => PlayerData.Gold;
    public int Gem => 0;
    public float CurrentHP => _playerController.hp;
    public float MaxHP => PlayerData.HP;
    public float CurrentEnergy => _playerController.Energy;
    public float MaxEnergy => PlayerData.Energy;
    public int Level => PlayerData.Level;
    public float currentEXP => PlayerData.Exp;
    public float MaxEXP => PlayerData.MaxExp;
    public float AttackPoint => _playerController.power;
    public float AttackSpeed => _playerController.atkSpeed;
    public float DefensePoint => _playerController.armor;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerController = FindObjectOfType<PlayerController>();
    }
}
