using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    private void Awake()
    {
        instance=this;
    }
    public List<ItemDataPractice> itemDB = new List<ItemDataPractice>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos;
    
    private void Start()
    {
        Vector3 pos = this.transform.position;
        for (int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos, Quaternion.identity);
            go.GetComponent<FeildItem>().SetItem(itemDB[Random.Range(0,3)]);
            
        }    
    }
}
 