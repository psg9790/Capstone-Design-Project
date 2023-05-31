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

    public List<GameObject> dataList = new List<GameObject>(); //스캔한 오브젝트 중 아이템만 수집
    public RectTransform content;   // UI  
    public GameObject Incontents;   // UI 중 리스트
    public Inventory inven; //인벤토리
    public int ClickNum;    // 클릭된 리스트의 번호
    public bool clicked;    //클릭 확인
    public GameObject commu_bar;   //UI on off 
    public Item GetItem;    //클릭한 아이템 저장
    private bool isPopup=false;
    
    
    private void Start()
    {
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
            if (col.CompareTag("Item") &&count <=45)
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
        }else if (clicked == true && inven.IsEmpty() ==false)
        {
            clicked = false;
        }
        
        //F키 입력 시 첫번 째 아이템 정보 옮기기
        if (Input.GetKeyDown(KeyCode.F) && dataList.Count != 0 && inven.IsEmpty()) 
        {
            ClickItem(0);
        } else if ((Input.GetKeyDown(KeyCode.F) && !(inven.IsEmpty()) && isPopup == false))
        {
            isPopup = true;
            
            Inventory.instance.popUp.text="슬롯이 가득 차 있습니다.";
            Inventory.instance.popUp.gameObject.SetActive(true);
            Invoke("popupHide", 1.0f);
        }
        

    }

    //클릭한 아이템 저장
    void ClickItem(int num)
    {
        GetItem = dataList[num].GetComponent<DroppedItem>().item;
        Inventory.instance.AddItem(GetItem);
        Destroy(dataList[num].gameObject);
        
    }

    // 아이템 갯수에 맞춰 content 켜짐 
    void ClearContent(int count)
    {
        for(int i= count;i<=45;i++)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                content.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }
    }
    
    //클릭시 이벤트
    public void ClickEvent(int val)
    {

        clicked = true;
        ClickNum = val;
    }
    
    public void popupHide()
    {
        Inventory.instance.popUp.gameObject.SetActive(false);
        isPopup = false;
    }
    
}


    

