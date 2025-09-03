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
        Hp = 10f, Power = 1f, AtkSpeed = 0.04f, Armor = 1f, Crit = 0.5f
    };
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

    void ApplyLevelGrowth()
    {
        HP       += growth.Hp;
        Power    += growth.Power;
        AtkSpeed += growth.AtkSpeed;
        Armor    += growth.Armor;
        Crit      = Mathf.Clamp(Crit + growth.Crit, 0f, 100f);
    }
}
