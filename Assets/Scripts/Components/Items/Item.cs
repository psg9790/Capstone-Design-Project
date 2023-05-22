using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemData itemData;
    public ulong id;
    public short tier;
    public string itemName;
    [ColorPalette] public Color itemColor;
    
    public Item()
    {
    }

    public Item(ItemData itemData, ulong id, short tier)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
    }

    public virtual List<string> Options_ToString()
    {
        return null;
    }
}