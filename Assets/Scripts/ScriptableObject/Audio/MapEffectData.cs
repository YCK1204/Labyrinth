using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapEffectAudioData", menuName = "Audio/MapEffectAudioData")]
public class MapEffectAudioData : AudioData
{
    [SerializeField]
    MapEffectAudioType effectAudioType;
    public MapEffectAudioType EffectAudioType { get { return effectAudioType; } }
    [SerializeField]
    AudioClip effectSound;
    public AudioClip EffectSound { get { return effectSound; } }
}
