using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour
{
    public Image image;

    private Item _item;
    private Item dropItem;

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemData.iconSprite;
                image.color = new Color(1, 1, 1);
                this.gameObject.SetActive(true);
            }
        }
    }
}
