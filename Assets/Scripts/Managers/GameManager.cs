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
using UnityEngine.Events;

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

    public UnityEvent itemChangedEvent;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            CheckDiceFileExists();
            CheckItemsFileExists();
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("GameManager cannot be two : Deleted.");
        }

        Application.targetFrameRate = 60;
    }

    public void Death_GrowthUI()
    {
        GameOverUI go = Instantiate(Resources.Load<GameOverUI>("UI/Canvas_GameOver"));
        go.Display_GrowthDungeonResult();
    }

    public void Death_RecordUI()
    {
        GameOverUI go = Instantiate(Resources.Load<GameOverUI>("UI/Canvas_GameOver"));
        go.Display_RecordDungeonResult();
    }
    

    [HideInInspector] public string diceFilePath;

    [Button]
    public int EndOfGrowthDungeon(int worldLevel) // 성장형 던전 종료 시 주사위 획득
    {
        CheckDiceFileExists();
        int diceEarn = (int)(worldLevel * 1.5f);
        ModifyDiceCount(diceEarn);
        ModifyMaxLevel(worldLevel);
        StovePCSDKManager.Instance.SetGrowthLevel(worldLevel);
        return diceEarn;
    }
    
    [Button]
    public bool EndOfRecordDungeon(int worldLevel) // 성장형 던전 종료 시 주사위 획득
    {
        CheckDiceFileExists();
        bool ret = GetCurrentRecord() < worldLevel;
        ModifyRecord(worldLevel);
        StovePCSDKManager.Instance.SetRecordLevel(worldLevel);
        return ret;
    }

    [Button]
    public void ModifyDiceCount(int amount) // 주사위 갯수 조정, 저장
    {
        CheckDiceFileExists();
        JObject diceJson;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                diceJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(diceJson["diceCount"].ToString());
                curCount += amount;
                diceJson["diceCount"] = curCount;
            }
        }
        File.WriteAllText(diceFilePath, diceJson.ToString());
    }

    void ModifyMaxLevel(int level)
    {
        CheckDiceFileExists();
        JObject diceJson;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                diceJson = (JObject)JToken.ReadFrom(reader);

                int curLevel = int.Parse(diceJson["maxLevel"].ToString());
                diceJson["maxLevel"] = Math.Max(curLevel, level);
            }
        }
        File.WriteAllText(diceFilePath, diceJson.ToString());
    }
    
    void ModifyRecord(int level)
    {
        CheckDiceFileExists();
        JObject diceJson;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                diceJson = (JObject)JToken.ReadFrom(reader);

                int curLevel = int.Parse(diceJson["record"].ToString());
                diceJson["record"] = Math.Max(curLevel, level);
            }
        }
        File.WriteAllText(diceFilePath, diceJson.ToString());
    }

    [Button]
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
    
    [Button]
    public int GetCurrentMaxLevel() // 현재 주사위를 몇개 보유중인지 확인
    {
        CheckDiceFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject diceJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(diceJson["maxLevel"].ToString());
            }
        }

        return ret;
    }
    
    [Button]
    public int GetCurrentRecord() // 현재 주사위를 몇개 보유중인지 확인
    {
        CheckDiceFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(diceFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject diceJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(diceJson["record"].ToString());
            }
        }

        return ret;
    }

    private void CheckDiceFileExists() // 주사위 저장 파일이 존재하는지 확인
    {
        diceFilePath = Application.persistentDataPath + "/dice.json";
        if (!File.Exists(diceFilePath))
        {
            Debug.Log("주사위 파일 존재하지 않음, 생성");
            JObject newDiceFile = new JObject(new JProperty("diceCount", 0));
            newDiceFile.Add(new JProperty("maxLevel", 0));
            newDiceFile.Add(new JProperty("record", 0));
            File.WriteAllText(diceFilePath, newDiceFile.ToString());
        }
    }


    [HideInInspector] public string itemsFilePath;
    public void CheckItemsFileExists()
    {
        itemsFilePath = Application.persistentDataPath + "/items.json";
        if (!File.Exists(itemsFilePath))
        {
            Debug.Log("아이템 파일 존재하지 않음, 생성");
            JItemsList newList = new JItemsList();
            // for (int i = 0; i < 6; i++)
            // {
            //     newList.items.Add(null);
            // }
            newList.items = ConvertArtifactToJArtifact(ItemGenerator.Instance.Generate6ItemsForChallenge());

            string result = JsonConvert.SerializeObject(newList, Formatting.Indented,
                new JsonSerializerSettings() {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
            File.WriteAllText(itemsFilePath, result);
        }
    }

    [Button]
    public void RerollItems()
    {
        CheckItemsFileExists();
        
        if (GetCurrentDiceCount() <= 0) // 주사위가 충분하지 않은 경우
        {
            UnityEngine.Debug.Log("주사위가 충분하지 않습니다.");
            return;
        }
        
        ModifyDiceCount(-1);

        string jsonfile = File.ReadAllText(itemsFilePath);
        JObject token = JObject.Parse(jsonfile);
        JItemsList newList = JsonConvert.DeserializeObject<JItemsList>(token.ToString());
        newList.items = ConvertArtifactToJArtifact(ItemGenerator.Instance.Generate6ItemsForChallenge());
        string result = JsonConvert.SerializeObject(newList, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        File.WriteAllText(itemsFilePath, result);
        itemChangedEvent.Invoke();
    }

    public void ChangeWeaponItemName(string name)
    {
        string jsonfile = File.ReadAllText(itemsFilePath);
        JObject token = JObject.Parse(jsonfile);
        JItemsList newList = JsonConvert.DeserializeObject<JItemsList>(token.ToString());
        newList.weapon.itemName = name;
        string result = JsonConvert.SerializeObject(newList, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        File.WriteAllText(itemsFilePath, result);
        itemChangedEvent.Invoke();
    }

    public JItemsList GetJItems()
    {
        CheckItemsFileExists();
        string jsonfile = File.ReadAllText(itemsFilePath);
        JObject token = JObject.Parse(jsonfile);
        JItemsList newList = JsonConvert.DeserializeObject<JItemsList>(token.ToString());
        return newList;
    }

    public void RemoveItemsJson()
    {
        File.Delete(itemsFilePath);
    }

    private List<JArtifact> ConvertArtifactToJArtifact(List<Artifact> input)
    {
        List<JArtifact> output = new List<JArtifact>();

        for (int i = 0; i < input.Count; i++)
        {
            JArtifact token = new JArtifact();
            token.itemName = input[i].itemName;
            token.itemTier = input[i].tier;
            token.itemOptions = input[i].options;
            output.Add(token);
        }

        return output;
    }
}

[Serializable]
public class JItemsList
{
    public JWeapon weapon = new JWeapon();
    public List<JArtifact> items = new List<JArtifact>();
}

[Serializable]
public class JWeapon
{
    public string itemName = "검";
    public float atk = 50f;
    public float atkspeed = 1f;
    public float critrate = 5f;
    public float critdamage = 15f;
    public int socket = 6;
}

[Serializable]
public class JArtifact
{
    public string itemName;
    public int itemTier;
    public Dictionary<ArtifactKey, float> itemOptions;
}