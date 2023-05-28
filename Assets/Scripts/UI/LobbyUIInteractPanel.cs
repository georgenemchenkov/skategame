using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUIInteractPanel : MonoBehaviour
{
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private ScaleUIAnimation _scaleUIAnimation;
    [SerializeField] private GameObject _interactPanel;
    [SerializeField] private TextMeshProUGUI _interactText;

    private void Awake()
    {
        _interactPanel.gameObject.SetActive(false);
        
        PlayerLobbyInteraction.PlayerInteractableEnter += PlayerLobbyInteractionOnPlayerInteractableEnter;
        PlayerLobbyInteraction.PlayerInteractableExit += PlayerLobbyInteractionOnPlayerInteractableExit;
    }

    private void OnDestroy()
    {
        PlayerLobbyInteraction.PlayerInteractableEnter -= PlayerLobbyInteractionOnPlayerInteractableEnter;
        PlayerLobbyInteraction.PlayerInteractableExit -= PlayerLobbyInteractionOnPlayerInteractableExit;
    }

    private void PlayerLobbyInteractionOnPlayerInteractableEnter(Interactable obj)
    {
        _interactPanel.gameObject.SetActive(true);
        _scaleUIAnimation.Show();
        _interactText.text = obj.GetName();
    }
    
    private void PlayerLobbyInteractionOnPlayerInteractableExit(Interactable obj)
    {
        _scaleUIAnimation.Hide();
    }
}
