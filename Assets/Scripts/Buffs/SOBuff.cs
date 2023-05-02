using UnityEngine;

[CreateAssetMenu(fileName = "BuffName", menuName = "Stats/Buff")]
public class SOBuff : ScriptableObject
{
    [SerializeField]
    private string _buffName;
    public string buffName => _buffName;
    [SerializeField]
    private string _description;
    public string description => _description;

    [SerializeField]
    private BuffTiming _timing;
    public BuffTiming timing => _timing;
    [SerializeField]
    private float _buffLength;
    public float buffLength => _buffLength;
    [SerializeField]
    private BuffRemoveType _removeType;
    public BuffRemoveType removeType => _removeType;

    [SerializeField]
    private GameObject _removeEffectPrefab;
    public GameObject removeEffectPrefab => _removeEffectPrefab;

    [Header("---Modifications---")]
    [SerializeField]
    private StatChangeType _changeType;
    public StatChangeType changeType => _changeType;
    [SerializeField]
    private PlayerStats _generalMods;
    public PlayerStats generalMods => _generalMods;
    [SerializeField]
    private AbilityStats _abilityMods;
    public AbilityStats abilityMods => _abilityMods;
}