using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTournament : MonoBehaviour
{
    [SerializeField] private Tournament _tournament;
    public static event Action<Tournament> InteractedTournament;

    private void Awake()
    {
        if (TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.SetName(_tournament.name);
        }
    }

    public void Interact()
    {
        InteractedTournament?.Invoke(_tournament);        
    }
}
