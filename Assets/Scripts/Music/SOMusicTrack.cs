using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MusicTrack", menuName = "Music/Track")]
public class SOMusicTrack : ScriptableObject
{
    [SerializeField, Range(0, 1)]
    private float _trackVolume = 0.5f;
    public float trackVolume => _trackVolume;
    
    [Header("---Intro---")]
    [SerializeField]
    private float _fadeInTime = 1f;
    public float fadeInTime => _fadeInTime;
    [SerializeField]
    private AnimationCurve _fadeInLevels;
    public AnimationCurve fadeInLevels => _fadeInLevels;
    [SerializeField]
    private AudioClip _introClip;
    public AudioClip introClip => _introClip;

    [Header("---Body---")]
    [SerializeField]
    private AudioClip _mainClip;
    public AudioClip mainClip => _mainClip;
    [SerializeField]
    private bool _isLoopable;
    public bool isLoopable => _isLoopable;

    [Header("---Outro---")]
    [SerializeField]
    private float _fadeOutTime = 1f;
    public float fadeOutTime => _fadeOutTime;
    [SerializeField]
    private AnimationCurve _fadeOutLevels;
    public AnimationCurve fadeOutLevels => _fadeOutLevels;
    [SerializeField]
    private AudioClip _outroClip;
    public AudioClip outroClip => _outroClip;
}