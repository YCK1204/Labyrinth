using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossMonsterData", menuName = "Creature/Monster/BossMonsterData", order = 2)]
public class BossMonsterData : GroundMonsterData
{
    [SerializeField]
    float walkingSpeed = 2f;
    public float WalkingSpeed { get { return walkingSpeed; } }
    [SerializeField]
    float runningSpeed = 4f;
    public float RunningSpeed { get { return runningSpeed; } }
    [SerializeField]
    float leapSpeed = 4f;
    public float LeapSpeed { get { return leapSpeed; } }
    [SerializeField]
    float fastChaseArea = 10f;
    public float FastChaseArea { get { return fastChaseArea; } }
    [SerializeField]
    float cooldownTime = .3f;
    public float CooldownTime { get { return cooldownTime; } }
}
