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
    // �񵿱� �ε� �߰� ���ɼ�?
}
