using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactSlot : ItemSlot
{
    private Item artiItem;
    public Item temp_arti;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            artiItem = DragSlot.instance.dragSlot.itemSlotui.item;
            if (artiItem!=null && Inventory.instance.count!=28)
            {
                Color color = Inventory.instance.artifactUIs[number].itemSlot.image.color;
                color.a = 0;
                Inventory.instance.artifactUIs[number].itemSlot.image.color = color;
                Inventory.instance.AddItem(artiItem);
                Inventory.instance.artifactUIs[number].isInstallation = false;           
                DragSlot.instance.dragSlot.itemSlotui.item= null;    

            }
            else
            {
                Inventory.instance.popUp.text="슬롯이 가득 차 있어 아티팩트를 해제할 수 없습니다";
                Inventory.instance.popUp.gameObject.SetActive(true);
                Invoke("taketime", 1.0f);
                Inventory.instance.popUp.gameObject.SetActive(false);
            }
        }
        DragSlot.instance.dragSlot =null;
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (Inventory.instance.tempItem!=null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.beginSlot = 2;
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
            ItemGenerator.Instance.PlayerDropItem(DragSlot.instance.dragSlot.itemSlotui.item);
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            Inventory.instance.artifactUIs[number].isInstallation = false;
        }
        
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        
        if (DragSlot.instance.dragSlot != null && (DragSlot.instance.dragSlot.itemSlotui.item.itemData.itemType==ItemType.Artifact ) && Inventory.instance.isInstallation==true)
        {
            if (DragSlot.instance.beginSlot == 0)                           // 아이템 슬롯에서 드래그 하는 경우
            {
                if (Inventory.instance.artifactUIs[number].isInstallation==true)
                {
                    Inventory.instance.AddItem(itemSlotui.item);
                }

                Inventory.instance.artifactUIs[number].itemSlot.itemSlotui.image.sprite = DragSlot.instance.dragSlot.itemSlotui.image.sprite;
                Inventory.instance.artifactUIs[number].itemSlot.itemSlotui.item =  DragSlot.instance.dragSlot.itemSlotui.item;
                Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
                Inventory.instance.artifactUIs[number].isInstallation = true;

            } else if (DragSlot.instance.beginSlot == 2)
            {
                    temp_arti = itemSlotui.item;                                // 현재 장착하고 있는 아이템
                    itemSlotui.item= DragSlot.instance.dragSlot.itemSlotui.item;    // 바뀔 아이템
        
                    if (temp_arti != null)
                    {
                        DragSlot.instance.dragSlot.itemSlotui.item = temp_arti;
                    }
                    else
                    {
                        DragSlot.instance.dragSlot.itemSlotui.item = null;
                        DragSlot.instance.dragSlot.itemSlotui.image.sprite=null;
                        DragSlot.instance.dragSlot.itemSlotui.image.gameObject.SetActive(false);
                        Inventory.instance.artifactUIs[DragSlot.instance.dragSlot.number].isInstallation = false;
                    }
            }
            
        }
        
        DragSlot.instance.dragSlot =null;
    }
    
    public void taketime()
    {
        Debug.Log("a");
    }
}
