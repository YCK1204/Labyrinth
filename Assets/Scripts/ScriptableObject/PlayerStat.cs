using UnityEngine;

[CreateAssetMenu(menuName = "Creature/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Base Stats")]
    public float hp = 100f;
    public float speed = 4f;
    public float armor = 5f;
    public float power = 8f;
    public float crit = 0f;
    public int lv = 1;
    public int exp = 0;
    public float kbResist = 0.2f;
    public float critX = 1.5f;
    public float armorPen = 0f;
    public float atkSpeed = 2f;
    public int gold = 300;
}