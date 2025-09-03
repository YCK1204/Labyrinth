using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterData : CreatureData
{
    [SerializeField]
    private Sprite spriteForCollisionSize;
    public Sprite SpriteForCollisionSize { get { return spriteForCollisionSize; } }
    [SerializeField]
    private Patrol _patrol;
    public Patrol Patrol { get { return _patrol; } }
    [SerializeField]
    private float _attackHitboxRadius = 0.5f;
    public float AttackHitboxRadius { get { return _attackHitboxRadius; } }
    [SerializeField]
    private float _attackRange = 1.0f;
    public float AttackRange { get { return _attackRange; } }
    [SerializeField]
    private int exp = 100;
    public int Exp { get { return exp; } }
    [SerializeField]
    private int gold = 100;
    public int Gold { get { return gold; } }
}
