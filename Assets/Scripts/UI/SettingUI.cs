using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Button closeButton;

    [SerializeField] private AudioMixer audioMixer;

    private const string MasterVolume = "MasterVolume";
    private const string BGMVolume = "BGMVolume";
    private const string SFXVolume = "SFXVolume";

    private void Awake()
    {
        if (!closeButton) closeButton = transform.Find("CloseButton")?.GetComponent<Button>();
        if (!masterVolumeSlider) masterVolumeSlider = transform.Find("MasterVolume/Slider")?.GetComponent<Slider>();
        if (!BGMVolumeSlider) BGMVolumeSlider = transform.Find("BGM/Slider")?.GetComponent<Slider>();
        if (!SFXVolumeSlider) SFXVolumeSlider = transform.Find("SFX/Slider")?.GetComponent<Slider>();

        if (closeButton) closeButton.onClick.AddListener(OnCloseButtonClicked);

        if (masterVolumeSlider) masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        if (BGMVolumeSlider) BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        if (SFXVolumeSlider) SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void OnEnable()
    {
        //SetSliderValueCurrentVolume();
        Manager.Audio.SetVolume(AudioType.MASTER, 1f);
        Manager.Audio.SetVolume(AudioType.BGM, 1f);
        Manager.Audio.SetVolume(AudioType.EFFECT, 1f);
    }

    private void OnCloseButtonClicked()
    {
        Manager.UI.HideSettingUI();
    }

    private void SetMasterVolume(float volume)
    {
        float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        Manager.Audio.SetVolume(AudioType.MASTER, volume);
        //audioMixer.SetFloat(MasterVolume, dbVolume);
    }

    private void SetBGMVolume(float volume)
    {
        float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        Manager.Audio.SetVolume(AudioType.BGM, volume);
        //audioMixer.SetFloat(BGMVolume, dbVolume);
    }

    private void SetSFXVolume(float volume)
    {
        float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        Manager.Audio.SetVolume(AudioType.EFFECT, volume);
        //audioMixer.SetFloat(SFXVolume, dbVolume);
    }

    //private void SetSliderValueCurrentVolume()
    //{
    //    float masterVolume;
    //    float bgmVolume;
    //    float sfxVolume;
        
    //    audioMixer.GetFloat(MasterVolume, out masterVolume);
    //    audioMixer.GetFloat(BGMVolume, out bgmVolume);
    //    audioMixer.GetFloat(SFXVolume, out sfxVolume);

    //    masterVolumeSlider.value = Mathf.Pow(10, masterVolume / 20);
    //    BGMVolumeSlider.value = Mathf.Pow(10, bgmVolume / 20);
    //    SFXVolumeSlider.value = Mathf.Pow(10, sfxVolume / 20);
    //}
}
