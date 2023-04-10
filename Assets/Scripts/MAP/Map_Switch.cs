using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map_Switch : MonoBehaviour
{
    
    //선택창
    public GameObject portalUI;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            portalUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            portalUI.SetActive(false);
        }
    }
}
