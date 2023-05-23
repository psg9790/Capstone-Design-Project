using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
// using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class overlapSphere : MonoBehaviour
{
    public float radius = 2.0f;

    public List<GameObject> dataList = new List<GameObject>();
    
    public RectTransform content;
    public GameObject Incontents;
    private GameObject ClearContents;
    private int MAX = 28;
    public Inventory inven;
    public int ClickNum;
    public bool clicked;
    public GameObject commu_bar;

    public Item GetItem;

    private void Start()
    {
        dataList.Add(commu_bar);
        dataList.Clear();
        commu_bar = GameObject.Find("commu_bar"); 
        content= GameObject.Find("Content").GetComponent<RectTransform>();
        inven = Inventory.instance;
        clicked = false;
        ClickNum = 100;
        
    }

    private void Update()
    {
        dataList.Clear();
        //데이터 초기화하여 List<gameobject> 싹 비운 후 overlapsphere로 리스트 추가
        //radius를 기준으로 구 안에 있는 콜라이덛를 검출함
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        int count = 0;
        
        foreach (Collider col in colliders)
        {
            //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
            if (col.CompareTag("Item") )
            {
                if (commu_bar.activeSelf == false)
                {
                    commu_bar.SetActive(true);
                }
                dataList.Add(col.gameObject);
                Incontents = content.GetChild(count).gameObject;
                Incontents.SetActive(true);
                TMP_Text name = content.GetChild(count).GetComponentInChildren<TMP_Text>();
                name.text = col.GetComponent<DroppedItem>().item.itemName;
                count++;
            }
            
        }
        if ( dataList.Count == 0)
        {
            if(commu_bar.activeSelf == true )
                commu_bar.SetActive(false);
        }
        ClearContent(count);
        
        if ( clicked && inven.IsEmpty() ) 
        {
            clicked = false;
            ClickItem(ClickNum);
        }
        
        //F키 입력 시 첫번 째 아이템 정보 옮기기
        if (Input.GetKeyDown(KeyCode.F) && dataList.Count != 0 && inven.IsEmpty()) 
        {
            ClickItem(0);
        }
        

    }

    void ClickItem(int num)
    {
        GetItem = dataList[num].GetComponent<DroppedItem>().item;
        Inventory.instance.AddItem(GetItem);
        Destroy(dataList[num].gameObject);
        
    }

    void ClearContent(int count)
    {
        for(int i= count;i<10;i++)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                content.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }
    }
    
    public void ClickEvent(int val)
    {

        clicked = true;
        ClickNum = val;
        UnityEngine.Debug.Log(ClickNum + "캐릭터에 인...직...");
        
    }
    
    
}


    

