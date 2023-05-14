using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemSlotUI itemSlotui;
    public Image image;
    public SlotToolTip _SlotToolTip; 
    public Weapon weapon_item;
    public int arti_count;
    public int number;
    public Image grade_Back;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            if (itemSlotui.item != null && itemSlotui.item is Weapon)
            {
                weapon_item = itemSlotui.item as Weapon; 
                if (Inventory.instance.isInstallation == true)
                {
                    UnityEngine.Debug.Log("1");
                    Inventory.instance.AddItem(Inventory.instance.tempItem);                    
                    Inventory.instance.tempItem = itemSlotui.item;
                }
                else if(Inventory.instance.isInstallation==false)
                {
                    Inventory.instance.tempItem = itemSlotui.item;                              // 무기 장착 아이템 정보 저장
                    Inventory.instance.backImage.gameObject.SetActive(false);                   // back 이미지 없앰.
                    Inventory.instance.weaponSlot.image.gameObject.SetActive(true);             // 무기 이미지 없앰.
                    Inventory.instance.isInstallation = true;
                }

                Inventory.instance.weaponSlot.image.sprite = itemSlotui.image.sprite;
                Inventory.instance.weaponSlot.image.color = itemSlotui.image.color;
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
            else if (itemSlotui.item != null && itemSlotui.item is Artifact && Inventory.instance.tempItem!=null)        // 아티팩트일 때    
            {
                weapon_item=  Inventory.instance.tempItem as Weapon;
                arti_count=(int)(weapon_item.options[WeaponKey.SOCKET]);
                
                UnityEngine.Debug.Log("artifact click");
                for (int i = 0; i < arti_count;i++)
                {
                    if (Inventory.instance.artifactUIs[i].isInstallation==false)
                    {
                        Inventory.instance.artifactUIs[i].itemSlot.image.sprite = itemSlotui.image.sprite;
                        Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item = itemSlotui.item;
                        Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
                        Inventory.instance.artifactUIs[i].isInstallation = true;
                        break;
                    }
                }
            }
            
            DragSlot.instance.dragSlot = null;
            
        }
         
        
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlotui.item != null)
        {
            DragSlot.instance.beginSlot = 0;
           
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemSlotui.image);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (itemSlotui.item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
           
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            ItemGenerator.Instance.PlayerDropItem(DragSlot.instance.dragSlot.itemSlotui.item);
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.beginSlot == 0 )
        {
            ChangeSlot();
        }else if (DragSlot.instance.dragSlot != null && DragSlot.instance.beginSlot == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(true);
            }
            
            Inventory.instance.AddItem(Inventory.instance.tempItem);
            Inventory.instance.tempItem = null;
            
            Inventory.instance.weaponSlot.image.gameObject.SetActive(false);
            Inventory.instance.backImage.gameObject.SetActive(true);
            Inventory.instance.isInstallation = false;
        } else if (DragSlot.instance.dragSlot != null && DragSlot.instance.beginSlot == 2)
        {
            if (DragSlot.instance.dragSlot.itemSlotui.item!=null)
            {
                DragSlot.instance.SetColor(0);
                Inventory.instance.AddItem(DragSlot.instance.dragSlot.itemSlotui.item);
                Inventory.instance.artifactUIs[DragSlot.instance.dragSlot.number].isInstallation = false;   
                Color color = Inventory.instance.artifactUIs[DragSlot.instance.dragSlot.number].itemSlot.image.color;
                color.a = 0;
                Inventory.instance.artifactUIs[DragSlot.instance.dragSlot.number].itemSlot.image.color = color;
                DragSlot.instance.dragSlot.itemSlotui.item= null;
            }
        }
        
        DragSlot.instance.dragSlot = null;
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
