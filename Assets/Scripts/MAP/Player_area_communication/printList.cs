using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class printList: MonoBehaviour
{
    public GameObject Player;
    private List<GameObject> List = new List<GameObject>();
    public RectTransform content;
    public GameObject listItemPrefab;

    public void showData(List<GameObject> dataList)
    {
        List= Player.GetComponent<Player_communication>().dataList;
       
       for (int i = 0; i < List.Count; i++)
       {
           GameObject item = Instantiate(listItemPrefab, content);
           TMP_Text Insert = item.GetComponentInChildren<TMP_Text>();
           Insert.text = List[i].tag;
       }
    }

    public void InitList(List<GameObject> dataList)
    {
        
        int count= content.transform.childCount;
        //init
        if (count != 1)
        {
            for (int i = 0; i < count; i++)
            {
                Destroy(content.transform.GetChild(1).gameObject);
            }
        }

        int listCount = dataList.Count;
        
        for (int i = 0; i < listCount; i++)
            {
                dataList.RemoveAt(0);
            }
        
        }
    
}

