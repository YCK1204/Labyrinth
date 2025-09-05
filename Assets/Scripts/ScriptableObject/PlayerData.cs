using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Creature/PlayerData")]
public class PlayerData : CreatureData
{
    [SerializeField] private float energy = 100f;
    public float Energy { get => energy; set => energy = value; }

    [SerializeField] private int gold = 300;
    public int Gold { get => gold; set => gold = value; }

    [SerializeField] private int level = 1;
    public int Level { get => level; set => level = value; }

    [SerializeField] private int exp = 0;
    public int Exp { get => exp; set => exp = value; }

    public int MaxExp => 100 * Level * Level;

    [SerializeField] private bool isHardMode = false;
    public bool IsHardMode
    {
        get => isHardMode;
        set
        {
            if (isHardMode == value) return;
            isHardMode = value;
        }
    }

    [System.Serializable]
    public struct GrowthPerLevel
    {
        public float Hp;
        public float Power;
        public float AtkSpeed;
        public float Armor;
        public float Crit;
    }

    public GrowthPerLevel growth = new GrowthPerLevel
    {
        Hp = 10f,
        Power = 1f,
        AtkSpeed = 0.04f,
        Armor = 1f,
        Crit = 0.5f
    };

    public EquipmentData equippedWeapon;
    public EquipmentData equippedArmor;

    public int AddExp(int amount)
    {
        if (amount <= 0) return 0;
        Exp += amount;
        int gained = 0;
        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            Level++;
            ApplyLevelGrowth();
            gained++;
        }
        return gained;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        Gold += amount;
    }

    void ApplyLevelGrowth()
    {
        HP += growth.Hp;
        Power += growth.Power;
        AtkSpeed += growth.AtkSpeed;
        Armor += growth.Armor;
        Crit = Mathf.Clamp(Crit + growth.Crit, 0f, 100f);

        var data = Manager.Audio.Player.GetAudiodata(PlayerAudioType.Player);
        var pos = Camera.main ? Camera.main.transform.position : Vector3.zero;
        Manager.Audio.PlayOneShot(data.LevelUp, pos);

    }
    public void ClearEquipment()
    {
        equippedWeapon = null;
        equippedArmor  = null;
    }
}
