using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalExit : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected int _cost = 1000;
    [SerializeField]
    private GameObject _portalObject;
    [SerializeField]
    private int _nextSceneNum;

    private void Start()
    {
        _cost = (int)(_cost * EnemyManager.instance.scaleFactor);
    }

    public bool Interact()
    {
        _portalObject.SetActive(true);
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        GameManager.instance.player.movement.enabled = false;

        SceneManage.instance.LoadScene(_nextSceneNum);
    }

    public bool CanInteract()
    {
        return true;
    }

    public int GetCost() => _cost;
}
