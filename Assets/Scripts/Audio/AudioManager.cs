using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAudioManager<T1, T2> where T1 : Enum where T2 : AudioData
{
    public void Init(List<T2> audioData);
    public void Clear();
    public T2 GetAudiodata(T1 type);
}

public class AudioManager : MonoBehaviour
{
    public PlayerAudio Player = new PlayerAudio();
    public MapEffectAudio MapEffect = new MapEffectAudio();
    public MonsterAudio Monster = new MonsterAudio();
    public UIAudio UI = new UIAudio();
    [SerializeField] private AudioSource prefab;
    private Queue<AudioSource> pool = new Queue<AudioSource>();
    AudioSource _bgmSource;
    void Start()
    {
        if (Manager.Audio != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        Manager.Audio = this;
        _bgmSource = gameObject.AddComponent<AudioSource>();
    }
    void Clear()
    {
        Player.Clear();
        MapEffect.Clear();
        Monster.Clear();
        UI.Clear();
    }
    public void PlayOneShot(AudioClip clip, Vector3 position)
    {
        AudioSource source = GetSource();
        source.transform.position = position;
        source.PlayOneShot(clip);
        StartCoroutine(ReleaseAfter(source, clip.length));
    }

    private AudioSource GetSource()
    {
        if (pool.Count > 0) return pool.Dequeue();
        return Instantiate(prefab);
    }

    private IEnumerator ReleaseAfter(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        pool.Enqueue(source);
    }
    public void SetBgm(AudioClip bgm)
    {
        _bgmSource.clip = bgm;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }
}
