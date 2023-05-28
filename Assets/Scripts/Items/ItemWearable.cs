using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

[System.Serializable]
public struct ItemAttachStruct
{
    public string attachBone;
    public GameObject prefab;
}
[CreateAssetMenu(fileName = "ItemName", menuName = "Items/New Wearable Item")]
public class ItemWearable : Item
{
    public ItemWearableTypes wearableType;
    public ItemAttachStruct[] ItemAttachPrefabs;

    public override string GetDisplayType()
    {
        return "Одежда";
    }
}
