﻿using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuffName", menuName = "Stats/Buff")]
public class SOBuff : ScriptableObject
{
    [Header("---General---")]
    [SerializeField]
    private string _buffName;
    public string buffName => _buffName;
    [SerializeField]
    private string _description;
    public string description => _description;
    [SerializeField]
    private Sprite _icon;
    public Sprite icon => _icon;

    [Header("---Timing---")]
    [SerializeField]
    private BuffTiming _timing;
    public BuffTiming timing => _timing;
    [SerializeField]
    private float _buffLength;
    public float buffLength => _buffLength;
    [SerializeField]
    private BuffRemoveType _removeType;
    public BuffRemoveType removeType => _removeType;

    [Header("---Modifications---")]
    [SerializeField]
    private PlayerStats _generalMods;
    public PlayerStats generalMods => _generalMods;
    [SerializeField]
    private AbilityStats _abilityMods;
    public AbilityStats abilityMods => _abilityMods;

    [Header("---Prefabs---")]
    [SerializeField, FormerlySerializedAs("_removeEffectPrefab")]
    private GameObject _effectPrefab;
    public GameObject effectPrefab => _effectPrefab;

    [Header("---Audio---")]
    [Range(0, 1)]
    [SerializeField]
    private float _audioVolume;
    public float audioVolume => _audioVolume;
    [SerializeField]
    private AudioClip[] _audioClips;
    public AudioClip[] audioClips => _audioClips;
}