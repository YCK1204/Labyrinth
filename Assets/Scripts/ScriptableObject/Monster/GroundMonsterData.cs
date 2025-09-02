using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMonsterData : MonsterData
{
    [SerializeField]
    private float _maxCheckDist = 0.3f;
    public float MaxCheckDist { get { return _maxCheckDist; } }
    [SerializeField]
    private Vector2 _attackHitboxOffset = Vector2.zero;
    public Vector2 AttackHitboxOffset { get { return _attackHitboxOffset; } }
}
