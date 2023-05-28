using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyUIInventoryTrashSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private LobbyUIInventoryPanel _inventoryPanel;
    
    public void OnDrop(PointerEventData eventData)
    {
        _inventoryPanel.TrashItemDropped();
    }
}
