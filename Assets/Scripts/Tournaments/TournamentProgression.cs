using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public struct TournamentResult
{
    public bool Completed;
    public int ReputationRewarded;
    public int ReputationRewardedExtra;
    public int ReputationRewardedTotal;
    public int MoneyRewarded;
    public int MoneyRewardedExtra;
    public int MoneyRewardedTotal;
}

public class TournamentProgression : MonoBehaviour
{
    public static TournamentProgression instance = null;
    [SerializeField] private Tournament _tournament;
    [SerializeField] private int _tournamentLevelId;
    [SerializeField] private TournamentLevel _tournamentLevel;
    [SerializeField] private int _pointsToComplete;
    [SerializeField] private int _pointsToCompleteMax;
    [SerializeField] private float _pointsToCompleteMaxMultipier = 1.2f;
    [SerializeField] private int _timeToComplete = 1;
    [SerializeField] private float _timerCurrent;
    [SerializeField] private int _timerCurrentInt;
    private bool _completed;
    private bool _ended;
    private int _currentPoints = 0;
    public static event Action<int> TimerTick;
    public static event Action TournamentCompleted;
    public static event Action<TournamentResult> TournamentEnded;

    private void Awake()
    {
        Init();
        PlayerTricks.PlayerSuccessfullyPerformedTrick += PlayerTricksOnPlayerSuccessfullyPerformedTrick;
    }

    private void OnDestroy()
    {
        PlayerTricks.PlayerSuccessfullyPerformedTrick -= PlayerTricksOnPlayerSuccessfullyPerformedTrick;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void PlayerTricksOnPlayerSuccessfullyPerformedTrick(Trick trick, float chance)
    {
        if(_ended || !_tournament) { return; }
        _currentPoints += trick.points;
        if(chance < 1)
        {        
            _currentPoints += trick.bonusPoints;
        }

        if (_currentPoints >= _pointsToCompleteMax)
        {
            _currentPoints = _pointsToCompleteMax;
            _completed = true;
            EndTournament();
        }

        if (_currentPoints >= _pointsToComplete && !_completed)
        {
            _completed = true;
            TournamentCompleted?.Invoke();
        }
    }

    private void Init()
    {
        if (TournamentStaticData.TournamentInProgress)
        {
            _tournament = TournamentStaticData.TournamentInProgress;
        }

        if (!_tournament) return;
        _tournamentLevelId = TournamentStaticData.TournamentLevelIdInProgress;
        _tournamentLevel = _tournament.levels[_tournamentLevelId];
        _pointsToComplete = _tournament.levels[_tournamentLevelId].pointsToComplete;
        _pointsToCompleteMax = (int)Math.Floor(_pointsToComplete * _pointsToCompleteMaxMultipier);
        _timerCurrent = _timeToComplete;
            
        if (instance == null) {
            instance = this;
        }
    }

    public int GetPointsToComplete()
    {
        return _pointsToComplete;
    }
    
    public int GetPointsToCompleteMax()
    {
        return _pointsToCompleteMax;
    }

    public int GetCurrentPoints()
    {
        return _currentPoints;
    }

    private void UpdateTimer()
    {
        if(_ended || !_tournament) { return; } 
        _timerCurrent -= Time.deltaTime;
        if ((int)_timerCurrent != _timerCurrentInt)
        {
            _timerCurrentInt = (int)_timerCurrent;
            TimerTick?.Invoke(_timerCurrentInt);   
        }
        
        if (_timerCurrent <= 0)
        {
            _timerCurrent = 0;
            EndTournament();            
        }
    }

    public void EndTournament()
    {
        _ended = true;
        int reputationRewarded = _tournamentLevel.reputationRewarded;
        int reputationRewardedExtra = 0;
        int moneyRewarded = _tournamentLevel.moneyRewarded;
        int moneyRewardedExtra = 0;
        if (_currentPoints > _pointsToComplete)
        {
            reputationRewardedExtra = (int)Math.Floor((float)_tournamentLevel.reputationRewarded * _currentPoints / _pointsToComplete) - reputationRewarded;
            moneyRewardedExtra = (int)Math.Floor((float)_tournamentLevel.moneyRewarded * _currentPoints / _pointsToComplete) - moneyRewarded;
        }

        int totalReputationRewarded = reputationRewarded + reputationRewardedExtra;
        int totalMoneyRewarded = moneyRewarded + moneyRewardedExtra;

        if (_completed)
        {
            PlayerStats.instance.UpdateAbilities(new PlayerAbilities()
            {
                Reputation = new Ability() {baseValue = totalReputationRewarded}
            });
            PlayerStats.instance.UpdateMoney(totalMoneyRewarded);
        }
        
        if (_completed && _tournament != null && !PlayerStats.instance.HasCompletedTournament(_tournament.GetId(_tournamentLevelId)))
        {
            PlayerStats.instance.UpdateCompletedTournament(_tournament.GetId(_tournamentLevelId), true);
        }
        
        TournamentEnded?.Invoke(new TournamentResult()
        {
            ReputationRewarded = reputationRewarded, ReputationRewardedExtra = reputationRewardedExtra, ReputationRewardedTotal = totalReputationRewarded,
            MoneyRewarded = moneyRewarded, MoneyRewardedExtra = moneyRewardedExtra, MoneyRewardedTotal = totalMoneyRewarded,
            Completed = _completed
        });
    }
}
