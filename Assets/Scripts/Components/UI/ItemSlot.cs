using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
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
    private bool tooltip=false;
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            if (itemSlotui.item != null && itemSlotui.item.itemData.itemType == ItemType.Weapon)                             // 무기 장착
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
                     // back 이미지 없앰.
                    Inventory.instance.weaponSlot.itemSlotui.image.gameObject.SetActive(true);             // 무기 이미지 없앰.
                    Inventory.instance.isInstallation = true;
                }
                
                Inventory.instance.weaponSlot.grade_Back.gameObject.SetActive(true);
                Inventory.instance.weaponSlot.itemSlotui.image.sprite = itemSlotui.image.sprite;
                Inventory.instance.weaponSlot.itemSlotui.image.color = itemSlotui.image.color;
                Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
                
                
                GameObject weapon = Instantiate(Inventory.instance.tempItem.itemData.weapon_gameObject);
                Player.Instance.weaponManager.SetWeapon(weapon);
                
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
            else if (itemSlotui.item != null && itemSlotui.item.itemData.itemType==ItemType.Artifact && Inventory.instance.tempItem!=null)        // 아티팩트일 때    
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
            ItemGenerator.Instance.PlayerDropItem(DragSlot.instance.dragSlot.itemSlotui.item);
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            DragSlot.instance.dragSlot.itemSlotui.image.gameObject.SetActive(false);
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
            
            Player.Instance.weaponManager.UnRegisterWeapon();
            Inventory.instance.weaponSlot.grade_Back.gameObject.SetActive(false);
           
            Inventory.instance.AddItem(Inventory.instance.tempItem);
            
            Inventory.instance.tempItem = null;
            
            Inventory.instance.weaponSlot.itemSlotui.image.gameObject.SetActive(false);
            Inventory.instance.backImage.gameObject.SetActive(true);
            Inventory.instance.isInstallation = false;
        } else if (DragSlot.instance.dragSlot != null && DragSlot.instance.beginSlot == 2 )
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
        Item tempItem = itemSlotui.item;                                // 현재 장착하고 있는 아이템
        grade_Back.gameObject.SetActive(true);
        itemSlotui.item= DragSlot.instance.dragSlot.itemSlotui.item;    // 바뀔 아이템
        
        if (tempItem != null)
        {
            DragSlot.instance.dragSlot.itemSlotui.item = tempItem;
        }
        else
        {
            DragSlot.instance.dragSlot.grade_Back.gameObject.SetActive(false);
            DragSlot.instance.dragSlot.itemSlotui.item = null;
            DragSlot.instance.dragSlot.itemSlotui.image.sprite=null;
            DragSlot.instance.dragSlot.itemSlotui.gameObject.SetActive(false);
        }
    }

    // 마우스가 슬롯에 들어갈 때 발동.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlotui.item != null && tooltip==false)
        {
            _SlotToolTip.ShowToolTip(itemSlotui.item,transform.position);
            tooltip = true;
        }
        
    }
    
    // 슬롯에서 빠져나갈 때 발동. 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip == true) 
        {
            _SlotToolTip.HideToolTip();
            tooltip = false;
        }
        
    }
}
