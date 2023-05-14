using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SOItem _item;
    private int _cost;
    [SerializeField]
    private List<GameObject> _animals = new();
    [SerializeField]
    private float _lookTime;
    [SerializeField]
    private ParticleSystem _dismissalAnimation;
    [SerializeField]
    private float _animationLength;
    private Vector3 _playerPos;
    private Vector3 _lookDirection;
    private Quaternion _oldRotation;
    private Quaternion _newRotation;
    private GameObject _animal;

    private void Start()
    {
        SetModel();
        _item = LootManager.instance.GetItem();
        _cost = LootManager.instance.GetChestCost();
        transform.position += GetComponent<Collider>().bounds.extents.y * Vector3.up;
        StartCoroutine(LookAtPlayer());
    }

    public bool Interact()
    {
        if (!CanInteract()) return false;

        StartCoroutine(SummonItem());
       

        return true;
    }

    IEnumerator SummonItem()
    {
        _dismissalAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(_animationLength);
        Destroy(gameObject);
        LootItem loot = Instantiate(LootManager.instance.GetPrefab(_item.rarity), transform.position, transform.rotation).GetComponent<LootItem>();
        loot.item = _item;
        GameManager.instance.player.inventory.Spend(_cost);

    }

    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            _playerPos = GameManager.instance.player.transform.position;

            _lookDirection = _playerPos - _animal.transform.position;

            StartCoroutine(Turning());

            yield return new WaitForSeconds(Random.Range(5, 15));

        }
    }

    IEnumerator Turning()
    {
        _oldRotation = _animal.transform.rotation;
        _newRotation = Quaternion.LookRotation(_lookDirection);
        float startTime = Time.time;
        while (Time.time < startTime + _lookTime)
        {
            _animal.transform.rotation = Quaternion.Lerp(_oldRotation, _newRotation, (Time.time - startTime) / _lookTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public bool CanInteract() => GameManager.instance.player.inventory.currency > _cost;

    public int GetCost() => _cost;

    private void SetModel()
    {
        int index = Random.Range(0, _animals.Count - 1);
        _animal = Instantiate(_animals[index], transform);
        _animal.transform.Translate(new Vector3(0, -0.25f, 0));
        this.transform.localScale = new Vector3(2, 2, 2);
    }
}
