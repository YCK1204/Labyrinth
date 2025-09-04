using System;
using System.Collections.Generic;
using UnityEngine;

public class MapEffectAudio : IAudioManager<MapEffectAudioType, MapEffectAudioData>
{
    Dictionary<MapEffectAudioType, MapEffectAudioData> _audioDict = new Dictionary<MapEffectAudioType, MapEffectAudioData>();

    public void Clear()
    {
        _audioDict.Clear();
    }
    public MapEffectAudioData GetAudiodata(MapEffectAudioType type)
    {
        MapEffectAudioData data = null;
        _audioDict.TryGetValue(type, out data);
        return data;
    }
    public void Init(List<MapEffectAudioData> audioData)
    {
        foreach (var data in audioData)
        {
            if (_audioDict.ContainsKey(data.EffectAudioType))
            {
                Debug.LogError($"Duplicate MapEffectAudioType {data.EffectAudioType} in MapEffectAudioData {data.name}");
                continue;
            }
            _audioDict[data.EffectAudioType] = data;
        }
    }
}
