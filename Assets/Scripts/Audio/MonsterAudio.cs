using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : IAudioManager<MonsterAudioType, MonsterAudioData>
{
    Dictionary<MonsterAudioType, MonsterAudioData> _audioDict = new Dictionary<MonsterAudioType, MonsterAudioData>();

    public void Clear()
    {
        _audioDict.Clear();
    }
    public MonsterAudioData GetAudiodata(MonsterAudioType type)
    {
        MonsterAudioData data = null;
        _audioDict.TryGetValue(type, out data);
        return data;
    }
    public void Init(List<MonsterAudioData> audioData)
    {
        foreach (var data in audioData)
        {
            if (_audioDict.ContainsKey(data.MonsterAudioType))
            {
                Debug.LogError($"Duplicate MonsterAudioType {data.MonsterAudioType} in MonsterAudioData {data.name}");
                continue;
            }
            _audioDict[data.MonsterAudioType] = data;
        }
    }
}
