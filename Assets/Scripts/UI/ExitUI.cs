using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    public void OnYesButtonClicked()
    {
        Manager.Audio.ClickBtn();
        Manager.Game.ResetPlayerData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNoButtonClicked()
    {
        if (Manager.UI != null)
        {
            Manager.Audio.ClickBtn();
            Manager.UI.HideExitUI();
        }
    }
}
