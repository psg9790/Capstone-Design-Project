using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemTypePractice {
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class ItemDataPractice
{
    public ItemTypePractice itemType;
    public string itemName;
    public Sprite itemImage;

    public bool Use()
    {
        return false;
    }
    
}
