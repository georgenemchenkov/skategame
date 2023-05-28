using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null; // Экземпляр объекта
    public PlayerStatsData playerStatsData { get; private set; }
    public static event Action<PlayerStatsData> PlayerStatsChanged;


    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start()
    {
        LoadPlayerStats();
    }

    public void SavePlayerStats()
    {
        SaveSystem.SavePlayerStats(this);
    }

    public void LoadPlayerStats()
    {
        PlayerStatsData data = SaveSystem.LoadPlayerStats();
        if (data == null)
        {
            playerStatsData = GetResettedPlayerStatsData();
            SavePlayerStats();
            data = SaveSystem.LoadPlayerStats();
        }

        playerStatsData = data;
        playerStatsData.Abilities.ClearModifiers();
        PlayerStatsChanged?.Invoke(playerStatsData);
        GetInventory().InitLoadInvoke();
    }

    private PlayerStatsData GetResettedPlayerStatsData()
    {
        return new PlayerStatsData(null)
        {
            Inventory = new PlayerInventory(),
            Abilities = new PlayerAbilities() { Agility = new Ability() { baseValue = 1}, Reputation  = new Ability() { baseValue = 1}, Strength  = new Ability() { baseValue = 1}}
        };
    }

    public void ResetPlayerStats()
    {
        playerStatsData = GetResettedPlayerStatsData();
        SavePlayerStats();
    }

    public void UpdateAbilities(PlayerAbilities playerAbilities)
    {
        playerStatsData.UpdateAbilities(playerAbilities);
        SavePlayerStats();
        PlayerStatsChanged?.Invoke(playerStatsData);
    }

    public void AddModifiers(PlayerAbilities modifierAbilities)
    {
        playerStatsData.AddModifiers(modifierAbilities);
        PlayerStatsChanged?.Invoke(playerStatsData);
    }

    public void RemoveModifiers(PlayerAbilities modifierAbilities)
    {
        playerStatsData.RemoveModifiers(modifierAbilities);
        PlayerStatsChanged?.Invoke(playerStatsData);
    }

    public void UpdateMoney(int amount)
    {
        playerStatsData.UpdateMoney(amount);
        SavePlayerStats();
        PlayerStatsChanged?.Invoke(playerStatsData);
    }
    
    public bool HasCompletedTournament(string name)
    {
        return playerStatsData.HasCompletedTournament(name);
    }

    public void UpdateCompletedTournament(string name, bool completed)
    {
        playerStatsData.UpdateCompletedTournament(name, completed);
        SavePlayerStats();
        PlayerStatsChanged?.Invoke(playerStatsData);
    }

    public PlayerInventory GetInventory()
    {
        return playerStatsData.Inventory;
    }
}
