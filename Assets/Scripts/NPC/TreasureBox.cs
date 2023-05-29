using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening; //dotween을 통해 rotate 자연스럽게 변경

public class TreasureBox : MonoBehaviour
{
    public bool isEnable;  //보물상자 영역에 플레이어가 있는지 확인
    public float rotSpeed =100; //열리는 속도
    private bool isActived=false; //보물상자가 오픈되었었는지 확인
    public GameObject Lid ; //보물상자 뚜껑부분
    public ParticleSystem openEffect; //보물상자 이펙트
    private GameObject dropPostion;//드랍 위치
    public ItemGenerator generator; //아이템 생성 
    

    void Start() //보물상자 뚜껑, drop위치, 제너레이터, 이펙트 init
    {
        Lid =transform.GetChild(1).gameObject;
        dropPostion = transform.GetChild(3).gameObject;
        generator = ItemGenerator.Instance;
        openEffect.Stop();
    }
    
    void Update()//f키를 누를 시 이펙트 시작, 드랍 아이템 생성,뚜껑 위치 전환, 상자 열림 확인
    {
        if (Input.GetKeyDown(KeyCode.F) && isEnable && isActived!=true)
        {            
            openEffect.Play();
            StartCoroutine(dropItems());
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

    //보물상자 오픈시 랜덤 갯수의 아이템 드랍, 이펙트 3.5초 후 종료
    IEnumerator dropItems()
    {
        //float yVal = transform.rotation.y*-1f;
        //Lid.transform.DORotate(new Vector3(90, 0, 0), 1.5f);
        
        Lid.transform.Rotate(new Vector3(90 ,0,0));
        yield return  new WaitForSeconds(1.5f);

        int random = Random.Range(2, 5);
        for(int i =0; i< random; i++)  generator.GenerateItem(dropPostion.transform, 1);
        yield return new WaitForSeconds(1.0f);
        openEffect.Stop();
    }
}
