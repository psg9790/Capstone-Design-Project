using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private bool isEnable;

    private string line;
    
    //바라보게 만들기
    private Vector3 targetPosition;
    //원래위치
    public Vector3 pos;
    public Animator anim;
    
    private void Start()
    {
        pos = transform.eulerAngles;

    }

    // Update is called once per frame
    void Update()
    {

        //F키를 통해 상호작용 시작.
        if (Input.GetKeyDown(KeyCode.F) && isEnable)
        {
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
        isEnable = true;
        //NPC가 캐릭터를 바라볼 위치 값 받음
        targetPosition = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z);
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
            
           transform.rotation= Quaternion.Euler(pos);
        }
    }

    void Text_next()
    {
        
        if (Input.GetKeyDown(KeyCode.R)&&isAction)
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


