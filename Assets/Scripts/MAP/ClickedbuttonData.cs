using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedbuttonData : MonoBehaviour
{
    public ButtonManager manager;
    
    public int num;

    private void Start()
    {
        num = Int32.Parse(gameObject.name);
    }

    public void clicked()
    {
        manager.clickLink(num);
    }
    
    

}
