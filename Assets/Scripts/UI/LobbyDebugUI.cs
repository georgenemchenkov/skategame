using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class LobbyDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugInvText;
    [SerializeField] private TMP_InputField _slotIdInput;
    [SerializeField] private TMP_InputField _wearTypeInput;

    private void Update()
    {
        if(PlayerStats.instance == null) return;

        _debugInvText.text = "<color=red>inventory debugging</color>\n";
        _debugInvText.text += "<color=yellow>items</color>\n";
        foreach (var kv in PlayerStats.instance.GetInventory().Items)
        {
            _debugInvText.text += $"{kv.Key} {kv.Value}\n";
        }
        
        _debugInvText.text += "<color=yellow>wearable</color>\n";
        foreach (var kv in PlayerStats.instance.GetInventory().WearableItems)
        {
            _debugInvText.text += $"{kv.Key} {kv.Value}\n";
        }
    }

    public void GiveDevBootsClicked()
    {
        PlayerStats.instance.GetInventory().AddItem("DevBoots");
    }

    public void GiveDevBarClicked()
    {
        PlayerStats.instance.GetInventory().AddItem("DevBar");
    }
    
    public void GiveDevHatClicked()
    {
        PlayerStats.instance.GetInventory().AddItem("DevHat");
    }
    
    public void GiveDevGreenHatClicked()
    {
        PlayerStats.instance.GetInventory().AddItem("DevGreenHat");
    }

    public void RemoveSlotClicked()
    {
        int id = int.Parse(_slotIdInput.text);
        PlayerStats.instance.GetInventory().RemoveItemFromSlot(id);
    }

    public void WearSlotClicked()
    {
        int id = int.Parse(_slotIdInput.text);
        PlayerStats.instance.GetInventory().WearSlot(id);
    }

    public void UnwearSlotClicked()
    {
        Enum.TryParse(_wearTypeInput.text, out ItemWearableTypes wearableType);
        int id = int.Parse(_slotIdInput.text);
        PlayerStats.instance.GetInventory().UnwearItem(wearableType, id);
    }
}
