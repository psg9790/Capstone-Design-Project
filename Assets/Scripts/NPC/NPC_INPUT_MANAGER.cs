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
        //id = 1000 : Talia
        talkData.Add(1, new string[]{"안녕","이 곳에 처음 왔구나?"});
        //id = 100 : 탈리아가 갇혀있는 Prision
        talkData.Add(2,new string[]{"쇠로 만들어진 감옥이다.","열쇠없이는 열 수 없는 것 같다."});
        //id = 200 : 다음 스테이지로 넘어갈 수 있는 문 
        talkData.Add(3,new string[]{"평범한 문이다. \n들어갈 수 있을 것 같다"});
    }

    public string GetTalk(int id, int talkIndex) //Object의 id , string배열의 index
    {
        return talkData[id][talkIndex]; //해당 아이디의 해당
    }
}
