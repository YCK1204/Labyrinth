using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureData : ScriptableObject
{
    [SerializeField]
    private float _hp;
    public float HP { get { return _hp; } }
    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } }
    [SerializeField]
    private float _armor;
    public float Armor { get { return _armor; } }
    [SerializeField]
    private float _power;
    public float Power { get { return _power; } }
    [SerializeField]
    private float _crit;
    public float Crit { get { return _crit; } }
    [SerializeField]
    private int _lv;
    public int LV { get { return _lv; } }
    [SerializeField]
    private float _kbResist;
    public float KBResist { get { return _kbResist; } }
    [SerializeField]
    private float _critX;
    public float CritX { get { return _critX; } }
    [SerializeField]
    private float _armorPen;
    public float ArmorPen { get { return _armorPen; } }
    [SerializeField]
    private float _atkSpeed;
    public float AtkSpeed { get { return _atkSpeed; } }
}
