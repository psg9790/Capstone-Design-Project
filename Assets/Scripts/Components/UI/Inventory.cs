using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    [ShowInInspector] public List<Item> items = new List<Item>(); // 아이템 배열
    [AssetList(Path = "/Resources/ItemData/")]
    public List<ItemData> itemDatas = new List<ItemData>();
    [SerializeField] public Transform artifactParent; // 슬롯의 부모가 되는 곳을 담을 곳
    [SerializeField] public ArtifactUI[] artifactUIs;
    
    
    [SerializeField] private Transform slotParent; // 슬롯의 부모가 되는 곳을 담을 곳
    [SerializeField] private ItemSlot[] slots;
    public ItemSlot weaponSlot;
    public Image backImage;
    public Item tempItem;
    public bool isInstallation=false;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
        artifactUIs = artifactParent.GetComponentsInChildren<ArtifactUI>();
    }

    void Awake()
    {
        EmptySlot();
        artifactNumbering();
    }

    private void Start()
    {
        // AddItem(items[0]);
        // AddItem(items[1]);
        instance = this;
    }

    public void EmptySlot(){
        for (int i=0; i < 28; i++) {
            slots[i].itemSlotui.item = null;
            slots[i].number = i;
        }
        
    }

    public void AddItem(Item item) {
        if (items.Count < slots.Length)
        {
            items.Add(item);
            for (int i = 0; i<28; i++)
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

    public void removeItem(Item _item, ItemSlot itemSlot)
    {
        
        items.Remove(_item);
        itemSlot.itemSlotui.item = null;
        itemSlot.itemSlotui.image.sprite= null;
        itemSlot.itemSlotui.gameObject.SetActive(false);
    }

    [Button]
    public void DEBUG_AddRandomItem()
    {
        int idx = Random.Range(0, itemDatas.Count);
        AddItem(new Item(itemDatas[idx], 0, 0));
    }

    public void artifactNumbering()
    {
        for (int i = 0; i < 6; i++) 
        {
            artifactUIs[i].artiNumber = i;
            artifactUIs[i].itemSlot.number = i;
        }
    }
}
