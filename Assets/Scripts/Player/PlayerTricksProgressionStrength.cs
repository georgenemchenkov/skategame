using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTricksProgressionStrength : MonoBehaviour
{
    [SerializeField] private PlayerTricks _playerTricks;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private int streakCountRequired = 10;
    private int streakCount = 0;

    private void Awake()
    {
        PlayerTricks.PlayerSuccessfullyPerformedTrick += PlayerTricksOnPlayerSuccessfullyPerformedTrick;
    }

    private void OnDestroy()
    {
        PlayerTricks.PlayerSuccessfullyPerformedTrick -= PlayerTricksOnPlayerSuccessfullyPerformedTrick;
    }

    private void PlayerTricksOnPlayerSuccessfullyPerformedTrick(Trick trick, float chance)
    {
        var trickChance = _playerTricks.GetChance(trick);
        streakCount += (int)(10 * (1 - trickChance));
        if (streakCount >= streakCountRequired)
        {
            var updatedStrength = new PlayerAbilities()
            {
                Strength = new Ability() {baseValue = 1}
            };
            _playerStats.UpdateAbilities(updatedStrength);
            streakCount = 0;
        }
    }

    public float GetStreakPercentage()
    {
        return (float)streakCount / streakCountRequired;
    }
}
