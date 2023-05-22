using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{

    public GameObject TreasureBox;
    static public TreasureManager instance;

    private void Start()
    {
        TreasureBox = GameObject.Find("treasure");

    }

    void Update()
    {

}
    
}
