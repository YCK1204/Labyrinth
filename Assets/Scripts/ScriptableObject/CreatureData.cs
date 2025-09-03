using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureData : ScriptableObject
{
    [SerializeField]
    private float _hp;
    public float HP { get { return _hp; } set { _hp = value; } }
    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    [SerializeField]
    private float _armor;
    public float Armor { get { return _armor; } set { _armor = value; } }
    [SerializeField]
    private float _power;
    public float Power { get { return _power; } set { _power = value; } }
    [SerializeField]
    private float _crit;
    public float Crit { get { return _crit; } set { _crit = value; } }
    [SerializeField]
    private int _lv;
    public int LV { get { return _lv; } set { _lv = value; } }
    [SerializeField]
    private float _kbResist;
    public float KBResist { get { return _kbResist; } set { _kbResist = value; } }
    [SerializeField]
    private float _critX;
    public float CritX { get { return _critX; } set { _critX = value; } }
    [SerializeField]
    private float _armorPen;
    public float ArmorPen { get { return _armorPen; } set { _armorPen = value; } }
    [SerializeField]
    private float _atkSpeed;
    public float AtkSpeed { get { return _atkSpeed; } set { _atkSpeed = value; } }
}
