using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class DebugPlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerTricks _playerTricks;
    [SerializeField] private Item testItem;

    
    public void IncreaseStrength()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Strength = new Ability(){ baseValue = 1 }
        });
    }
    
    public void DecreaseStrength()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Strength = new Ability(){ baseValue = -1 }
        });
    }
    
    public void IncreaseAgility()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Agility = new Ability(){ baseValue = 1 }
        });
    }
    
    public void DecreaseAgility()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Agility = new Ability(){ baseValue = -1 }
        });
    }
    
    public void IncreaseImprovisation()
    {
        _playerTricks.AddImprovisationPoints(1);
    }
    
    public void DecreaseImprovisation()
    {
        _playerTricks.RemoveImprovisationPoints(1);
    }
    
    public void IncreaseReputation()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Reputation = new Ability(){ baseValue = 1 }
        });
    }
    
    public void DecreaseReputation()
    {
        _playerStats.UpdateAbilities(new PlayerAbilities()
        {
            Reputation = new Ability(){ baseValue = -1 }
        });
    }

    public void ResetData()
    {
        _playerStats.ResetPlayerStats();
    }

    
    private void Start()
    {
        //_playerStats.playerStatsData.Inventory.AddItem(testItem.name);
    }
}
