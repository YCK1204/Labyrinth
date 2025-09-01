using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoblinData", menuName = "Creature/Monster/GoblinData", order = 1)]
public class GoblinData : MonsterData
{
    [SerializeField]
    private float _maxCheckDist = 0.3f;
    public float MaxCheckDist { get { return _maxCheckDist; } }
    [SerializeField]
    private Vector2 _attackHitboxOffset = Vector2.zero;
    public Vector2 AttackHitboxOffset { get { return _attackHitboxOffset; } }
}
