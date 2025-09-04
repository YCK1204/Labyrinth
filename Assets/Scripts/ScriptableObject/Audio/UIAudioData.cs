using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAudioData", menuName = "Audio/UIAudioData")]
public class UIAudioData : AudioData
{
    [SerializeField]
    UIAudioType uIAudioType;
    public UIAudioType UIAudioType { get { return uIAudioType; } }
    [SerializeField]
    AudioClip effectSound;
    public AudioClip EffectSound { get { return effectSound; } }
}
