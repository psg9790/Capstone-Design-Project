using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class overlapSphere : MonoBehaviour
{
    public float radius = 2.0f;

    public List<GameObject> dataList = new List<GameObject>();
    public RectTransform content;
    private GameObject Incontents;
    private GameObject ClearContents;
    private int MAX = 50;
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
            if (col.CompareTag("Treasure") || col.CompareTag("Item") || col.CompareTag("NPC"))
            {
                Debug.Log(col.gameObject.name + "추가");
                dataList.Add(col.gameObject);
                
                Incontents = content.GetChild(count).gameObject;
                Incontents.SetActive(true);
                Incontents.GetComponentInChildren<TMP_Text>().text = dataList[count].name;
                count++;
                
            }
        }
        ClearContent(count);

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
}

/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
        {
            //목록 생성 후 데이터 추가
            GenerateContent();
            AddData();
        }
    }

    public void OnTriggerExit (Collider other)
    {
        //콜라이더 하나 벗어나면 datalist 초기화
        if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
        {
            //목록창 하나 제거 후 업데이트
            DeleteContent();
            UpdateData();
        }
            
    }

    void InitData()
    {
        //데이터 리스트 초기화
        int i ;
        int dataListCount = dataList.Count;
        UnityEngine.Debug.Log("리스트 갯수: "+dataListCount);
        dataList.Clear();
        UnityEngine.Debug.Log("리스트 제거 후 남은 갯수: "+ dataList.Count);
    }

    
    //콘텐츠 아래 목록들 보이도록 생성
    void GenerateContent()
    {
        //ㅇ소 생성
            GameObject Item = Instantiate(ListItemPrefab, content);
            /*
            Item.GetComponentInChildren<TMP_Text>().text = dataList[i].name;
            UnityEngine.Debug.Log("생성된 목록: " + Item.name);
            
            
    } */
    /*
    //목록에 데이터 추가
    void AddData()
    {
        int ListCount = content.childCount;
        for (int i = 0; i < ListCount; i++)
        {
            TMP_Text insert = content.GetChild(i).GetComponentInChildren<TMP_Text>();
            if (dataList.Count > i ) insert.text = dataList[i].name;
        }
    }
    void UpdateData()
    {
        int ContentCount = content.childCount;
        int dataListCount = dataList.Count;
        
        if (ContentCount > 0 )
        {
            for (int i = 0; i < ContentCount; i++)
            {
                if (dataListCount> i && content.GetChild(i) != null) 
                {
                    TMP_Text insert = content.GetChild(i).GetComponentInChildren<TMP_Text>();
                    insert.text = dataList[i].name;
                }
                else if (dataList.Count <= i && content.GetChild(i) != null) 
                {
                    Destroy(content.GetChild(i).gameObject);
                }
                
            }
        }
    }    
    
    void DeleteContent()
    {
        int ContentCount = content.childCount;
        Debug.Log("content 자식 수:" + ContentCount);
        if (ContentCount > 0)
        {
            Destroy(content.GetChild(0).gameObject);
        }
    }
    */
    

