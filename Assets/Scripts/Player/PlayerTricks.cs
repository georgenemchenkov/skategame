using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerTricks : MonoBehaviour
{
    public static PlayerTricks instance = null;

    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Animator _animator;
    [SerializeField] private int ImprovisationPoints = 3;
    [SerializeField] private int improvisationPointsPrice = 1;
    [SerializeField] private float improvisationMultiplier = 1.3f;
    public List<Trick> Tricks = new List<Trick>();
    private Dictionary<Trick, float> _improviseTrickChances = new Dictionary<Trick, float>();
    public static event Action<Trick, float> PlayerSuccessfullyPerformedTrick;
    public static event Action<Trick> PlayerFailedTrick;
    public static event Action PlayerTrickDataChanged;
    private bool _trickInProgress;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public float GetChance(Trick trick)
    {
        if (_improviseTrickChances.ContainsKey(trick))
        {
            Debug.Log("contains custom chacnce so " + _improviseTrickChances.GetValueOrDefault(trick));
            return _improviseTrickChances.GetValueOrDefault(trick);
        }
        
        return Mathf.Clamp((float) _playerStats.playerStatsData.Abilities.Agility.GetValue() /
               trick.abilitiesRequired.Agility.baseValue, 0,1);
    }

    public void DoTrick(Trick trick)
    {
        if (!CanPerform(trick))
        {
            return; 
        }
        StartCoroutine(DoTrickCoroutine(trick));
    }

    IEnumerator DoTrickCoroutine(Trick trick)
    {
        _trickInProgress = true;
        PlayerTrickDataChanged?.Invoke();

        var randValue = Random.Range(0f, 1f);
        bool success = randValue <= GetChance(trick);
        
        if (success)
        {
            // Successfully performed trick
            _animator.SetTrigger(trick.animatorTrigger);
        }
        else
        {
            _animator.SetTrigger(trick.animatorFailedTrigger);
        }
        
        yield return new WaitForSeconds(trick.performTime);
        
        if (success)
        {
            PlayerSuccessfullyPerformedTrick?.Invoke(trick, GetChance(trick));
        }
        else
        {
            PlayerFailedTrick?.Invoke(trick);
        }

        _trickInProgress = false;
        RemoveImprovisation(trick);
        PlayerTrickDataChanged?.Invoke();
    }

    public bool CanPerform(Trick trick)
    {
        if (_trickInProgress)
        {
            return false;
        }
        
        return _playerStats.playerStatsData.Abilities.Strength.GetValue() >= trick.abilitiesRequired.Strength.baseValue;
    }

    public bool CanImprovise(Trick trick)
    {
        if (GetChance(trick) >= 1)
            return false;
        
        return ImprovisationPoints >= improvisationPointsPrice;
    }

    public bool IsImprovising(Trick trick)
    {
        return _improviseTrickChances.ContainsKey(trick);
    }

    public void ApplyImprovisation(Trick trick)
    {
        _improviseTrickChances.Add(trick, Mathf.Clamp(GetChance(trick) * improvisationMultiplier,0,1));
        RemoveImprovisationPoints(improvisationPointsPrice);
    }

    public void CancelImprovisation(Trick trick)
    {
        if (_improviseTrickChances.ContainsKey(trick))
        {
            RemoveImprovisation(trick);
            AddImprovisationPoints(improvisationPointsPrice);
        }
    }

    private void RemoveImprovisation(Trick trick)
    {
        _improviseTrickChances.Remove(trick);
    }

    public void AddImprovisationPoints(int points)
    {
        ImprovisationPoints += points;
        PlayerTrickDataChanged?.Invoke();
    }

    public void RemoveImprovisationPoints(int points)
    {
        ImprovisationPoints -= points;
        PlayerTrickDataChanged?.Invoke();
    }

    public int GetImprovisationPoints()
    {
        return ImprovisationPoints;
    }
}
