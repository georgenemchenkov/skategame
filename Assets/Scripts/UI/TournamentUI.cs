using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class TournamentUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _tournamentInProgressScreen;
        [SerializeField] private GameObject _tournamentEndScreen;

        [SerializeField] private TextMeshProUGUI _statsStrengthText;
        [SerializeField] private TextMeshProUGUI _statsAgilityText;
        [SerializeField] private TextMeshProUGUI _statsImprovisationText;
        [SerializeField] private TextMeshProUGUI _statsReputationText;
        [SerializeField] private Slider _statsStrengthSlider;
        [SerializeField] private Slider _statsAgilitySlider;
        [SerializeField] private Transform _tricksContainer;
        [SerializeField] private GameObject _quitButton;
        [SerializeField] private GameObject _finishButton;
        
        [SerializeField] private Transform _tournamentProgressUI;
        [SerializeField] private TextMeshProUGUI _tournamentProgressText;
        [SerializeField] private TextMeshProUGUI _tournamentProgressTimerText;
        [SerializeField] private Slider _tournamentProgressSlider;
        [SerializeField] private Slider _tournamentProgressSliderToFinish;
        [SerializeField] private Image _tournamentProgressSliderFill;
        [SerializeField] private Color _tournamentProgressSliderFillColorCompleted = Color.green;
        [SerializeField] private GameObject _tournamentCompletedUI;
        [SerializeField] private GameObject _tournamentLostUI;
        [SerializeField] private TextMeshProUGUI _tournamenCompletedAbilityRewardsText;
        [SerializeField] private TextMeshProUGUI _tournamenCompletedMoneyRewardsText;
        
        [SerializeField] private GameObject _trickButtonPrefab;
        [SerializeField] private GameObject _failedTrickPrefab;
    
        private PlayerStats _playerStats;
        private PlayerTricks _playerTricks;
        private PlayerTricksProgressionStrength _playerTricksProgressionStrength;
        private PlayerTricksProgressionAgility _playerTricksProgressionAgility;

        private void Awake()
        {
            PlayerStats.PlayerStatsChanged += PlayerStatsOnPlayerStatsChanged;
            PlayerTricks.PlayerTrickDataChanged += PlayerTricksOnPlayerTrickDataChanged;
            PlayerTricks.PlayerFailedTrick += PlayerTricksOnPlayerFailedTrick;
            TournamentProgression.TournamentCompleted += TournamentProgressionOnTournamentCompleted;
            TournamentProgression.TimerTick += TournamentProgressionOnTimerTick;
            TournamentProgression.TournamentEnded += TournamentProgressionOnTournamentEnded;
            _playerStats = PlayerStats.instance;
            _playerTricks = _playerStats.gameObject.GetComponent<PlayerTricks>();
            _playerTricksProgressionStrength = _playerStats.gameObject.GetComponent<PlayerTricksProgressionStrength>();
            _playerTricksProgressionAgility = _playerStats.gameObject.GetComponent<PlayerTricksProgressionAgility>();
        }

        private void OnDestroy()
        {
            PlayerStats.PlayerStatsChanged -= PlayerStatsOnPlayerStatsChanged;
            PlayerTricks.PlayerTrickDataChanged -= PlayerTricksOnPlayerTrickDataChanged;
            PlayerTricks.PlayerFailedTrick -= PlayerTricksOnPlayerFailedTrick;
            TournamentProgression.TournamentCompleted -= TournamentProgressionOnTournamentCompleted;
            TournamentProgression.TimerTick -= TournamentProgressionOnTimerTick;
            TournamentProgression.TournamentEnded -= TournamentProgressionOnTournamentEnded;
        }

        private void PlayerTricksOnPlayerTrickDataChanged()
        {
            RefreshUI();
        }

        private void PlayerStatsOnPlayerStatsChanged(PlayerStatsData obj)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            LoadPlayerStats();
            LoadPlayerTricks();
            LoadTournamentProgress();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void LoadPlayerStats()
        {
            _statsStrengthText.text = "Сила: " + _playerStats.playerStatsData.Abilities.Strength.GetValue() + "\n";
            _statsAgilityText.text = "Ловкость: " + _playerStats.playerStatsData.Abilities.Agility.GetValue() + "\n";
            _statsImprovisationText.text = "Импровизация: " + _playerTricks.GetImprovisationPoints() + "\n";
            _statsReputationText.text = "Репутация: " + _playerStats.playerStatsData.Abilities.Reputation.GetValue() + "\n";
            _statsStrengthSlider.value = _playerTricksProgressionStrength.GetStreakPercentage();
            _statsAgilitySlider.value = _playerTricksProgressionAgility.GetStreakPercentage();
        }

        private void LoadPlayerTricks()
        {
            foreach (Transform child in _tricksContainer) {
                Destroy(child.gameObject);
            }
        
            foreach (Trick trick in _playerTricks.Tricks)
            {
                var trickButton = Instantiate(_trickButtonPrefab, _tricksContainer);
                trickButton.GetComponent<TrickButton>().SetTrick(trick, _playerTricks);
            }
        }

        private void LoadTournamentProgress()
        {
            _tournamentProgressUI.gameObject.SetActive(TournamentProgression.instance);
            
            if (!TournamentProgression.instance) { return; }

            int currentPoints = TournamentProgression.instance.GetCurrentPoints();
            int pointsToComplete = TournamentProgression.instance.GetPointsToComplete();
            int pointsToCompleteMax = TournamentProgression.instance.GetPointsToCompleteMax();
            
            _tournamentProgressText.text = currentPoints + " / " + pointsToComplete + " (" + pointsToCompleteMax + ")";
            _tournamentProgressSlider.value = (float) currentPoints / pointsToCompleteMax;
            _tournamentProgressSliderToFinish.value = (float) pointsToComplete / pointsToCompleteMax;
        }
        
        private void PlayerTricksOnPlayerFailedTrick(Trick obj)
        {
            Instantiate(_failedTrickPrefab, transform);
        }
        
        private void TournamentProgressionOnTournamentCompleted()
        {
            _tournamentProgressSliderFill.color = _tournamentProgressSliderFillColorCompleted;
            _quitButton.SetActive(false);
            _finishButton.SetActive(true);
        }
        
        private void TournamentProgressionOnTimerTick(int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            _tournamentProgressTimerText.text = ts.ToString("mm\\:ss");
        }

        public void ClickFinish()
        {
            if (TournamentProgression.instance)
            {
                TournamentProgression.instance.EndTournament();
            }
            else
            {
                SceneManager.LoadScene("Lobby");
            }
        }
        
        private void TournamentProgressionOnTournamentEnded(TournamentResult result)
        {
            _tournamentInProgressScreen.alpha = 0;
            _tournamentEndScreen.SetActive(true);
            
            _tournamentCompletedUI.SetActive(result.Completed);
            _tournamentLostUI.SetActive(!result.Completed);

            _tournamenCompletedAbilityRewardsText.text = $"Репутация: +{result.ReputationRewarded} (+{result.ReputationRewardedExtra} Доп.) Всего: {result.ReputationRewardedTotal}";
            _tournamenCompletedMoneyRewardsText.text = $"Mонеты: +{result.MoneyRewarded} (+{result.MoneyRewardedExtra} Доп.) Всего: {result.MoneyRewardedTotal}";
        }
        
        public void ClickReturnToLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
