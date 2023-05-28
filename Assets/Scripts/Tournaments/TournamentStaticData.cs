using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TournamentStaticData
{
    public static Tournament TournamentInProgress;
    public static int TournamentLevelIdInProgress;
    public static Vector3 LastPlayerPosition;

    public static void StartLevel(string _levelName, Tournament _tournament, int _levelId, Vector3 _playerPosition)
    {
        TournamentInProgress = _tournament;
        TournamentLevelIdInProgress = _levelId;
        LastPlayerPosition = _playerPosition;
        
        SceneManager.LoadScene(_levelName);
    }
}
