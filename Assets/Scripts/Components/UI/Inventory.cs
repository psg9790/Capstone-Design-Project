using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
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
    [ShowInInspector] [SerializeField] public ArtifactUI[] artifactUIs;
    public Sprite[] grade_image;
    
    [SerializeField] private Transform slotParent; // 슬롯의 부모가 되는 곳을 담을 곳
    [SerializeField] public ItemSlot[] slots;
    public ItemSlot weaponSlot;
    public Image backImage;
    [ShowInInspector] public Item tempItem=null;
    public bool isInstallation=false;

    [SerializeField] private TMP_Text sideStatDisplayText;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
        artifactUIs = artifactParent.GetComponentsInChildren<ArtifactUI>();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(instance);
            EmptySlot();
            artifactNumbering();
       
            grade_image = new Sprite[6]; 
            this.gameObject.SetActive(false);
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // AddItem(items[0]);
        // AddItem(items[1]);
        
    }

    public void EmptySlot(){
        for (int i=0; i < 28; i++) {
            slots[i].itemSlotui.item = null;
            slots[i].number = i;
        }
        
    }

    public void AddItem(Item item) {
        if (IsEmpty())
        {
            items.Add(item);
            for (int i = 0; i<28; i++)
            {
                if (slots[i].itemSlotui.item == null)
                {
                    slots[i].itemSlotui.item = item;
                    switch (slots[i].itemSlotui.item.tier)
                    {
                        case 0:
                            slots[i].grade_Back.sprite = grade_image[0]; 
                            break;
                        case 1:
                            slots[i].grade_Back.sprite = grade_image[1]; 
                            break;
                        case 2:
                            slots[i].grade_Back.sprite = grade_image[2]; 
                            break;
                        case 3:
                            slots[i].grade_Back.sprite = grade_image[3] ; 
                            break;
                        case 4:
                            slots[i].grade_Back.sprite = grade_image[4]; 
                            break;
                        case 5:
                            slots[i].grade_Back.sprite = grade_image[5]; 
                            break;
                        case 6:
                            slots[i].grade_Back.sprite = grade_image[6]; 
                            break;
                    }
                    slots[i].grade_Back.gameObject.SetActive(true); 
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

    public bool IsEmpty()
    {
        if (items.Count < slots.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void removeItem(Item _item, ItemSlot itemSlot)
    {
        
        items.Remove(_item);
        itemSlot.grade_Back.gameObject.SetActive(false);
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

    public void SetSideStatDisplayText(string displayText)
    {
        if (sideStatDisplayText != null)
        {
            sideStatDisplayText.text = displayText;
        }
        else
        {
            Debug.LogError("sideStat 컴포넌트 부착 바람");
        }
    }
}
