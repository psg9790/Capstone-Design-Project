using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactUI : MonoBehaviour
{
    public Image lockImage;
    public bool isInstallation=false;
    public ItemSlot itemSlot;
    public int artiNumber;
    
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragSlot.instance.dragSlot = this;
            if (isInstallation == true)
            {
                itemSlot.itemSlotui.image.sprite = null;
                itemSlot.itemSlotui.gameObject.SetActive(false);
                Inventory.instance.AddItem(itemSlot.itemSlotui.item);      
                DragArtiSlot.instance.dragArtiSlot.isInstallation = false;
            }
        }
        DragSlot.instance.dragSlot = null;
    }
    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlotui.item != null)                     // 아티팩트UI가 널이 아니면 
        {
            DragSlot.instance.beginSlot = 2;
            DragArtiSlot.instance.dragArtiSlot = this;
            DragArtiSlot.instance.DragSetImage(itemSlotui.image);      // 아티팩트 이미지 설정.

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlotui.item != null)                    // 아티팩트가 null이 아니면
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Inventory.instance.removeItem(DragSlot.instance.dragSlot.itemSlotui.item, DragSlot.instance.dragSlot);       // 이것도 바꿔야 함.
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot.itemSlotui.item is Artifact )
        {
            
        }
    }
    */
}
