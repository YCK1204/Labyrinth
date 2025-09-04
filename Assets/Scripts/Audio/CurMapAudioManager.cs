using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurMapAudioManager : MonoBehaviour
{
    [SerializeField]
    BgmAudioData bgmAudioData;
    [SerializeField]
    List<MonsterAudioData> monsterAudioData = new List<MonsterAudioData>();
    [SerializeField]
    List<PlayerAudioData> playerAudioData = new List<PlayerAudioData>();
    [SerializeField]
    List<MapEffectAudioData> mapEffectAudioData = new List<MapEffectAudioData>();
    [SerializeField]
    List<UIAudioData> uiAudioData = new List<UIAudioData>();

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null;
        Manager.Audio.Monster.Init(monsterAudioData);
        Manager.Audio.Player.Init(playerAudioData);
        Manager.Audio.MapEffect.Init(mapEffectAudioData);
        Manager.Audio.UI.Init(uiAudioData);
        Manager.Audio.SetBgm(bgmAudioData.Bgm);
        Object.Destroy(this);
    }
}
