using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map_Switch : MonoBehaviour
{
    //이동할 던전 씬
    public string transferMapName;
    //선택창
    public GameObject portalUI;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            portalUI.SetActive(true);
        }
    }
}
