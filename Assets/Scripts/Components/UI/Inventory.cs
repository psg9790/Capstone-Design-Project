using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    public List<Item> items; // 아이템 배열

    [SerializeField] private Transform slotParent; // 슬롯의 부모가 되는 곳을 담을 곳
    [SerializeField] private ItemSlotUI[] slots;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlotUI>();
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
            slots[i].item = null;
        }
    }

    public void AddItem(Item item) {
        if (items.Count < slots.Length)
        {
            Debug.Log("a");
            items.Add(item);
            for (int i = 0; i<slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].item = item;
                    break;
                }
            }
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }

    public void removeItem(Item _item,ItemSlotUI _itemSlotUI)
    {
        items.Remove(_item);
        _itemSlotUI.item = null;
        _itemSlotUI.image.sprite= null;
    }
}
