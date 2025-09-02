using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyingeyeData", menuName = "Creature/Monster/FlyingeyeData", order = 2)]
public class FlyingeyeData : FlyingMonsterData
{
    [SerializeField]
    float _onAttackDuration = 0.5f;
    public float OnAttackDuration { get { return _onAttackDuration; } }
    [SerializeField]
    float _finishAttackDuration = 0.5f;
    public float FinishAttackDuration { get { return _finishAttackDuration; } }
}
