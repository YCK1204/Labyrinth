using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Creature/PlayerData")]
public class PlayerData : CreatureData
{
    [SerializeField]
    private float energy = 100f;
    public float Energy { get { return energy; } set { energy = value; } }
    [SerializeField]
    private int gold = 300;
    public int Gold { get { return gold; } set { gold = value; } }
    [SerializeField]
    private int level = 1;
    public int Level { get { return level; } set { level = value; } }
    private int exp = 0;
    public int Exp { get { return exp; } set { exp = value; } }

    public int MaxExp => 100 * Level * Level;
}
