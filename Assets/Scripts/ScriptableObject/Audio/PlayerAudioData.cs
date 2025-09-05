using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioData", menuName = "Audio/PlayerAudioData", order = 1)]
public class PlayerAudioData : CreatureAudioData
{
    [SerializeField]
    PlayerAudioType playerAudioType;
    public PlayerAudioType PlayerAudioType { get { return playerAudioType; } }

    [SerializeField] private AudioClip levelUp;
    public AudioClip LevelUp => levelUp;
}
