using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class TournamentUIInventoryItemSlot : MonoBehaviour
{
    [SerializeField] private ItemHoverInfo _hoverInfo;
    [SerializeField] private Image _sprite;
    [SerializeField] private Button _button;
    private int _slotId;
    private ItemUsable _item;

    public void Init(ItemUsable item, int slotId)
    {
        _item = item;
        _sprite.sprite = item.icon;
        _slotId = slotId;
        _hoverInfo.Init(item);
        
        PlayerInventory.PlayerInventoryItemUsed += PlayerInventoryOnPlayerInventoryItemUsed;
    }

    private void OnDestroy()
    {
        PlayerInventory.PlayerInventoryItemUsed -= PlayerInventoryOnPlayerInventoryItemUsed;
    }

    private void PlayerInventoryOnPlayerInventoryItemUsed(Item item)
    {
        if (!PlayerStats.instance.GetInventory().CanUseItem(_item))
        {
            if (_button != null)
            { 
                _button.interactable = false; 
            }
            return;
        }

        if (_button != null)
        {
            _button.interactable = true;
        }
    }

    public void Use()
    {
        if (!PlayerStats.instance.GetInventory().CanUse() || !PlayerStats.instance.GetInventory().CanUseItem(_item))
        {
            return;
        }

        if (_button != null)
        {
            _button.interactable = false;
        }
        LeanTween.scale(gameObject, Vector3.zero,_item.useTime).setEase(LeanTweenType.easeInOutSine);
        PlayerStats.instance.GetInventory().UseItem(_slotId);
    }
}
