using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Items;
using TMPro;
using UnityEngine;

public class LobbyUIShopPanel : MonoBehaviour
{
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _shopSlotPrefab;
    [SerializeField] private Transform _shopSlotContainer;
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private GameObject _confirmModal;
    [SerializeField] private TextMeshProUGUI _confirmModalNameText;
    [SerializeField] private TextMeshProUGUI _confirmModalPriceText;
    private Item _modalItem;

    private void Awake()
    {
        _shopPanel.SetActive(false);
        _confirmModal.SetActive(false);
        
        InteractableShop.InteractedShop += InteractableShopOnInteractedShop;
        PlayerStats.PlayerStatsChanged += PlayerStatsOnPlayerStatsChanged;
    }

    private void PlayerStatsOnPlayerStatsChanged(PlayerStatsData playerStatsData)
    {
        _balanceText.text = "Деньги: " + playerStatsData.Money;
    }

    private void OnDestroy()
    {
        InteractableShop.InteractedShop -= InteractableShopOnInteractedShop;
        PlayerStats.PlayerStatsChanged -= PlayerStatsOnPlayerStatsChanged;
    }

    private void InteractableShopOnInteractedShop(ShopType shopType)
    {
        if (_lobbyUI.GetIsOccupied())
        {
            return;
        }
        
        SpawnItems(shopType);

        _lobbyUI.SetOccupied(true);
        _shopPanel.SetActive(true);
    }

    public void CloseClicked()
    {
        _lobbyUI.SetOccupied(false);
        _shopPanel.SetActive(false);
    }

    private void SpawnItems(ShopType shopType)
    {
        foreach (Transform g in _shopSlotContainer)
        {
            Destroy(g.gameObject);
        }
        
        foreach (var item in _items)
        {
            if (shopType == ShopType.Wearable && item.GetType() != typeof(ItemWearable))
            {
                continue;
            }
            
            if (shopType == ShopType.Usable && item.GetType() != typeof(ItemUsable))
            {
                continue;
            }
            
            var go = Instantiate(_shopSlotPrefab, _shopSlotContainer);
            go.GetComponent<LobbyUIShopItemSlot>().Init(item, this);
        }
    }

    public void PurchaseClicked(Item item)
    {
        _confirmModalNameText.text = item.displayName;
        _confirmModalPriceText.text = item.price.ToString();
        _modalItem = item;
        _confirmModal.SetActive(true);
    }

    public void PurchaseConfirmed()
    {
        PlayerStats.instance.GetInventory().PurchaseItem(_modalItem);
        _confirmModal.SetActive(false);
    }
}
