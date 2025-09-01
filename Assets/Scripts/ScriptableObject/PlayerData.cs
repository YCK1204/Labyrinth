using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Creature/PlayerData")]
public class PlayerData : CreatureData
{
    [SerializeField]
    private int gold = 300;
    public int Gold { get { return gold; } set { gold = value; } }
    [SerializeField]
    private int exp = 0;
    public int Exp { get { return exp; } set { exp = value; } }
}
