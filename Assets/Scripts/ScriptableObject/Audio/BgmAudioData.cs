using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BgmAudioData", menuName = "Audio/BgmAudioData")]
public class BgmAudioData : AudioData
{
    [SerializeField]
    AudioClip bgm;
    public AudioClip Bgm { get { return bgm; } }
}
