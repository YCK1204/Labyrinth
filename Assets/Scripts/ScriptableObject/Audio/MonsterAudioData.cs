using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterAudioData", menuName = "Audio/MonsterAudioData", order = 1)]
public class MonsterAudioData : CreatureAudioData
{
    [SerializeField]
    MonsterAudioType monsterAudioType;
    public MonsterAudioType MonsterAudioType { get { return monsterAudioType; } }
}
