using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerLobbyInteraction : MonoBehaviour
{
    private Interactable _interactable;
    public static event Action<Interactable> PlayerInteractableEnter;
    public static event Action<Interactable> PlayerInteractableExit;

    private void Awake()
    {
        StarterAssetsInputs.InteractPressed += StarterAssetsInputsOnInteractPressed;
    }

    private void OnDestroy()
    {
        StarterAssetsInputs.InteractPressed -= StarterAssetsInputsOnInteractPressed;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (_interactable)
            {
                RemoveInteractable();
            }
            
            _interactable = interactable;
            PlayerInteractableEnter?.Invoke(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (_interactable == interactable)
            {
                _interactable = null;
                PlayerInteractableExit?.Invoke(interactable);
            }
        }
    }
    
    private void StarterAssetsInputsOnInteractPressed()
    {
        if (_interactable)
        {
            _interactable.Interact();
            //RemoveInteractable();
        }
    }

    private void RemoveInteractable()
    {
        PlayerInteractableExit?.Invoke(_interactable);
        _interactable = null;
    }
}
