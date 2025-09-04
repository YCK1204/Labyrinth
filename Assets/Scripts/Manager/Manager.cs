using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance;
    static Manager Instance
    {
        get 
        { 
            if (_instance == null)
            {
                var instance = FindObjectOfType<Manager>();
                if (instance == null)
                {
                    instance = new GameObject("Manager").AddComponent<Manager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return _instance; 
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    UIManager _ui;
    public static UIManager UI { get { return Instance._ui; } set { Instance._ui = value; DontDestroyOnLoad(value.gameObject); } }
    AudioManager _audio;
    public static AudioManager Audio { get { return Instance._audio; } set { Instance._audio = value; DontDestroyOnLoad(value.gameObject); } }
}
