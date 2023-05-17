using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactSlot : ItemSlot
{
    private Item artiItem;
    public Artifact arti;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            artiItem = DragSlot.instance.dragSlot.itemSlotui.item;
            if (artiItem!=null)
            {
                Color color = Inventory.instance.artifactUIs[number].itemSlot.image.color;
                color.a = 0;
                Inventory.instance.artifactUIs[number].itemSlot.image.color = color;
                Inventory.instance.AddItem(artiItem);
                Inventory.instance.artifactUIs[number].isInstallation = false;           
                DragSlot.instance.dragSlot.itemSlotui.item= null;    

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
        }
       
        Inventory.instance.artifactUIs[number].isInstallation = false;
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot.itemSlotui.item is Artifact && Inventory.instance.isInstallation==true)
        {

            if (Inventory.instance.artifactUIs[number].isInstallation==true)
            {
                Inventory.instance.AddItem(itemSlotui.item);
            }

            Inventory.instance.artifactUIs[number].itemSlot.itemSlotui.image.sprite = DragSlot.instance.dragSlot.itemSlotui.image.sprite;
            Inventory.instance.artifactUIs[number].itemSlot.itemSlotui.item =  DragSlot.instance.dragSlot.itemSlotui.item;
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);
            Inventory.instance.artifactUIs[number].isInstallation = true;
            
        }
        
        DragSlot.instance.dragSlot =null;
    }
}
