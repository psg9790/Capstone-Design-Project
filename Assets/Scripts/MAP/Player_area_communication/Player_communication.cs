using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Player_communication : MonoBehaviour
{
    public GameObject ListUI;
    
    public TMP_Text Text;
    
    private string objectTag;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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

    private void OnTriggerExit(Collider other)
    { 
        ListUI.SetActive(false);
        
    }
    
    
}
