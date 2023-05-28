using System;
using UnityEngine;

[System.Serializable]
public enum ShopType
{
    Usable,
    Wearable,
}

public class InteractableShop : MonoBehaviour
{
    [SerializeField] private ShopType _shopType;
    public static event Action<ShopType> InteractedShop;

    public void Use()
    {
        InteractedShop?.Invoke(_shopType);
    }
}
