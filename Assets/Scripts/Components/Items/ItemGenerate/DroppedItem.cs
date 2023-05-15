using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [ShowInInspector] public Item item;

    public void Adjust(Item item)
    {
        this.item = item;
        gameObject.name = item.itemName;
        VisualEffect();
    }

    private void VisualEffect()
    {
        
    }
}
