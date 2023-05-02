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
    List<GameObject> dataList = new List <GameObject>();
    
    private TMP_Text tmp;
    public RectTransform content;
    public GameObject listItemPrefab;

    private Transform[] childList;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
        {
            scan(other);
        }
    }

     void scan(Collider other)
     {
         
         for (int i = 0; i < content.transform.childCount; i++)
         {
             Debug.Log(content.transform.childCount +"삭제");
             Destroy(content.GetChild(0).gameObject);
             dataList.RemoveAt(0);
         }
         
         //radius를 기준으로 구 안에 있는 콜라이덛를 검출함
         colliders =
             Physics.OverlapSphere(this.transform.position, radius);
         
         foreach (Collider col in colliders)
         {
             //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
             if (col.CompareTag("Treasure") || col.CompareTag("Item") || col.CompareTag("NPC"))
             {
                 dataList.Add(other.gameObject);
             }
             
         }

         for (int i = 0; i < dataList.Count; i++)
         {
             GameObject item = Instantiate(listItemPrefab, content);
             TMP_Text Insert = item.GetComponentInChildren<TMP_Text>();
             Insert.text = dataList[i].tag;
         }
     }

     void OnTriggerExit(Collider other)
         {
             if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
             {
                 scan(other);
             }
             
         }
}
