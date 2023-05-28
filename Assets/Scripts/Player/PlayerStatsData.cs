using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[System.Serializable]
public class PlayerStatsData
{
    public PlayerAbilities Abilities;
    public PlayerInventory Inventory;
    public List<string> CompletedTournaments = new List<string>();
    public int Money;

    public PlayerStatsData(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            Abilities = playerStats.playerStatsData.Abilities;
            Inventory = playerStats.playerStatsData.Inventory;
            Money = playerStats.playerStatsData.Money;
            CompletedTournaments = playerStats.playerStatsData.CompletedTournaments;
        }
    }

    public void UpdateAbilities(PlayerAbilities playerAbilities)
    {
        Abilities.UpdateBaseValues(playerAbilities);
    }
    
    public void AddModifiers(PlayerAbilities modifierAbilities)
    {
        Abilities.ApplyModifiers(modifierAbilities);
    }

    public void RemoveModifiers(PlayerAbilities modifierAbilities)
    {
        Abilities.RemoveModifiers(modifierAbilities);
    }

    public void UpdateMoney(int amount)
    {
        Money += amount;
    }

    public bool HasCompletedTournament(string name)
    {
        if (CompletedTournaments == null)
        {
            CompletedTournaments = new List<string>();
        }
        return CompletedTournaments.Contains(name);
    }

    public void UpdateCompletedTournament(string name, bool completed)
    {
        if (CompletedTournaments == null)
        {
            CompletedTournaments = new List<string>();
        }
        if (completed)
        {
            CompletedTournaments.Add(name);
        }
        else
        {
            CompletedTournaments.Remove(name);
        }
    }
}
