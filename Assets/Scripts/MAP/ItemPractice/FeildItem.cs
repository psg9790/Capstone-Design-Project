using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeildItem : MonoBehaviour
{
    public ItemDataPractice item;
    public SpriteRenderer image;

    public void SetItem(ItemDataPractice _item)
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        
    }
    
    //아이템 획득
    public ItemDataPractice GetItem()
    {
        return item;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
    
}
