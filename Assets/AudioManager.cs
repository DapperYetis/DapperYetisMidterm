using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager instance => _instance;

    [SerializeField]
    private AudioSource _musicAudioSource;
    [SerializeField]
    private AudioClip _backgroundMusic;

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
    }

    public void StartBackgroundMusic()
    {
        _bgMusicPlaying = false;
        StartCoroutine(DoBackgroundMusic());
    }

    private IEnumerator DoBackgroundMusic()
    {
        _bgMusicPlaying = true;
        WaitForEndOfFrame wait = new();
        _musicAudioSource.PlayOneShot(_backgroundMusic);
        while (_bgMusicPlaying)
        {
            yield return wait;
        }
        _musicAudioSource.Stop();
    }
}
