using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player_communication : MonoBehaviour
{
    /*
    //overlap sphere 사용하기 위한것
    public Collider[] colliders;
    public float radius = 1.0f;
    */
     
    public List<GameObject> dataList = new List <GameObject>();
    
    public RectTransform content;
    public GameObject listItemPrefab;

    private Transform[] childList;
    
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
        {
            dataList.Add(other.transform.gameObject);
            StartCoroutine(Scanner());
            
        }
        
    }
     
     void OnTriggerExit(Collider other)
     {
         
         if (other.CompareTag("Treasure") || other.CompareTag("Item") || other.CompareTag("NPC"))
         {
             
             scanInit();
             ShowData();
             
         }
             
     }

     void scanInit()
     {
         int count= content.transform.childCount;
         
         
             Debug.Log("리스트 제거:" +count);
             for (int i = 1; i < count; i++)
             {
                 Destroy(content.transform.GetChild(1).gameObject);
             }
         

         int dataCount = dataList.Count;
         if (dataCount != 0)
         {
             Debug.Log("데이터 제거:" +dataCount);
             dataList.RemoveRange(0, dataCount);
         }
     }

     void ShowData()
     {
         int i = 0;
         for (i = 0; i < dataList.Count; i++)
         {
             
             GameObject item = Instantiate(listItemPrefab, content);
             TMP_Text insert = item.GetComponentInChildren<TMP_Text>();
             Debug.Log("리스트생성");
             insert.text = dataList[i].tag+" 줍기" ;
         }
         
     }
     
     /*
     void scan()
     {
         scanInit();
         //radius를 기준으로 구 안에 있는 콜라이덛를 검출함
         colliders =
             Physics.OverlapSphere(transform.position, radius);
         foreach (Collider col in colliders)
         {
             //콜라이더의 테그를 인식하여 이에 맞는 표현 보이기
             if (col.CompareTag("Treasure") || col.CompareTag("Item") || col.CompareTag("NPC"))
             {
                 dataList.Add(col.gameObject);
             }
         }
         
     }
     */

     
     IEnumerator Scanner()
     {
       
         yield return  new WaitForSeconds(0.5f);
         
         yield return  new WaitForSeconds(0.2f);
         
         yield return  new WaitForSeconds(0.2f);
         
     }

     
     /*
     private void OnDrawGizmosSelected()
     {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, radius);
     }
     */
}
