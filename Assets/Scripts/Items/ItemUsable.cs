using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemName", menuName = "Items/New Usable Item")]
public class ItemUsable : Item
{
    public float useTime = .25f;

    public override string GetDisplayType()
    {
        return "Используемый предмет";
    }
}
