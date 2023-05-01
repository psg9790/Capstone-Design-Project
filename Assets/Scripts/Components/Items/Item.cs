using System;
using System.Collections.Generic;

public struct ItemOption
{
    // public float atk;
    // public float atk_speed;
    // public float atk_cooldown;
    // public int tripod_skillNum;
    // public int tripod_tripodNum;
    //
    // public float def;
    // public float hp;
    // public float movement_speed;

    public List<ItemValueType> typeidx;
    public List<float> values;
}

public class Item
{
    public ItemData itemData;
    public ulong id;
    public ItemOption itemOption;


    public Item(){}
    public Item(ItemData itemData)
    {
        this.itemData = itemData;
    }
}