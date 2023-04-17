using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public ItemSlotUI dragSlot;

    [SerializeField] private Image itemImage;
    // Start is called before the first frame update
    private void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        itemImage.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
}
