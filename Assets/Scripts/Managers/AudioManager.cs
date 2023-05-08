using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager instance => _instance;

    [SerializeField]
    private AudioSource _musicAudioSource;
    [SerializeField]
    private AudioClip _backgroundMusic;
    [SerializeField]
    private AudioMixer _musicVolume;
    [SerializeField]
    private AudioMixer _sfxVolume;

    private bool _bgMusicPlaying;

    private void Start()
    {
        if(_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        StartBackgroundMusic();
        SettingsManager.instance._onMusicSliderChange.AddListener(SetMusicVolume);
        SettingsManager.instance._onSFXSliderChange.AddListener(SetSFXVolume);
        SetMusicVolume();
        SetSFXVolume();
    }

    public void StartBackgroundMusic()
    {
        _bgMusicPlaying = false;
        StartCoroutine(DoBackgroundMusic());
    }

    private IEnumerator DoBackgroundMusic()
    {
        _bgMusicPlaying = true;
        _musicAudioSource.loop = true;
        _musicAudioSource.clip = GetBackgroundMusic();
        WaitForEndOfFrame wait = new();
        _musicAudioSource.Play();
        while (_bgMusicPlaying)
        {
            yield return wait;
        }
        _musicAudioSource.Stop();
        _musicAudioSource.loop = false;
    }


    // For later logic based off the current game state
    private AudioClip GetBackgroundMusic()
    {
        return _backgroundMusic;
    }

    private void SetMusicVolume()
    {
        _musicVolume.SetFloat("MusicVolume", Mathf.Log10(SettingsManager.instance.GetMusicVolume()) * 30);
    }

    private void SetSFXVolume()
    {
        _sfxVolume.SetFloat("sfxVolume", Mathf.Log10(SettingsManager.instance.GetSFXVolume()) * 30);
    }


}
