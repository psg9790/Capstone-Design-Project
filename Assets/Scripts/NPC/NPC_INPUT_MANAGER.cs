using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        talkData.Add(1, new string[]{"오 심상치 않은데?"});
        talkData.Add(2,new string[]{"중요한 건 꺾이지 않은 마음"});
        talkData.Add(3,new string[]{"딸깍... 딸깍... 캐리.... 기모리~"});
        talkData.Add(4,new string[]{"넣을게~"});
        talkData.Add(5,new string[]{"딸깍... 딸깍... 캐리.... 기모리~"});

    }

    public string GetTalk(int id, int talkIndex) //Object의 id , string배열의 index
    {
        return talkData[id][talkIndex]; //해당 아이디의 해당
    }
}
