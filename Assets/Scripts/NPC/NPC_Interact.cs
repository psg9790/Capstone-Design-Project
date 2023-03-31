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
    
    public NPC_data talkManager;
    public int talkIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 포인터로 레이저 생성
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
            //raycast(입력, 출력): 레이저 받았을 때 리턴값을 t,f로 주기 위함. 
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag=="NPC")
            {
                //레이저 맞은 오브젝트 이름 받음
                string objectName = hit.collider.gameObject.name;
                
                // 최적화 필요할 듯
                NPC_INPUT_MANAGER manager = GameObject.Find("NPC_Manager").GetComponent<NPC_INPUT_MANAGER>();
                //getcomponent: 오브젝트에 담긴 스크립트 가져올 수 있다.
                NPC_data data = hit.transform.gameObject.GetComponent<NPC_data>();
                string line = manager.GetTalk(data.id, 0);
                NPC_NAME.text = objectName;
                NPC_TELL.text = line;
                
                NPC_Text.SetActive(true);
                
            }

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "NPC" &&
                hit.collider.gameObject.GetComponent<NPC_data>().select1 == true)
            {
                string objectName = hit.collider.gameObject.name;

                NPC_INPUT_MANAGER manager = GameObject.Find("NPC_Manager").GetComponent<NPC_INPUT_MANAGER>();

                NPC_data data = hit.transform.gameObject.GetComponent<NPC_data>();

                string line = manager.GetTalk(data.id, );
            }


        }
        
    }
    
    public void Text_Exit()
    {
        NPC_Text.SetActive(false);
    }
    
}
