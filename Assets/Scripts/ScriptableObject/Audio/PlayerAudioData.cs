using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioData : CreatureAudioData
{
    [SerializeField]
    PlayerAudioType playerAudioType;
    public PlayerAudioType PlayerAudioType { get { return playerAudioType; } }
}
