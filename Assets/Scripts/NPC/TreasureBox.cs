using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureBox : MonoBehaviour
{
    public bool isEnable;  //보물상자 영역에 플레이어가 있는지 확인
    public float rotSpeed =100; //열리는 속도
    private bool isActived=false; //보물상자가 오픈되었었는지 확인
    public GameObject Lid ; //보물상자 뚜껑부분
    public ParticleSystem openEffect; //보물상자 이펙트
    private GameObject dropPostion;//드랍 위치
    public ItemGenerator generator; //아이템 생성 
    

    void Start()
    {
        Lid =transform.GetChild(1).gameObject;
        dropPostion = transform.GetChild(3).gameObject;
        generator = ItemGenerator.Instance;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isEnable && isActived!=true)
        {            
            openEffect.Play();
            int random = Random.Range(2, 7);
            StartCoroutine(dropItems());
            Lid.transform.Rotate(new Vector3(90 ,0,0));
            isActived = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
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
    }

    //보물상자 오픈시 랜덤 갯수의 아이템 드랍
    IEnumerator dropItems()
    {
       
        yield return  new WaitForSeconds(0.2f);

        int random = Random.Range(2, 5);
        for(int i =0; i< random; i++)  generator.GenerateItem(dropPostion.transform, 1);
    }
}
