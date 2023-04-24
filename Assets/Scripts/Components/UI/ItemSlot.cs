using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemSlotUI itemSlotui;
    private Item _item;
    private Item dropItem;
    public ItemSlot dropSlot;
    public Image image;
    private SlotToolTip theSlot;
    public SlotToolTip _SlotToolTip;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            /*
            DragSlot.instance.dragSlot = this;
            if (itemSlotui.item != null && itemSlotui.item.ItemType==Weapon)     
            {
               if (itemSlotui.item.type == itemSlotui.item.ItemType.Weapon)
               {
                    Inventory.instance.weaponBack.gameObject.SetActive(false);
                    Inventory.instance.weaponImage.sprite = itemSlotui.image.sprite;
                    Inventory.instance.weaponImage.color = itemSlotui.image.color;
                    Inventory.instance.weaponImage.gameObject.SetActive(true);
                }
                
            }
            
            if (itemSlotui.item != null && itemSlotui.item.ItemType==Artifact)        // 아티팩트일 때    
            {
                if (itemSlotui.item.type == itemSlotui.item.ItemType.Weapon)
                {
                    
                }
            }
            DragSlot.instance.dragSlot = null;
            */
        }
         
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlotui.item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemSlotui.image);

            DragSlot.instance.transform.position = eventData.position;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(itemSlotui.item!=null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot!=null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item tempItem = itemSlotui.item;
        
        itemSlotui.item= DragSlot.instance.dragSlot.itemSlotui.item;
        
        if (tempItem != null)
        {
            DragSlot.instance.dragSlot.itemSlotui.item = tempItem;
        }
        else
        {
            DragSlot.instance.dragSlot.itemSlotui.item = null;
            DragSlot.instance.dragSlot.image.sprite=null;
            DragSlot.instance.dragSlot.itemSlotui.gameObject.SetActive(false);
        }
    }

    // 마우스가 슬롯에 들어갈 때 발동.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlotui != null)
        {
            _SlotToolTip.ShowToolTip(itemSlotui.item, transform.position);
        }
    }
    
    // 슬롯에서 빠져나갈 때 발동. 
    public void OnPointerExit(PointerEventData eventData)
    {
        _SlotToolTip.HideToolTip();
    }
}
