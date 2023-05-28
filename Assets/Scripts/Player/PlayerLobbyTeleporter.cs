using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyTeleporter : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    void Awake()
    {
        if (TournamentStaticData.LastPlayerPosition != Vector3.zero)
        {
            var charWasEnabled = _characterController.enabled;
            _characterController.enabled = false;
            transform.position = TournamentStaticData.LastPlayerPosition;
            if (charWasEnabled)
            {
                _characterController.enabled = true;
            }
        }
    }
}
