using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

public class PlayerWearableManager : MonoBehaviour
{
    private Dictionary<GameObject, ItemWearableTypes>
        _wearableObjects = new Dictionary<GameObject, ItemWearableTypes>();
    
    private Dictionary<ItemWearableTypes, ItemWearable>
        _itemWearables = new Dictionary<ItemWearableTypes, ItemWearable>();

    private void Awake()
    {
        PlayerInventory.PlayerInventoryLoaded += PlayerInventoryOnPlayerInventoryLoaded;
        PlayerInventory.PlayerInventoryWearableUpdated += PlayerInventoryOnPlayerInventoryWearableUpdated;
    }

    private void OnDestroy()
    {
        PlayerInventory.PlayerInventoryLoaded -= PlayerInventoryOnPlayerInventoryLoaded;
        PlayerInventory.PlayerInventoryWearableUpdated -= PlayerInventoryOnPlayerInventoryWearableUpdated;
    }

    private void PlayerInventoryOnPlayerInventoryWearableUpdated(ItemWearableTypes wearableType, ItemWearable itemWearable)
    {
        WearItem(wearableType, itemWearable);
    }

    private void WearItem(ItemWearableTypes wearableType, ItemWearable itemWearable)
    {
        foreach (var wearableObject in _wearableObjects.ToList())
        {
            if (wearableObject.Value == wearableType)
            {
                _wearableObjects.Remove(wearableObject.Key);
                Destroy(wearableObject.Key.gameObject);
            }
        }
        
        if (_itemWearables.ContainsKey(wearableType) && _itemWearables[wearableType] != itemWearable)
        {
            Debug.Log("Remove modifiers");
            PlayerStats.instance.RemoveModifiers(_itemWearables[wearableType].abilityModifiers);
            _itemWearables.Remove(wearableType);
        }
        

        if (itemWearable == null)
        {
            return;
        }
        
        foreach (var attachPrefab in itemWearable.ItemAttachPrefabs)
        {
            var t = RecursiveFindChild(transform, attachPrefab.attachBone);
            var go = Instantiate(attachPrefab.prefab, t);
            _wearableObjects.Add(go, itemWearable.wearableType);
        }
        _itemWearables.Add(wearableType, itemWearable);
        PlayerStats.instance.AddModifiers(itemWearable.abilityModifiers);
    }
    
    private void PlayerInventoryOnPlayerInventoryLoaded(PlayerInventory playerInventory)
    {
        foreach (var item in playerInventory.WearableItems)
        {
            WearItem(item.Key, (ItemWearable)playerInventory.GetItemInWear(item.Key));
        }
    }
    
    private static Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform result = null;

        foreach (Transform child in parent)
        {
            if (child.name == childName)
                result = child.transform;
            else
                result = RecursiveFindChild(child, childName);

            if (result != null) break;
        }

        return result;
    }
    
}
