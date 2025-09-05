using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    public static PlayerDataUI Instance { get; private set; }

    [SerializeField] PlayerData playerData;
    private PlayerController _playerController;
    private PlayerEquipment _playerEquipment;

    public int Gold => playerData.Gold;
    public int Gem => 0;
    public float CurrentHP => _playerController.hp;
    public float CurrentEnergy => _playerController.Energy;
    public float MaxHP => _playerEquipment ? _playerEquipment.Hp : (playerData ? playerData.HP : 100f);
    public float MaxEnergy => _playerEquipment ? _playerEquipment.Energy : (playerData ? playerData.Energy : 100f);
    public int Level => playerData.Level;
    public float CurrentEXP => playerData.Exp;
    public float MaxEXP => playerData.MaxExp;
    public float AttackPoint => _playerEquipment ? _playerEquipment.Power : (_playerController ? _playerController.power : 0f);
    public float AttackSpeed => _playerEquipment ? _playerEquipment.AtkSpeed : (_playerController ? _playerController.atkSpeed : 1f);
    public float DefensePoint => _playerEquipment ? _playerEquipment.Armor : (_playerController ? _playerController.armor : 0f);


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
