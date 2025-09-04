using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : IAudioManager<UIAudioType, UIAudioData>
{
    Dictionary<UIAudioType, UIAudioData> _audioDict = new Dictionary<UIAudioType, UIAudioData>();

    public void Clear()
    {
        _audioDict.Clear();
    }

    public UIAudioData GetAudiodata(UIAudioType type)
    {
        UIAudioData data = null;
        _audioDict.TryGetValue(type, out data);
        return data;
    }

    public void Init(List<UIAudioData> audioData)
    {
        foreach (var data in audioData)
        {
            if (_audioDict.ContainsKey(data.UIAudioType))
            {
                Debug.LogError($"Duplicate UIAudioType {data.UIAudioType} in UIAudioData {data.name}");
                continue;
            }
            _audioDict[data.UIAudioType] = data;
        }
    }
}
