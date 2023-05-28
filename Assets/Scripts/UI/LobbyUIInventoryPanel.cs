using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Items;
using Player;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUIInventoryPanel : MonoBehaviour
{
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private Transform _inventorySlotContainer;
    [SerializeField] private Transform _wearableSlotContainer;
    [SerializeField] private Image _inventoryDragIcon;
    [SerializeField] private GameObject _trashPanel;
    private int _trashSlot;
    private Item _dragItem;
    private LobbyUIInventorySlot _dragSlot;
    private bool _isOpen;
    private Dictionary<int, LobbyUIInventorySlot> _inventorySlots = new Dictionary<int, LobbyUIInventorySlot>();
    private Dictionary<ItemWearableTypes, LobbyUIInventorySlot> _wearableSlots = new Dictionary<ItemWearableTypes, LobbyUIInventorySlot>();

    private void Awake()
    {
        _inventoryPanel.SetActive(false);
        _trashPanel.SetActive(false);
        _inventoryDragIcon.gameObject.SetActive(false);
        PopulateSlots();
        StarterAssetsInputs.InventoryPressed += StarterAssetsInputsOnInventoryPressed;
        PlayerInventory.PlayerInventoryLoaded += PlayerStatsOnPlayerInventoryUpdated;
        PlayerInventory.PlayerInventorySlotUpdated += PlayerInventoryOnPlayerInventorySlotUpdated;
        PlayerInventory.PlayerInventoryWearableUpdated += PlayerInventoryOnPlayerInventoryWearableUpdated;
    }

    private void OnDestroy()
    {
        StarterAssetsInputs.InventoryPressed -= StarterAssetsInputsOnInventoryPressed;
        PlayerInventory.PlayerInventoryLoaded -= PlayerStatsOnPlayerInventoryUpdated;
        PlayerInventory.PlayerInventorySlotUpdated -= PlayerInventoryOnPlayerInventorySlotUpdated;
        PlayerInventory.PlayerInventoryWearableUpdated -= PlayerInventoryOnPlayerInventoryWearableUpdated;
    }

    private void StarterAssetsInputsOnInventoryPressed()
    {
        if (_lobbyUI.GetIsOccupied() && !_isOpen)
        {
            return;
        }
        _isOpen = !_isOpen;
        _inventoryPanel.SetActive(_isOpen);
        _lobbyUI.SetOccupied(_isOpen);

    }

    public void PopulateSlots()
    {
        for (int i = 0; i < PlayerInventory.MaxSize; i++)
        {
            var go = Instantiate(_inventorySlotPrefab, _inventorySlotContainer);
            var inventorySlot = go.GetComponent<LobbyUIInventorySlot>();
            inventorySlot.InitSlot(i, _lobbyUI, this);
            _inventorySlots.Add(i, inventorySlot);
        }

        foreach (ItemWearableTypes wearableType in Enum.GetValues(typeof(ItemWearableTypes)))
        {
            var go = Instantiate(_inventorySlotPrefab, _wearableSlotContainer);
            var inventorySlot = go.GetComponent<LobbyUIInventorySlot>();
            inventorySlot.InitWearableSlot(wearableType, _lobbyUI, this);
            _wearableSlots.Add(wearableType, inventorySlot);
        }
    }

    private void PlayerStatsOnPlayerInventoryUpdated(PlayerInventory playerInventory)
    {
        foreach (var item in playerInventory.Items)
        {
            _inventorySlots[item.Key].InitItem(playerInventory.GetItemInSlot(item.Key));
        }
        
        foreach (var item in playerInventory.WearableItems)
        {
            _wearableSlots[item.Key].InitItem(playerInventory.GetItemInWear(item.Key));
        }
    }
    
    private void PlayerInventoryOnPlayerInventorySlotUpdated(int slot, Item item)
    {
        _inventorySlots[slot].InitItem(item);
    }
    
    private void PlayerInventoryOnPlayerInventoryWearableUpdated(ItemWearableTypes wearableType, ItemWearable item)
    {
        _wearableSlots[wearableType].InitItem(item);
    }

    public void SetDragItem(Item item, LobbyUIInventorySlot dragSlot)
    {
        _dragItem = item;
        _dragSlot = dragSlot;
        if (item == null)
        {
            _inventoryDragIcon.gameObject.SetActive(false); 
            return;
        }

        _inventoryDragIcon.gameObject.SetActive(true);
        _inventoryDragIcon.sprite = item.icon;
    }

    public void EndDragItem()
    {
        _dragItem = null;
        _dragSlot = null;
        _inventoryDragIcon.gameObject.SetActive(false);
    }

    public void DragItem(LobbyUIInventorySlot slot, PointerEventData eventData)
    {
        if(slot != _dragSlot) { return; }
        _inventoryDragIcon.rectTransform.position = eventData.position;
    }

    public void DragItemDropped(LobbyUIInventorySlot onto)
    {
        _inventoryDragIcon.gameObject.SetActive(false);
        if (_dragSlot == onto || _dragSlot == null)
        {
            _dragSlot = null;
            _dragItem = null;
            return;
        }

        if (!_dragSlot.IsWearable())
        {
            if (onto.IsWearable())
            {
                if (_dragItem.GetType() != typeof(ItemWearable) || onto.GetWearable() != ((ItemWearable)_dragItem).wearableType)
                {
                    _dragSlot = null;
                    _dragItem = null;
                    return;
                }
                PlayerStats.instance.GetInventory().WearSlot(_dragSlot.GetSlot());
            }
            else
            {
                PlayerStats.instance.GetInventory().SwapSlots(_dragSlot.GetSlot(), onto.GetSlot());
            }
        }
        else
        {
            PlayerStats.instance.GetInventory().UnwearItem(_dragSlot.GetWearable(), onto.GetSlot());
        }
        _dragSlot = null;
        _dragItem = null;
    }

    public void TrashItemDropped()
    {
        if (_dragItem == null || _dragSlot.GetSlot() == -1)
        {
            return;
        }
        
        _trashSlot = _dragSlot.GetSlot();
        _inventoryDragIcon.gameObject.SetActive(false);
        _dragSlot = null;
        _dragItem = null;
        _trashPanel.SetActive(true);
    }

    public void TrashItemConfirm()
    {
        PlayerStats.instance.GetInventory().RemoveItemFromSlot(_trashSlot);
        _trashPanel.SetActive(false);
    }
}
