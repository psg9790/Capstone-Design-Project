using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Interact : MonoBehaviour
{
    //미니맵카메라, 메인카메라로 두 카메라 있기 때문에 이와 같이 만듦
    public Camera getCamera;
    
    private RaycastHit hit;
    public GameObject NPC_Text;
    public TMP_Text NPC_NAME;
    public TMP_Text NPC_TELL;
    public GameObject NPC_Select1; 
    public GameObject NPC_Select2; 
    public TMP_Text NPC_Select1_Text;
    public TMP_Text NPC_Select2_Text;
    
    public NPC_data talkManager;
    //몇번째 대화가 이어지는지 저장하는 변수
    public int talkIndex;
    private NPC_INPUT_MANAGER manager;
    private NPC_data data;
    private bool isAction;

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            NPC_Text.SetActive(false);
        }
    }
    private void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 포인터로 레이저 생성
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
            
            //raycast(입력, 출력): 레이저 받았을 때 리턴값을 t,f로 주기 위함. 
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag=="NPC")
            {
                isAction = true;
                //레이저 맞은 오브젝트 이름 받음
                string objectName = hit.collider.gameObject.name;
                talkIndex = 0;
                // 최적화 필요할 듯
                manager = GameObject.Find("NPC_Manager").GetComponent<NPC_INPUT_MANAGER>();
                //getcomponent: 오브젝트에 담긴 스크립트 가져올 수 있다.
                data = hit.transform.gameObject.GetComponent<NPC_data>();
                string line = manager.GetTalk(data.id,talkIndex);
                NPC_NAME.text = objectName;
                NPC_TELL.text = line;
                NPC_Text.SetActive(true);
                
                if (data.select1==true)
                {
                    NPC_Select1.SetActive(true);
                    if(data.select2==true)
                    {
                        NPC_Select2.SetActive(true);
                    }else
                    {
                        NPC_Select2.SetActive(false);
                    }
                }else{
                    NPC_Select1.SetActive(false);
                    NPC_Select2.SetActive(false);
                }
                
            }
            
        
        }

        Text_next();
        
    }

    void Text_next()
    {
        
        if (Input.GetKeyDown(KeyCode.Space)&&isAction!=false)
        {
            talkIndex++;
            string line = manager.GetTalk(data.id, talkIndex);
            if (line != null)
            {
                NPC_TELL.text=line;
                
            }
            else
            {
                isAction = false;
            }
        }
    }
    
    public void Text_Exit()
    {
        NPC_Text.SetActive(false);
    }
    
}
