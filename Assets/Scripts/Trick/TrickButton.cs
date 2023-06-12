using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrickButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _chanceText;
    [SerializeField] private GameObject _improviseButton;
    [SerializeField] private GameObject _improviseCancelButton;
    private Trick _trick;
    private PlayerTricks _playerTricks;
    
    public void SetTrick(Trick trick, PlayerTricks playerTricks)
    {
        _trick = trick;
        _playerTricks = playerTricks;
        _nameText.text = trick.trickName;
        _descriptionText.text = $"Сила: {trick.abilitiesRequired.Strength.baseValue} Ловкость: {trick.abilitiesRequired.Agility.baseValue}";
        _chanceText.text = $"Шанс: {Mathf.Round(playerTricks.GetChance(trick)*100)}%";
        if (!playerTricks.CanPerform(trick))
        {
            _button.interactable = false;
            _improviseButton.SetActive(false);
        }

        if (!playerTricks.CanImprovise(trick))
        {
            _improviseButton.SetActive(false);
        }

        if (playerTricks.IsImprovising(trick) && playerTricks.CanPerform(trick))
        {
            _improviseCancelButton.SetActive(true);
        }
    }

    public void ActivateTrick()
    {
        _playerTricks.DoTrick(_trick);
    }

    public void ActivateImprovisation()
    {
        _playerTricks.ApplyImprovisation(_trick);
    }

    public void DeactivateImprovisation()
    {
        _playerTricks.CancelImprovisation(_trick);
    }
}
