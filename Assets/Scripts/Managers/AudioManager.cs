using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder.MeshOperations;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager instance => _instance;

    // Sources
    private bool _usingTrackA;
    [SerializeField]
    private AudioSource _musicTrackA;
    [SerializeField]
    private AudioSource _musicTrackB;

    // Tracks
    [SerializeField]
    private SOMusicTrack _menuMusic;
    [SerializeField]
    private SOMusicTrack _levelMusic;
    [SerializeField]
    private SOMusicTrack _bossMusic;

    // Mixers
    [SerializeField]
    private AudioMixer _musicVolume;
    [SerializeField]
    private AudioMixer _sfxVolume;

    private void Start()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        StartBackgroundMusic();
        SetMusicVolume();
        SetSFXVolume();
        SettingsManager.instance._onMusicSliderChange.AddListener(SetMusicVolume);
        SettingsManager.instance._onSFXSliderChange.AddListener(SetSFXVolume);
    }

    private SOMusicTrack GetCurrentMusicTrack()
    {
        if (EnemyManager.instance.inBossRoom)
            return _bossMusic;
        else if (GameManager.instance.buildIndex != 0)
            return _levelMusic;
        else
            return _menuMusic;
    }

    public void StartBackgroundMusic()
    {
        _usingTrackA = !_usingTrackA;
        StartCoroutine(DoBackgroundMusic(_usingTrackA ? _musicTrackA : _musicTrackB, GetCurrentMusicTrack()));
    }

    private IEnumerator DoBackgroundMusic(AudioSource source, SOMusicTrack track)
    {
        Debug.Log("BG music starting");
        WaitForEndOfFrame wait = new();
        // Set Up
        bool trackType = _usingTrackA;
        source.clip = track.introClip != null ? track.introClip : track.mainClip;
        source.loop = true;
        source.volume = 0;
        source.Play();
        if(track.introClip != null)
            source.PlayScheduled(AudioSettings.dspTime + source.clip.length);

        // Fade In
        double startTime = AudioSettings.dspTime;
        float inverseTotalTime = 1 / track.fadeInTime;
        while (AudioSettings.dspTime < startTime + track.fadeInTime)
        {
            source.volume = track.fadeInLevels.Evaluate((float)(AudioSettings.dspTime - startTime) * inverseTotalTime);
            yield return wait;
        }

        // Main
        while (trackType == _usingTrackA)
        {
            yield return wait;
        }


        if (track.outroClip != null)
        {
            source.clip = track.outroClip;
            source.PlayScheduled(AudioSettings.dspTime + source.clip.length);
        }

        // Fade Out
        startTime = AudioSettings.dspTime;
        inverseTotalTime = 1 / track.fadeOutTime;
        while (AudioSettings.dspTime < startTime + track.fadeOutTime)
        {
            source.volume = track.fadeOutLevels.Evaluate((float)(AudioSettings.dspTime - startTime) * inverseTotalTime);
            yield return wait;
        }
        source.Stop();
        source.volume = 0;
        source.loop = false;
    }


    #region Volume
    private void SetMusicVolume()
    {
        _musicVolume.SetFloat("MusicVolume", Mathf.Log10(SettingsManager.instance.GetMusicVolume()) * 30);
    }

    private void SetSFXVolume()
    {
        _sfxVolume.SetFloat("sfxVolume", Mathf.Log10(SettingsManager.instance.GetSFXVolume()) * 30);
    }
    #endregion
}
