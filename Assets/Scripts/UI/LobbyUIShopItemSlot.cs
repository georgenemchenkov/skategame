using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIShopItemSlot : MonoBehaviour
{
    [SerializeField] private ItemHoverInfo _hoverInfo;
    [SerializeField] private LobbyUIShopPanel _shopPanel;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    private Item _item;

    public void Init(Item item, LobbyUIShopPanel shopPanel)
    {
        _item = item;
        _itemIcon.sprite = item.icon;
        _itemName.text = item.displayName;
        _itemPrice.text = item.price.ToString();
        _shopPanel = shopPanel;
        _hoverInfo.Init(item);
        PlayerInventory.PlayerInventorySlotUpdated += PlayerInventoryOnPlayerInventorySlotUpdated; 
    }

    private void Start()
    {
        RefreshAvailability();
    }

    private void OnDestroy()
    {
        PlayerInventory.PlayerInventorySlotUpdated -= PlayerInventoryOnPlayerInventorySlotUpdated; 
    }

    private void PlayerInventoryOnPlayerInventorySlotUpdated(int slotId, Item wItem)
    {
        RefreshAvailability();
    }

    private void RefreshAvailability()
    {
        if(_purchaseButton)
        _purchaseButton.interactable = PlayerStats.instance.GetInventory().CanPurchaseItem(_item);
    }

    public void PurchaseClicked()
    {
        _shopPanel.PurchaseClicked(_item);
    }
}
