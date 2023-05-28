using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;

public class ItemInfoCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private TextMeshProUGUI _infoText;
    private ItemHoverInfo _itemHoverInfo;
    private Item _item;


    public void Init(Item item, ItemHoverInfo itemHoverInfo)
    {
        _item = item;
        if (item == null)
        {
            Destroy(gameObject);
        }

        _itemHoverInfo = itemHoverInfo;
        _infoText.text = _infoText.text.Replace("{name}", item.displayName);
        _infoText.text = _infoText.text.Replace("{type}", item.GetDisplayType());
        _infoText.text = _infoText.text.Replace("{strength}", item.abilityModifiers.Strength.baseValue.ToString());
        _infoText.text = _infoText.text.Replace("{agility}", item.abilityModifiers.Agility.baseValue.ToString());
        _infoText.text = _infoText.text.Replace("{reputation}", item.abilityModifiers.Reputation.baseValue.ToString());
    }

    public void UpdatePanelPosition(Vector3 position)
    {
        _panel.position = position;
        if (_panel.position.y < _panel.sizeDelta.y)
        {
            _panel.position = new Vector3(_panel.position.x, position.y + _panel.sizeDelta.y, _panel.position.z);
        }
    }

    private void Update()
    {
        if (_itemHoverInfo && !_itemHoverInfo.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
}
