using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField]private UnityEvent _onInteract;

    public string GetName()
    {
        return _name;
    }
    
    public void SetName(string name)
    {
        _name = name;
    }

    public void Interact()
    {
        _onInteract?.Invoke();
    }
}
