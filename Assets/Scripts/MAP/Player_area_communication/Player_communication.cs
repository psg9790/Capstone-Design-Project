using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Player_communication : MonoBehaviour
{
    public GameObject ListUI;
    
    public TMP_Text Text;
    public Text text;
    private string objectTag;

    //overlap sphere 사용하기 위한것
    public Collider[] colliders;
    public float radius = 1.0f;
    List<string> dataList = new List < string>();
    private TMP_Text tmp;
    public RectTransform content;
    public GameObject listItemPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
     void OnTriggerEnter(Collider other)
    {
        
        //radius를 기준으로 구 안에 있는 콜라이덛를 검출함
        colliders =
            Physics.OverlapSphere(this.transform.position, radius);
        foreach (Collider col in colliders)
        {
            //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
            if (col.CompareTag("Treasure"))
            {
                dataList.Add("treasure");
            }else if (col.CompareTag("NPC"))
            {
                dataList.Add("NPC");
            }else if (col.CompareTag("Item"))
            {
                dataList.Add("NPC");
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            GameObject item = Instantiate(listItemPrefab,content);
            //tmp = item.GetComponentInChildren<Text>();
            //tmp.text= dataList[i];
            
            
        }
    }
    
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            ListUI.SetActive(true);
            Text.text = "talk - F key";
        }else if (other.CompareTag("Treasure"))
        {
            ListUI.SetActive(true);
            Text.text = "open - F key";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //상호작용 중에는 NAV를 꺼둬서 이걸로 인식시킴
        if (GetComponent<Move>().enabled == false)
        {
            
            ListUI.SetActive(false);
            
        }
        else
        {
            ListUI.SetActive(true);
        }
        
    }
    */

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            Destroy(content.transform.GetChild(0).gameObject);
        }
        colliders =
            Physics.OverlapSphere(this.transform.position, radius);
        foreach (Collider col in colliders)
        {
            //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
            if (col.CompareTag("Treasure"))
            {
                dataList.Add("treasure");
            }else if (col.CompareTag("NPC"))
            {
                dataList.Add("NPC");
            }else if (col.CompareTag("Item"))
            {
                dataList.Add("NPC");
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            
            GameObject item = Instantiate(listItemPrefab,content);
            item.GetComponentInChildren<Text>().text=dataList[i];
            
        }
    }
    
    
}
