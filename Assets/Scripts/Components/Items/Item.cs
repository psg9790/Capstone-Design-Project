using System;
using System.Collections.Generic;


public class Item
{
    public ItemData itemData;
    public ulong id;
    public short tier;
    
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