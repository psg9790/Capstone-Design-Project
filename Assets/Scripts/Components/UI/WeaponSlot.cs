using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : ItemSlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            if (Inventory.instance.isInstallation==true && Inventory.instance.count<28)
            {
                Player.Instance.weaponManager.UnRegisterWeapon();
                grade_Back.gameObject.SetActive(false);
                for (int i = 0; i < 6; i++)
                {
                    Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(true);
                }

                Inventory.instance.AddItem(Inventory.instance.tempItem);
                DragSlot.instance.dragSlot.grade_Back.gameObject.SetActive(false);
                Inventory.instance.tempItem = null;
                
                Inventory.instance.weaponSlot.itemSlotui.gameObject.SetActive(false);
                Inventory.instance.backImage.gameObject.SetActive(true);
                Inventory.instance.isInstallation = false;
            }
            else if(Inventory.instance.isInstallation==true && Inventory.instance.count==28)
            {
                Inventory.instance.popUp.text="슬롯이 가득 차 있어 무기를 해제할 수 없습니다";
                Inventory.instance.popUp.gameObject.SetActive(true);
                Invoke("popupHide", 1.0f);
            }
        }
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (Inventory.instance.tempItem!=null)
        {
            UnityEngine.Debug.Log("beginDrag");
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.beginSlot = 1;
            DragSlot.instance.DragSetImage(itemSlotui.image);

            DragSlot.instance.transform.position = eventData.position;
        }

    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        if (Inventory.instance.tempItem!=null)
        {
            UnityEngine.Debug.Log("OnDrag");
            DragSlot.instance.transform.position = eventData.position;
        }
           
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ItemGenerator.Instance.PlayerDropItem(Inventory.instance.tempItem);
            Player.Instance.weaponManager.UnRegisterWeapon();
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            Inventory.instance.isInstallation = false;
            Inventory.instance.tempItem = null;
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot.itemSlotui.item.itemData.itemType==ItemType.Weapon)
        { 
            weapon_item = DragSlot.instance.dragSlot.itemSlotui.item as Weapon;             // 아이템 슬롯에서 드래그한 것.
            extra_item = Inventory.instance.tempItem;
            
            grade_Back.gameObject.SetActive(true);
            grade_Back.sprite = DragSlot.instance.dragSlot.grade_Back.sprite;
            
            Inventory.instance.tempItem = DragSlot.instance.dragSlot.itemSlotui.item;
            itemSlotui.item = Inventory.instance.tempItem;
            Inventory.instance.weaponSlot.itemSlotui.image.sprite = DragSlot.instance.dragSlot.itemSlotui.image.sprite;
            Inventory.instance.weaponSlot.itemSlotui.image.color = DragSlot.instance.dragSlot.itemSlotui.image.color;
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            
            if (Inventory.instance.isInstallation == true)  // 무기 장착이 되어 있다면 
            {
                
                Inventory.instance.AddItem(extra_item); // 현재 ㅊ
            }
            else
            {
                Inventory.instance.backImage.gameObject.SetActive(false); 
                Inventory.instance.weaponSlot.itemSlotui.image.gameObject.SetActive(true); 
                Inventory.instance.isInstallation = true;
            }
            
            
            GameObject weapon = Instantiate(Inventory.instance.tempItem.itemData.weapon_gameObject);
            Player.Instance.weaponManager.SetWeapon(weapon);

            for (int i = 0; i < 6; i++)
            {
                UnityEngine.Debug.Log("unlock");
                Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(true);
            }

            arti_count = (int)(weapon_item.options[WeaponKey.SOCKET]);

 
            for (int i = 0; i < arti_count; i++)
            {
                UnityEngine.Debug.Log("lock");
                Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(false);
            }
        }
    }
    
    public void popupHide()
    {
        Inventory.instance.popUp.gameObject.SetActive(false);
    }
}
