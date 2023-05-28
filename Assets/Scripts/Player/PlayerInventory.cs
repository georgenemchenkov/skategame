using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Items;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerInventory
    {
        public const int MaxSize = 8;

        private bool _isUseInProgress = false;
        // item slot, item name
        public Dictionary<int, string> Items = new Dictionary<int, string>();
        public Dictionary<ItemWearableTypes, string> WearableItems = new Dictionary<ItemWearableTypes, string>();
        
        [NonSerialized]
        public Dictionary<string, Item> CachedItems = new Dictionary<string, Item>();
        
        public static event Action<PlayerInventory>  PlayerInventoryLoaded;
        public static event Action<int, Item> PlayerInventorySlotUpdated;
        public static event Action<ItemWearableTypes, ItemWearable> PlayerInventoryWearableUpdated;
        public static event Action<Item> PlayerInventoryItemUsed;

        [NonSerialized]
        private List<Item> _usedItems = new List<Item>();

        public void InitLoadInvoke()
        {
            CachedItems = new Dictionary<string, Item>();
            _usedItems = new List<Item>();
            _isUseInProgress = false;
            PlayerInventoryLoaded?.Invoke(this);
        }


        public void AddItem(string itemId)
        {
            if (Items.Count < MaxSize)
            {
                AddItemToSlot(GetFirstFreeSlot(), itemId);
            }
        }
        
        public void AddItem(Item item)
        {
            if (Items.Count < MaxSize)
            {
                AddItemToSlot(GetFirstFreeSlot(), item);
            }
        }

        /**
         * Core inventory logic [Create]
         */
        public void AddItemToSlot(int slot, string itemId)
        {
            if (Items.ContainsKey(slot))
            {
                return;
            } 
            
            var item = CacheItemOrSkip(itemId);
            Items.Add(slot, itemId);
            PlayerInventorySlotUpdated?.Invoke(slot, item);
            PlayerStats.instance.SavePlayerStats();
        }
        
        /**
         * Core inventory logic [Create] overload
         */
        public void AddItemToSlot(int slot, Item item)
        {
            if (Items.ContainsKey(slot))
            {
                return;
            }
            
            CacheItemOrSkip(item);
            Items.Add(slot, item.name);
            PlayerInventorySlotUpdated?.Invoke(slot, item);
            PlayerStats.instance.SavePlayerStats();
        }
        
        /**
         * Core inventory logic swap 
         */
        private void SwapItemWearable(int slotId, ItemWearableTypes wearableType)
        {
            if (!Items.ContainsKey(slotId))
            {
                return;
            }

            var item1 = CacheItemOrSkip(Items[slotId]);
            var item2 = CacheItemOrSkip(WearableItems[wearableType]);

            if (((ItemWearable) item1).wearableType != ((ItemWearable) item2).wearableType)
            {
                return;
            }

            Items.Remove(slotId);
            WearableItems.Remove(wearableType);
            
            Items.Add(slotId, item2.name);
            WearableItems.Add(wearableType, item1.name);
            
            
            PlayerInventorySlotUpdated?.Invoke(slotId, item2);
            PlayerInventoryWearableUpdated?.Invoke(wearableType, (ItemWearable)item1);
            PlayerStats.instance.SavePlayerStats();
        }

        public void SwapSlots(int slotId1, int slotId2)
        {
            if (Items.ContainsKey(slotId2))
            {
                (Items[slotId1], Items[slotId2]) = (Items[slotId2], Items[slotId1]);
                PlayerInventorySlotUpdated?.Invoke(slotId1, GetCachedItem(Items[slotId1]));
                PlayerInventorySlotUpdated?.Invoke(slotId2, GetCachedItem(Items[slotId2]));
                PlayerStats.instance.SavePlayerStats();

            }
            else
            {
                Items[slotId2] = Items[slotId1];
                Items.Remove(slotId1);
                PlayerInventorySlotUpdated?.Invoke(slotId1, null);
                PlayerInventorySlotUpdated?.Invoke(slotId2, GetCachedItem(Items[slotId2]));
                PlayerStats.instance.SavePlayerStats();
            }
        }

        public void RemoveItemFromSlot(int slot)
        {
            Items.Remove(slot);
            PlayerInventorySlotUpdated?.Invoke(slot, null);
            PlayerStats.instance.SavePlayerStats();
        }

        public void WearSlot(int slot)
        {
            if (Items.ContainsKey(slot) && GetCachedItem(Items[slot]).GetType() == typeof(ItemWearable))
            {
                ItemWearable itemWearable = (ItemWearable)GetCachedItem(Items[slot]);
                if (WearableItems.ContainsKey(itemWearable.wearableType))
                {
                    SwapItemWearable(slot, itemWearable.wearableType);
                }
                else
                {
                    Items.Remove(slot);
                    WearableItems.Add(itemWearable.wearableType, itemWearable.name);
                    PlayerInventorySlotUpdated?.Invoke(slot, null);
                    PlayerInventoryWearableUpdated?.Invoke(itemWearable.wearableType, itemWearable);
                    PlayerStats.instance.SavePlayerStats();
                }
            }
        }

        public void UnwearItem(ItemWearableTypes wearableType, int toSlot)
        {
            if (toSlot == -1)
            {
                return;
            }
            if (WearableItems.ContainsKey(wearableType))
            {
                if (Items.ContainsKey(toSlot) && GetCachedItem(Items[toSlot]).GetType() != typeof(ItemWearable))
                {
                    return;
                }

                if (Items.ContainsKey(toSlot))
                {
                    SwapItemWearable(toSlot, wearableType);
                }
                else
                {
                    var item = (ItemWearable)GetItemInWear(wearableType);
                    
                    Items.Add(toSlot, item.name);
                    WearableItems.Remove(wearableType);
                    PlayerInventorySlotUpdated?.Invoke(toSlot, item);
                    PlayerInventoryWearableUpdated?.Invoke(item.wearableType, null);
                    PlayerStats.instance.SavePlayerStats();
                }
            }
        }

        public int GetFirstFreeSlot()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                if (!Items.ContainsKey(i))
                {
                    return i;
                }
            }

            return -1;
        }

        public Item GetItemInSlot(int slot)
        {
            if (!Items.ContainsKey(slot))
            {
                return null;
            }

            return GetCachedItem(Items[slot]);
        }
        
        public Item GetItemInWear(ItemWearableTypes wearableType)
        {
            if (!WearableItems.ContainsKey(wearableType))
            {
                return null;
            }

            return GetCachedItem(WearableItems[wearableType]);
        }

        private Item GetCachedItem(string id)
        {
            Item item = null;
            if (!CachedItems.ContainsKey(id))
            {
                item = Item.GetById(id);
                CachedItems.Add(id, item);
            }
            else
            {
                item = CachedItems[id];
            }

            return item;
        }

        private Item CacheItemOrSkip(string id)
        {
            if (!CachedItems.ContainsKey(id))
            {
                Item item = Item.GetById(id);
                CachedItems.Add(id, item);
                return item;
            }
            else
            {
                return CachedItems[id];
            }
        }
        
        private void CacheItemOrSkip(Item item)
        {
            if (!CachedItems.ContainsKey(item.name))
            {
                CachedItems.Add(item.name, item);
            }
        }

        public void UseItem(int usableSlotId)
        {
            if (!CanUse())
            {
                Debug.Log("Item is already in use!");
                return;
            }
            
            Debug.Log("Using item from slot " + usableSlotId);


            var item = (ItemUsable)GetItemInSlot(usableSlotId);
            UseItemAsync(usableSlotId, item);
            /*
            var item = (ItemUsable)GetItemInSlot(usableSlotId);
            _isUseInProgress = true;
            Debug.Log("task start");
            Task.Run(async() =>
            {
                await Task.Delay((int)(item.useTime * 100));
                Debug.Log("task end");
                PlayerStats.instance.playerStatsData.AddModifiers(item.abilityModifiers);
                RemoveItemFromSlot(usableSlotId);
                Debug.Log("task end");
                _isUseInProgress = false;
            });
            */
        }

        private async void UseItemAsync(int usableSlotId, ItemUsable itemUsable)
        {
            _isUseInProgress = true;
            await Task.Delay((int)(itemUsable.useTime * 1000));
            PlayerStats.instance.AddModifiers(itemUsable.abilityModifiers);
            _usedItems.Add(itemUsable);
            PlayerInventoryItemUsed?.Invoke(itemUsable);
            RemoveItemFromSlot(usableSlotId);
            _isUseInProgress = false;
        }

        public bool CanUse()
        {
            return !_isUseInProgress;
        }

        public bool CanUseItem(ItemUsable item)
        {
            return !_usedItems.Contains(item);
        }

        public void PurchaseItem(Item item)
        {
            if (!CanPurchaseItem(item))
            {
                return;
            }
            
            PlayerStats.instance.UpdateMoney(-item.price);
            AddItem(item);
        }

        public bool CanPurchaseItem(Item item)
        {
            bool available = !(Items.ContainsValue(item.name) && item.GetType() == typeof(ItemWearable));

            if (WearableItems.ContainsValue(item.name))
            {
                available = false;
            }
            
            if (Items.Count >= MaxSize || PlayerStats.instance.playerStatsData.Money < item.price)
            {
                available = false;
            }

            return available;
        }
    }
}
