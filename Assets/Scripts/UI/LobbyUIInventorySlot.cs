using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUIInventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private ItemHoverInfo _hoverInfo;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    private LobbyUI _lobbyUI;
    private LobbyUIInventoryPanel _lobbyUIInventoryPanel;
    private int _slot = -1;
    private ItemWearableTypes _wearableType;
    private Item _item;
    
    public void InitSlot(int slot, LobbyUI lobbyUI, LobbyUIInventoryPanel lobbyUIInventoryPanel)
    {
        _slot = slot;
        _button.gameObject.SetActive(false);
        _lobbyUI = lobbyUI;
        _lobbyUIInventoryPanel = lobbyUIInventoryPanel;
    }

    public void InitWearableSlot(ItemWearableTypes wearableType, LobbyUI lobbyUI, LobbyUIInventoryPanel lobbyUIInventoryPanel)
    {
        _wearableType = wearableType;
        _button.gameObject.SetActive(false);
        _lobbyUI = lobbyUI;
        _lobbyUIInventoryPanel = lobbyUIInventoryPanel;
    }
    
    public void InitItem(Item item)
    {
        if (item == null)
        {
            _button.gameObject.SetActive(false);
            _item = null;
            _hoverInfo.Destroy();
            return;
        }
        
        _icon.sprite = item.icon;
        _item = item;
        _hoverInfo.Init(_item);
        _button.gameObject.SetActive(true);
    }


    public void OnDrag(PointerEventData eventData)
    {
        _lobbyUIInventoryPanel.DragItem(this, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            _lobbyUIInventoryPanel.SetDragItem(_item, this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            _lobbyUIInventoryPanel.EndDragItem();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        _lobbyUIInventoryPanel.DragItemDropped(this);
    }

    public bool IsWearable()
    {
        return _slot == -1;
    }

    public int GetSlot()
    {
        return _slot;
    }

    public ItemWearableTypes GetWearable()
    {
        return _wearableType;
    }
}
