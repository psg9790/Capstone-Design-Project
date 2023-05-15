using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC_Trigger : MonoBehaviour
{
    public GameObject NPC_Text;
    public TMP_Text NPC_NAME;
    public TMP_Text NPC_TELL;
    public GameObject NPC_Select1; 
    public GameObject NPC_Select2; 
    public TMP_Text NPC_Select1_Text;
    public TMP_Text NPC_Select2_Text;
    
    //몇번째 대화가 이어지는지 저장하는 변수
    public int talkIndex;
    public NPC_INPUT_MANAGER manager;
    private NPC_data data;
    public bool isAction;

    //영역안에 있어야만 상호작용 할 수 있도록 함.
    private bool isEnable;
    //말하는 중 인식
    private bool isTalking;
    private string line;
    
    //바라보게 만들기
    private Vector3 targetPosition;
    //원래위치
    public Vector3 pos;
    public Animator anim;

    public GameObject PLAYER;
    //private Move MoveEnable;
    
    private void Start()
    {
        pos = transform.eulerAngles;
        isTalking = false;
        isEnable = false;
        //MoveEnable = PLAYER.GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //F키를 통해 상호작용 시작.
        if (Input.GetKeyDown(KeyCode.F) && isEnable&&isTalking!=true)
        {
            //캐릭터 마우스 이동 OFF
            //MoveEnable.enabled = false;
            //캐릭터 중지시킴
            PLAYER.GetComponent<NavMeshAgent>().enabled = false;
            //대화시작시 F로 대화 다시 시작 방지
            isTalking = true;
            //캐릭터를 바라봄
            transform.LookAt(targetPosition);
            //모션 변경
            anim.SetTrigger("dismissing");
            //대화창 시작
            talkIndex = 0;
            isAction = true;
            NPC_Text.SetActive(true);
            data = GetComponent<NPC_data>();

             line = manager.GetTalk(data.id, talkIndex);
             
            NPC_NAME.text = data.NPCName;
            NPC_TELL.text = line;
            NPC_Select1.SetActive(false);
            NPC_Select2.SetActive(false);
            

        }
        
        Text_next();
        
        
    }

    private void OnTriggerStay(Collider col)
{
    if (col.CompareTag("Player") )
    {
        Debug.Log("enter");
        isEnable = true;
        
        //NPC가 캐릭터를 바라볼 위치 값 받음
        targetPosition = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z);
    }

    if (NPC_Text.activeSelf == false )
    {
        isTalking = false;
        //MoveEnable.enabled = true;
        PLAYER.GetComponent<NavMeshAgent>().enabled = true;
    }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPC_Text.SetActive(false);
            NPC_Select1.SetActive(false);
            NPC_Select2.SetActive(false);
            isEnable = false;
            isTalking = false; 
           transform.rotation= Quaternion.Euler(pos);
           
        }
        
    }

    void Text_next()
    {
        
        if (Input.GetKeyDown(KeyCode.F)&&isAction)
        {
            talkIndex++;
            line = manager.GetTalk(data.id, talkIndex);
            
            if (line != null)
            {
                NPC_TELL.text=line;
                
            }
            
            else
            {
                isAction = false;
                if (data.select1 )
                {
                    NPC_Select1.SetActive(true);
                    if (data.select2 )
                    {
                        NPC_Select2.SetActive(true);
                    }
                    else
                    {
                        NPC_Select2.SetActive(false);
                    }
                }
                else
                {
                    NPC_Select1.SetActive(false);
                    NPC_Select2.SetActive(false);
                }
            }
        }
    }

    
}


