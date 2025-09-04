using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : IAudioManager<PlayerAudioType, PlayerAudioData>
{
    Dictionary<PlayerAudioType, PlayerAudioData> _audioDict = new Dictionary<PlayerAudioType, PlayerAudioData>();

    public void Clear()
    {
        _audioDict.Clear();
    }

    public PlayerAudioData GetAudiodata(PlayerAudioType type)
    {
        PlayerAudioData data = null;
        _audioDict.TryGetValue(type, out data);
        return data;
    }

    public void Init(List<PlayerAudioData> audioData)
    {
        foreach (var data in audioData)
        {

            if (_audioDict.ContainsKey(data.PlayerAudioType))
            {
                Debug.LogError($"Duplicate PlayerAudioType {data.PlayerAudioType} in PlayerAudioData {data.name}");
                continue;
            }
            _audioDict[data.PlayerAudioType] = data;
        }
    }
}
