using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image image;

    private Item _item;
    public bool isClicked=false;
    private Item dropItem;
    public ItemSlotUI dropSlot;

    public Item item {
        get { return _item; }
        set {
            _item = value;
            if (_item != null) {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1 );
            } 
        }
    }

    
    private void Update()
    {
        if (isClicked == true)
        {
            Inventory.instance.removeItem(dropItem, dropSlot);
            isClicked = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(image);

            DragSlot.instance.transform.position = eventData.position;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_item!=null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ItemDropPopup.instance.gameObject.SetActive(true);
            dropItem = item;
            dropSlot = DragSlot.instance.dragSlot;
        }
        DragSlot.instance.SetColor(0);
        //DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot!=null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item tempItem = item;

        Debug.Log(DragSlot.instance);
        
        item= DragSlot.instance.dragSlot.item;
        
        if (tempItem != null)
        {
            DragSlot.instance.dragSlot.item = tempItem;
        }
        else
        {
            DragSlot.instance.dragSlot.item = null;
            DragSlot.instance.dragSlot.image.sprite=null;
        }
    }
}
