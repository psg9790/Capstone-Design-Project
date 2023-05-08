using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureBox : MonoBehaviour
{
    public bool isEnable;
    public float rotSpeed =100;
    
    private bool isActived=false;
    public GameObject Lid ;
    public ParticleSystem openEffect;

    public GameObject itemPrefeb;
    private GameObject dropPostion;
    private int random;
    
    // Start is called before the first frame update
    void Start()
    {
        Lid =transform.GetChild(1).gameObject;
        dropPostion = transform.GetChild(3).gameObject;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isEnable && isActived!=true)
        {            
            openEffect.Play();
            random = Random.Range(2, 7);
            StartCoroutine(dropItems());
            Lid.transform.Rotate(new Vector3(90 ,0,0));
            isActived = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("enter");
            isEnable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnable = false;
        }
        /*
        if (isActived!=true)
        {
            //Lid.transform.Rotate(-90,0,0);
            isActived = false;
            //Lid.transform.GetChild(0).gameObject.SetActive(false);
        }*/
    }

    
    IEnumerator dropItems()
    {
       
        yield return  new WaitForSeconds(0.2f);

        int random = Random.Range(2, 100);
        Debug.Log("drop item: "+random);
        drop();
    }

    private void drop()
    {
        var itemGo = Instantiate<GameObject>(this.itemPrefeb);
        Vector3 vec3 = dropPostion.transform.position;
        vec3 += new Vector3(0.0f, 0.0f, 0.0f);
        itemGo.transform.position = vec3;
    }
    
}
