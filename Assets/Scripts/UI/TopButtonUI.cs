using UnityEngine;

public class TopButtonUI : MonoBehaviour
{
    private static TopButtonUI instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
