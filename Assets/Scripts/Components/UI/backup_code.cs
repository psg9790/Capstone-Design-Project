using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemSlotUI itemSlotui;
    public ItemSlot dropSlot;
    public Image image;
    public SlotToolTip _SlotToolTip;
    private Weapon weapon_item;
    private int arti_count;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            DragSlot.instance.dragSlot = this;
            if (itemSlotui.item != null && itemSlotui.item is Weapon)
            {
                weapon_item = itemSlotui.item as Weapon;
                if (Inventory.instance.isInstallation == true)
                {
                    UnityEngine.Debug.Log("true");
                    Inventory.instance.AddItem(Inventory.instance.tempItem);
                    Inventory.instance.tempItem = itemSlotui.item;
                }
                else
                {
                    Debug.Log("false");
                    Inventory.instance.tempItem = itemSlotui.item;
                    Inventory.instance.weaponBack.gameObject.SetActive(false);
                    Inventory.instance.weaponImage.gameObject.SetActive(true);
                    Inventory.instance.isInstallation = true;
                }

                Inventory.instance.weaponImage.sprite = itemSlotui.image.sprite;
                Inventory.instance.weaponImage.color = itemSlotui.image.color;
                Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
                
                for (int i = 0; i < 6; i++)
                {
                    Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(true);
                }
                
                arti_count = (int)(weapon_item.options[WeaponKey.SOCKET]);
                

                for (int i = 0; i <arti_count;i++)
                {
                    Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(false);
                }
                

            }
            
            else if (itemSlotui.item != null && itemSlotui.item.itemData.itemType==ItemType.Artifact)        // 아티팩트일 때    
            {
                for(int i=0;Inventory.instance.artifacts[i];i++)
                {
                    Inventory.instance.artifacts[i].artifactImage.sprite = itemSlotui.image.sprite ;
                    Inventory.instance.artifacts[i].artifactImage.gameObject.SetActive(true);
                    break;
                }
            }
            
            DragSlot.instance.dragSlot = null;
            
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
       
        if (itemSlotui.item != null)
        {
            _SlotToolTip.ShowToolTip(itemSlotui.item,transform.position);
        }
        
    }
    
    // 슬롯에서 빠져나갈 때 발동. 
    public void OnPointerExit(PointerEventData eventData)
    {
        _SlotToolTip.HideToolTip();
    }
}

*/
