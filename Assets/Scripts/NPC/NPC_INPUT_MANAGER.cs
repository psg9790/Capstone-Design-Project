using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NPC의 대사를 저장
public class NPC_INPUT_MANAGER : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        
        GenerateData();
    }

    void GenerateData()
    {
        //id = 1 : SIMSANG
        talkData.Add(1, new string[] { "1", "2","3","4" });
        talkData.Add(2, new string[] { "1", "2","3","4" });
        talkData.Add(3, new string[] {"1", "2","3","4" });
        talkData.Add(4, new string[] {"1", "2","3","4" });

        talkData.Add(6, new string[] { "창고입니다.","창고","창고"});
        talkData.Add(7, new string[] { "상점입니다.","구매", "판매" });
        talkData.Add(8, new string[]{"ab", "aa"});
    }

    public string GetTalk(int id, int talkIndex) //Object의 id , string배열의 index
    {
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }else
        {
            return talkData[id][talkIndex]; //해당 아이디의 해당
        }
        
    }

   
}
