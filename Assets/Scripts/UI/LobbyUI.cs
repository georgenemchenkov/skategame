using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerStatsText;
    private bool _isUIOccupied = false;
    
    private void Awake()
    {
        PlayerStats.PlayerStatsChanged += PlayerStatsOnPlayerStatsChanged;
    }

    private void OnDestroy()
    {
        PlayerStats.PlayerStatsChanged -= PlayerStatsOnPlayerStatsChanged;
    }

    private void PlayerStatsOnPlayerStatsChanged(PlayerStatsData statsData)
    {
        _playerStatsText.text = $"Сила: {statsData.Abilities.Strength.GetValue()}\n";
        _playerStatsText.text += $"Ловкость: {statsData.Abilities.Agility.GetValue()}\n";
        _playerStatsText.text += $"Репутация: {statsData.Abilities.Reputation.GetValue()}\n";
        _playerStatsText.text += $"Деньги: {statsData.Money}\n";
    }

    public void SetOccupied(bool val)
    {
        _isUIOccupied = val;
        
        StarterAssetsInputs.instance.SetCursorState(!val);
        //StarterAssetsInputs.instance.SetCursorInputForLook(!val);
        FindObjectOfType<ThirdPersonController>().LockCameraPosition = val;
    }

    public bool GetIsOccupied()
    {
        return _isUIOccupied;
    }
}
