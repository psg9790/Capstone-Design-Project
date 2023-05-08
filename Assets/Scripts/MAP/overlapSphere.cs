using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class overlapSphere : MonoBehaviour
{
    public float radius = 2.0f;

    public List<GameObject> dataList = new List<GameObject>();
    
    public RectTransform content;
    private GameObject Incontents;
    private GameObject ClearContents;
    private int MAX = 50;
    public List<ItemDataPractice> ItemData = new List<ItemDataPractice>();
    
    private void Start()
    {
    }

    private void Update()
    {
        //데이터 초기화하여 List<gameobject> 싹 비운 후 overlapsphere로 리스트 추가
        
        
        //radius를 기준으로 구 안에 있는 콜라이덛를 검출함
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        int count = 0;
        dataList.Clear();
        
        foreach (Collider col in colliders)
        {
            //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
            if (col.CompareTag("Item") )
            {
                Debug.Log(col.gameObject.name + "추가");
                dataList.Add(col.gameObject);
                Incontents = content.GetChild(count).gameObject;
                Incontents.SetActive(true);
                Incontents.GetComponentInChildren<TMP_Text>().text = dataList[count].GetComponent<FeildItem>().item.itemName;
                 
                count++;
            }
        }
        ClearContent(count);

        if (Input.GetKeyDown(KeyCode.F))
        {
            ItemData.Add(dataList[0].GetComponent<FeildItem>().item);
            Destroy(dataList[0].gameObject);
            UnityEngine.Debug.Log("Destroyed");
        }
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

    public void AddItem()
    {
        
        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        
        UnityEngine.Debug.Log("클릭된 버튼 번호: "+ clickButton.name);
        
        int buttonNum = Int32.Parse(clickButton.name);

        ItemDataPractice _item = dataList[0].GetComponent<FeildItem>().item;
        
        ItemData.Add(_item);
        
    }

    
}


    

