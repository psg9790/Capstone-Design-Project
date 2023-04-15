using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    private bool isEnable;

    public float rotSpeed =100;

    private bool isActived=false;

    private GameObject Lid ;
    
    // Start is called before the first frame update
    void Start()
    {
        Lid =transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isEnable && isActived!=true)
        {
            Lid.transform.Rotate(new Vector3(90 ,0,0));
            isActived = true;
            Lid.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") )
        {
            isEnable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnable = false;
        }

        if (isActived)
        {
            Lid.transform.Rotate(-90,0,0);
            isActived = false;
            Lid.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    
}
