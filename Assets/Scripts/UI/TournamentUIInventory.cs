using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Player;
using UnityEngine;

public class TournamentUIInventory : MonoBehaviour
{

    [SerializeField]private GameObject _slotPrefab;
    [SerializeField]private Transform _slotContainer;
    private Dictionary<int, TournamentUIInventoryItemSlot> _itemSlots =
        new Dictionary<int, TournamentUIInventoryItemSlot>();


    private void Awake()
    {
        PlayerInventory.PlayerInventoryLoaded += PlayerInventoryOnPlayerInventoryLoaded;
        PlayerInventory.PlayerInventorySlotUpdated += PlayerInventoryOnPlayerInventorySlotUpdated;
    }

    private void OnDestroy()
    {
        PlayerInventory.PlayerInventoryLoaded -= PlayerInventoryOnPlayerInventoryLoaded;
        PlayerInventory.PlayerInventorySlotUpdated -= PlayerInventoryOnPlayerInventorySlotUpdated;

    }

    private void PlayerInventoryOnPlayerInventoryLoaded(PlayerInventory playerInventory)
    {
        foreach (var slotItem in playerInventory.Items)
        {
            var item = playerInventory.GetItemInSlot(slotItem.Key);
            if (item.GetType() == typeof(ItemUsable))
            {

                var slot = Instantiate(_slotPrefab, _slotContainer).GetComponent<TournamentUIInventoryItemSlot>();
                slot.Init((ItemUsable)item, slotItem.Key);
                _itemSlots.Add(slotItem.Key, slot);       
            }
        }
    }
    
    private void PlayerInventoryOnPlayerInventorySlotUpdated(int slotId, Item slotItem)
    {
        if (_itemSlots.ContainsKey(slotId) && slotItem == null)
        {
            var go = _itemSlots[slotId].gameObject;
            _itemSlots.Remove(slotId);
            Destroy(go);
        }
    }
}
