using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map_Switch : MonoBehaviour
{
    
    //선택창
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GrowthLevelManager.Instance.();
     q   }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            
        }
    }
}
