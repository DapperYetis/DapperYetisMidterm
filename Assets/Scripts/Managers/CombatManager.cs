using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    private static CombatManager _instance;
    public static CombatManager instance => _instance;

    // Instance variables
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private int _attackBudgetMax;
    private int _attackBudget;
    private bool _isRunning;
    private Queue<Action> _attacks;

    private void Start()
    {
        if(_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        _attackBudget = _attackBudgetMax;
        _attacks = new();
    }

    public void QueueAttack(Action attack)
    {
        _attacks.Enqueue(attack);
        if (!_isRunning)
            StartCoroutine(RunDequeue());
    }

    private IEnumerator RunDequeue()
    {
        _isRunning = true;
        if (_attacks.Count > 0 && _attackBudget > 0)
        {
            --_attackBudget;
            (_attacks.Dequeue()).Invoke();

            StartCoroutine(RunDequeue());
            yield return new WaitForSeconds(_attackTime);
            ++_attackBudget;
        }
        else
        {
            _isRunning = false;
            yield return null;
        }
    }
}
