using UnityEngine;

[System.Serializable]
public struct AbilityStats
{
    [SerializeField]
    private GameObject _prefab;
    public GameObject prefab => _prefab;
    [SerializeField]
    private float _damage;
    public float damage => _damage;
    [SerializeField]
    private float _cooldown;
    public float cooldown => _cooldown;
    [SerializeField]
    private float _lifetime;
    public float lifetime => _lifetime;
    [SerializeField]
    private float _speed;
    public float speed => _speed;


    public static AbilityStats operator +(AbilityStats s1, AbilityStats s2)
    {
        AbilityStats stats = new();

        stats._prefab = s1._prefab;
        stats._damage = s1._damage + s2._damage;
        stats._cooldown = s1._cooldown + s2._cooldown;
        stats._lifetime = s1._lifetime + s2._lifetime;
        stats._speed = s1._speed + s2._speed;

        return stats;
    }
}
