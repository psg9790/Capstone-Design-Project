using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph.Serialization;

// 정확한 역할은 정해지지 않았음, 대신 GameManager로써 마스터 설정같은거 다 때려 넣을듯
// 일단 저장로직 인터페이스, 씬 이동할때 갈아끼울 씬로드매니저
public class GameManager : MonoBehaviour
{
    // singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    // public GameObject menuSet;
    // public GameObject invenSet;
    //
    /*
    [Header("# Player Info")] 
    public int Hp;
    public int MaxHp=100;
    */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("GameManager cannot be two : Deleted.");
        }

        Application.targetFrameRate = 60;
    }

    // void Update()
    // {
    //     //Sub Menu
    //     if (Input.GetButtonDown("Cancel") && (invenSet.activeSelf)==false)
    //     {
    //         if (menuSet.activeSelf)
    //         {
    //             Time.timeScale = 1;
    //             menuSet.SetActive(false);
    //         }
    //         else
    //         {
    //             Time.timeScale = 0;
    //             menuSet.SetActive(true); 
    //         }
    //     }
    // }

    private string diceFilePath = "Resources\\dice.json";
    [Button]
    public int EndOfGrowthDungeon(int worldLevel) // 성장형 던전 종료 시 주사위 획득
    {
        CheckDiceFileExists();
        ModifyDiceCount(worldLevel + 1);
        return GetCurrentDiceCount();
    }

    public void ModifyDiceCount(int amount) // 주사위 갯수 조정, 저장
    {
        CheckDiceFileExists();
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject diceJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(diceJson["diceCount"].ToString());
                curCount += amount;
                diceJson["diceCount"] = curCount;
                File.WriteAllText(diceFilePath, diceJson.ToString());
            }
        }  
    }
    
    public int GetCurrentDiceCount() // 현재 주사위를 몇개 보유중인지 확인
    {
        CheckDiceFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject diceJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(diceJson["diceCount"].ToString());
            }
        }

        return ret;
    }

    private void CheckDiceFileExists() // 주사위 저장 파일이 존재하는지 확인
    {
        if (!File.Exists(diceFilePath))
        {
            Debug.Log("주사위 파일 존재하지 않음, 생성");
            JObject newDiceFile = new JObject(new JProperty("diceCount", 0));
            File.WriteAllText(diceFilePath, newDiceFile.ToString());
        }
    }


    private string itemsFilePath = "Resources\\items.json";
    private void CheckItemFileExists()
    {
        if (!File.Exists(itemsFilePath))
        {
            Debug.Log("아이템 파일 존재하지 않음, 생성");
            JItemList newList = new JItemList();
            for (int i = 0; i < 6; i++)
            {
                newList.items.Add(null);
            }
            JsonConvert.SerializeObject(newList);
        }
    }
}

[Serializable]
public class JItemList
{
    public List<Artifact> items = new List<Artifact>();
}