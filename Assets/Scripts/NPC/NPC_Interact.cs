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
            
        }
        
    }
    
    public void Text_Exit()
    {
        NPC_Text.SetActive(false);
    }
    
}

/*
using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

public class TalkManager : MonoBehaviour
{
    NPCInfo npcOBJ;

    public float setTalkSpeed = 0.1f;
    WaitForSeconds talkSpeed;
    bool isTalkToAnyNPC = false;
    bool isTalking = false;
    IObservable<Unit> clickStream;

    void Start()
    {
        talkSpeed = new WaitForSeconds(setTalkSpeed);

        // 대화 스트림
        // 대화가능 상태에서 트리거버튼을 누르면 대화 시작
        // StopCoroutine으로 기존 대화중이던 내용 삭제
        clickStream = this.UpdateAsObservable().Where(_ => (Input_Z.Is_Down_Left_Trigger() || Input_Z.Is_Down_Right_Trigger() || Input.GetKeyDown("space")))
            .Publish() // Hot 변환
            .RefCount(); // Observer 추가시 자동 Connect

        clickStream
            .Where(_ => isTalkToAnyNPC && !isTalking)
            .Subscribe(_ => { StartCoroutine("TalkTalk"); });

        clickStream // 한번 클릭
            .Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(150)))
            .Where(x => x.Count < 2 && isTalking)
            .Subscribe(_ => isClicked = true);

        clickStream // 더블 클릭
            .Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(150)))
            .Where(x => x.Count >= 2 && isTalking)
            .Subscribe(_ => isDoubleClicked = true);
    }

    // 대화 클릭감지를 위한 변수들
    bool isClicked = false;
    bool isDoubleClicked = false;

    IEnumerator TalkTalk()
    {
        // 대화창 초기화
        npcOBJ.talkContent.text = "";
        npcOBJ.canvas_TalkWindow.SetActive(true);
        isTalking = true;
        int currContentNum = 0;

        while (true) // 할말없으면 break
        {
            yield return null;
            while (true) //한 마디가 끝나거나 더블클릭, 클릭 후 클릭하면 break
            {
                yield return null;
                if (isClicked || isDoubleClicked) break;
            }

            DoubleClicked:
            isClicked = false;
            isDoubleClicked = false;
            npcOBJ.talkContent.text = "";

            // 한글자씩 대화 출력
            // 클릭시 대화 전체출력
            // 더블클릭시 다음 대화로 이동
            foreach (char c in npcOBJ.talkContentList[currContentNum])
            {
                npcOBJ.talkContent.text += c;
                yield return talkSpeed;
                if (isDoubleClicked && currContentNum < (npcOBJ.talkContentList.Count -1))
                {
                    currContentNum++;
                    goto DoubleClicked;
                }
                if (isClicked || isDoubleClicked) break;
            }

            npcOBJ.talkContent.text = npcOBJ.talkContentList[currContentNum];

            // 한마디 끝, 클릭초기화 후 다음 이벤트 대기
            currContentNum++;
            isClicked = false;
            isDoubleClicked = false;
            if (currContentNum >= npcOBJ.talkContentList.Count) break;
        }

        while (true) // 씬 이동전 클릭대기
        {
            yield return null;
            if (isClicked || isDoubleClicked) break;
        }
        // 대화 후 추가적인 로직
    }

    public void InAreaNPC(NPCInfo _npcOBJ)
    {
        // NPC 앞으로 들어가면 true 나오면 false
        if (isTalkToAnyNPC == true) return;
        npcOBJ = _npcOBJ;

        isTalkToAnyNPC = true;
        npcOBJ.canvas_CanTalk.SetActive(true);
        // NPC 애니메이션 동작
    }

    public void OutAreaNPC(NPCInfo _npcOBJ)
    {
        // NPC 앞으로 들어가면 true 나오면 false
        if (isTalkToAnyNPC == false) return;
        npcOBJ = _npcOBJ;

        isTalkToAnyNPC = false;
        npcOBJ.canvas_CanTalk.SetActive(false);

        // 대화 초기화
        StopCoroutine("TalkTalk");
        npcOBJ.canvas_TalkWindow.SetActive(false);
        isTalking = false;
        isClicked = false;
        isDoubleClicked = false;
    }
}
*/