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
            if (Inventory.instance.tempItem!=null)
            {
                UnityEngine.Debug.Log("weaponslot2");
                for (int i = 0; i < 6; i++)
                {
                    Inventory.instance.artifactUIs[i].lockImage.gameObject.SetActive(true);
                }

                Inventory.instance.AddItem(Inventory.instance.tempItem);
                Inventory.instance.tempItem = null;
                
                Inventory.instance.weaponSlot.image.gameObject.SetActive(false);
                Inventory.instance.backImage.gameObject.SetActive(true);
                Inventory.instance.isInstallation = false;
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
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            //ItemGenerator.Instance.PlayerDropItem(DragSlot.instance.dragSlot.itemSlotui.item);
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        Inventory.instance.tempItem = null;
        Inventory.instance.isInstallation = false;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot.itemSlotui.item is Weapon)
        { 
            weapon_item = DragSlot.instance.dragSlot.itemSlotui.item as Weapon;
            if (Inventory.instance.isInstallation == true)
            {
                Inventory.instance.AddItem(Inventory.instance.tempItem);
            }
            else
            {
                Inventory.instance.backImage.gameObject.SetActive(false); // back 이미지 없앰.
                Inventory.instance.weaponSlot.image.gameObject.SetActive(true); // 무기 이미지 없앰.
                Inventory.instance.isInstallation = true;
            }
            Inventory.instance.tempItem = DragSlot.instance.dragSlot.itemSlotui.item;
            Inventory.instance.weaponSlot.image.sprite = DragSlot.instance.dragSlot.itemSlotui.image.sprite;
            Inventory.instance.weaponSlot.image.color = DragSlot.instance.dragSlot.itemSlotui.image.color;
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);

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
}
