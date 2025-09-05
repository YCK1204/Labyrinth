using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerEx
{
    public Action OnSceneChanged;
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        OnSceneChanged?.Invoke();
    }
    // 비동기 로드 추가 가능성?
}
