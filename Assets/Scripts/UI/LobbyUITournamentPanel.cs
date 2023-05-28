using TMPro;
using UnityEngine;

public class LobbyUITournamentPanel : MonoBehaviour
{
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private GameObject _tournamentPanel;
    [SerializeField] private TextMeshProUGUI _tournamentNameText;
    [SerializeField] private TextMeshProUGUI _requiredText;
    [SerializeField] private Transform _tournamentLevelPrefab;
    [SerializeField] private Transform _tournamentLevelsContainer;
    [SerializeField] private GameObject _trainingPanel;
    [SerializeField] private Tournament _tournament;
    
    private void Awake()
    {
        InteractableTournament.InteractedTournament += InteractableTournamentOnInteractedTournament;
    }

    private void OnDestroy()
    {
        InteractableTournament.InteractedTournament -= InteractableTournamentOnInteractedTournament;
    }

    private void InteractableTournamentOnInteractedTournament(Tournament tournament)
    {
        if(_lobbyUI.GetIsOccupied()) { return; }
        
        
        _lobbyUI.SetOccupied(true);
        
        _tournament = tournament;
        _tournamentPanel.SetActive(true);
        _tournamentNameText.text = tournament.tournamentName;
        foreach (Transform g in _tournamentLevelsContainer.transform)
        {
            Destroy(g.gameObject);
        }

        int tournamentLevel = 0;
        foreach (var level in tournament.levels)
        {
            var go = Instantiate(_tournamentLevelPrefab, _tournamentLevelsContainer);
            if (go.TryGetComponent<LobbyUITournamentLevelItem>(out var lobbyUITournamentLevelItem))
            {
                lobbyUITournamentLevelItem.Init(tournament, tournamentLevel);
            }
            tournamentLevel++;
        }

        if (!tournament.CanParticipate(PlayerStats.instance.playerStatsData))
        {
            _requiredText.gameObject.SetActive(true);
            _requiredText.text = $"Bы имеете недостаточно репутации. Tребуется {tournament.abilitiesRequired.Reputation.baseValue} очков";
        }
        else
        {
            _requiredText.gameObject.SetActive(false);
        }
        
        _trainingPanel.SetActive(tournament.training);

    }

    void Start()
    {
        _tournamentPanel.SetActive(false);
    }

    public void CloseClicked()
    {
        _tournamentPanel.SetActive(false);
        _lobbyUI.SetOccupied(false);
    }

    public void StartTraining()
    {
        TournamentStaticData.StartLevel(_tournament.sceneName, null, 0,
            PlayerStats.instance.gameObject.transform.position);
        /*
        TournamentStaticData.TournamentInProgress = null;
        TournamentStaticData.TournamentLevelIdInProgress = 0;
        SceneManager.LoadScene(_tournament.sceneName);
        */

    }
}
