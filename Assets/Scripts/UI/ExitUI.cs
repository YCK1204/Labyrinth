using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    public void OnYesButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ONNOButtonClicked()
    {
        if (Manager.UI != null)
        {
            Manager.UI.HideExitUI();
        }
    }
}
