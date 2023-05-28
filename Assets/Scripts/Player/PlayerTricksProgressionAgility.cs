using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTricksProgressionAgility : MonoBehaviour
{
    [SerializeField] private PlayerTricks _playerTricks;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private int trickStreakRequired = 3;

    private Trick lastPerformedTrick;
    private int trickStreak = 0;

    private void Awake()
    {
        PlayerTricks.PlayerSuccessfullyPerformedTrick += PlayerTricksOnPlayerSuccessfullyPerformedTrick;
        PlayerTricks.PlayerFailedTrick += PlayerTricksOnPlayerFailedTrick;
    }

    private void OnDestroy()
    {
        PlayerTricks.PlayerSuccessfullyPerformedTrick -= PlayerTricksOnPlayerSuccessfullyPerformedTrick;
        PlayerTricks.PlayerFailedTrick -= PlayerTricksOnPlayerFailedTrick;
    }

    private void PlayerTricksOnPlayerFailedTrick(Trick trick)
    {
        trickStreak = 0;
        lastPerformedTrick = null;
    }

    private void PlayerTricksOnPlayerSuccessfullyPerformedTrick(Trick trick, float chance)
    {
        var trickChance = _playerTricks.GetChance(trick);

        trickStreak++;
        if (lastPerformedTrick == trick && trickChance < 1f)
        {
            if (trickStreak >= trickStreakRequired)
            {
                var updatedAgility = new PlayerAbilities()
                {
                    Agility = new Ability() { baseValue = 1 }
                };
                _playerStats.UpdateAbilities(updatedAgility);
                trickStreak = 0;
            }
        }
        else
        {
            trickStreak = 0;
            trickStreak++;
        }
        lastPerformedTrick = trick;
    }

    public float GetStreakPercentage()
    {
        return (float)trickStreak / trickStreakRequired;
    }
}
