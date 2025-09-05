using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureAudioData : AudioData
{
    [SerializeField]
    AudioClip die;
    public AudioClip Die { get { return die; } }
    [SerializeField]
    AudioClip hitSuccess;
    public AudioClip HitSuccess { get { return hitSuccess; } }
    [SerializeField]
    AudioClip hitFail;
    public AudioClip HitFail { get { return hitFail; } }
}
