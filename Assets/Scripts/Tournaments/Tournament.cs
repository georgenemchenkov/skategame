using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct TournamentLevel
{
    public int pointsToComplete;
    public int reputationRewarded;
    public int moneyRewarded;
}

[CreateAssetMenu(fileName = "TournamentName", menuName = "Tournaments/New Tournament", order = 1)]
public class Tournament : ScriptableObject
{
    public string tournamentName;
    [FormerlySerializedAs("levelPointsToComplete")] [Header("Сколько уровней у турнира и необходимое количество очков на каждый уровень")]
    public TournamentLevel[] levels;
    public PlayerAbilities abilitiesRequired;
    public string sceneName;
    public bool training;

    public string GetId(int level)
    {
        return name + "." + level;
    }

    public bool CanParticipate(PlayerStatsData data)
    {
        return data.Abilities.Reputation.GetValue() >= abilitiesRequired.Reputation.baseValue;
    }

    public bool CanParticipateLevel(PlayerStatsData data, int levelId)
    {
        if (levelId > 0 && !data.HasCompletedTournament(GetId(levelId - 1)))
        {
            return false;
        }

        return true;
    }
}
