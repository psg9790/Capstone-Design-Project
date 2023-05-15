using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject Player;
    public overlapSphere overlapSphere;

    
    private void Start()
    {
        
    }

    public void clickLink(int val)
    {
        
        Player = GameObject.Find("Player2(Clone)");
        UnityEngine.Debug.Log(Player.name + "인...직...");
        overlapSphere = Player.GetComponent<overlapSphere>();
        
        Player.GetComponent<overlapSphere>().ClickEvent(val);
        
    }

    
}
