using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    public List<Item> items; // 아이템 배열

    [SerializeField] private Transform slotParent; // 슬롯의 부모가 되는 곳을 담을 곳
    [SerializeField] private ItemSlot[] slots;
    public Image weaponImage;
    public Image weaponBack;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
    }

    void Awake()
    {
        EmptySlot();
    }

    private void Start()
    {
        AddItem(items[0]);
        AddItem(items[1]);
        instance = this;
    }

    public void EmptySlot(){

        for (int i=0; i < slots.Length; i++) {
            slots[i].itemSlotui.item = null;
            
            Debug.Log(i);
        }
    }

    public void AddItem(Item item) {
        if (items.Count < slots.Length)
        {
            items.Add(item);
            for (int i = 0; i<slots.Length; i++)
            {
                if (slots[i].itemSlotui.item == null)
                {
                    slots[i].itemSlotui.item = item;
                    break;
                }
            }
        }
        else
        {
            Debug.Log(items.Count);
            Debug.Log(slots.Length);
            print("슬롯이 가득 차 있습니다.");
        }
    }

    public void removeItem(Item _item,ItemSlot itemSlot)
    {
        items.Remove(_item);
        itemSlot.itemSlotui.item = null;
        itemSlot.itemSlotui.image.sprite= null;
        itemSlot.itemSlotui.gameObject.SetActive(false);
    }
}
