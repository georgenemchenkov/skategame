using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUITournamentLevelItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private GameObject _checkMarkGameObject;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private TextMeshProUGUI _startButtonText;

    [SerializeField]
    private Tournament _tournament;
    [SerializeField]
    private int _levelId;
    public void Init(Tournament tournament, int levelId)
    {
        _tournament = tournament;
        _levelId = levelId;
        
        _levelNameText.text = (levelId + 1)+ "-й " + "этап";
        bool hasCompleted = PlayerStats.instance.HasCompletedTournament(tournament.GetId(levelId));
        _checkMarkGameObject.SetActive(hasCompleted);
        if (hasCompleted)
        {
            _startButtonText.text = "Играть заново";
        }


        if(!tournament.CanParticipate(PlayerStats.instance.playerStatsData) || !tournament.CanParticipateLevel(PlayerStats.instance.playerStatsData,  levelId))
        {
            _startButton.SetActive(false);
        }
    }

    public void StartClicked()
    {
        TournamentStaticData.StartLevel(_tournament.sceneName, _tournament, _levelId, PlayerStats.instance.gameObject.transform.position);
        /*
        TournamentStaticData.TournamentInProgress = _tournament;
        TournamentStaticData.TournamentLevelIdInProgress = _levelId;
        SceneManager.LoadScene(_tournament.sceneName);
        */
    }
}
