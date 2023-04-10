using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("----- Player HUD -----")]
    public GameObject player;

    [Header("----- Settings -----")]
    [SerializeField] Slider _SoundSlider;
    [SerializeField] AudioMixer _MasterMixer;

    [Header("----- UI -----")]
    [SerializeField] GameObject _ActiveMenu;
    [SerializeField] GameObject _PauseMenu;
    [SerializeField] GameObject _WinMenu;
    [SerializeField] GameObject _LoseMenu;
    [SerializeField] GameObject _SettingsMenu;
    [SerializeField] Image _HPbar;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVolume(float volume)
    {
        if (volume < 1)
        {
            volume = .001f;
        }

        RefreshSlider(volume);
        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
        _MasterMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);

    }

    public void SetVolumeFromSlider()
    {
        SetVolume(_SoundSlider.value);
    }


    public void RefreshSlider(float volume)
    {
        _SoundSlider.value = volume;
    }


}
