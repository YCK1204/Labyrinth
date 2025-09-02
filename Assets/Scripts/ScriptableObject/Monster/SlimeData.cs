using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeData", menuName = "Creature/Monster/SlimeData")]
public class SlimeData : MonsterData
{
    [SerializeField]
    private float _maxCheckDist = 0.3f;
    public float MaxCheckDist { get { return _maxCheckDist; } }
}
