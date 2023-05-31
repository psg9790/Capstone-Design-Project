using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Rendering;

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

    public ISave Save;
    
    public UnityEvent itemChangedEvent;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            UUA_CO = StartCoroutine(UUA());
            
            #if StovePCSDK
                Save = new StoveSave();
#else
                Save = new LocalSave();
            #endif

        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("GameManager cannot be two : Deleted.");
        }

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        CheckItemsFileExists();
    }

    private void OnDestroy()
    {
        if (UUA_CO != null)
        {
            StopCoroutine(UUA_CO);
        }
    }

    private Coroutine UUA_CO;
    IEnumerator UUA()
    {
        while (true)
        {
            Resources.UnloadUnusedAssets();
            yield return new WaitForSeconds(20f);
        }
    }

    public void UpdateCanvasInterval(float time)
    {
        return;
        DOVirtual.DelayedCall(time, () =>
        {
            UpdateCanvas();
        });
    }

    private void UpdateCanvas()
    {
        CanvasScaler[] scalers = FindObjectsByType<CanvasScaler>(FindObjectsSortMode.None);
        // Debug.Log("scalers: "+scalers.Length);
        for (int i = 0; i < scalers.Length; i++)
        {
            scalers[i].uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Resolution scale = Screen.currentResolution;
            scalers[i].referenceResolution = new Vector2(scale.width, scale.height);
            scalers[i].matchWidthOrHeight = 0.5f;
        }
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
    
    private string coinFilePath = "";
    private void CheckCoinFileExists()
    {
        coinFilePath = Application.persistentDataPath + "/coin.json";
        if (!File.Exists(coinFilePath))
        {
            JObject newCoinFile = new JObject(new JProperty("Count", 0));
            File.WriteAllText(coinFilePath, newCoinFile.ToString());
        }
    }

    public int GetCoin()
    {
        CheckCoinFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(coinFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject coinJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(coinJson["Count"].ToString());
            }
        }

        return ret;
    }
    public void AddCoin(int amount)
    {
        CheckCoinFileExists();
        JObject coinJson;
        using (StreamReader file = File.OpenText(coinFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                coinJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(coinJson["Count"].ToString());
                curCount += amount;
                coinJson["Count"] = curCount;
            }
        }
        File.WriteAllText(coinFilePath, coinJson.ToString());
    }

    public void UseCoin()
    {
        CheckCoinFileExists();
        JObject coinJson;
        using (StreamReader file = File.OpenText(coinFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                coinJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(coinJson["Count"].ToString());
                curCount--;
                coinJson["Count"] = curCount;
            }
        }
        File.WriteAllText(coinFilePath, coinJson.ToString());
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
        
        if (GetCoin() <= 0) // 주사위가 충분하지 않은 경우
        {
            UnityEngine.Debug.Log("주사위가 충분하지 않습니다.");
            return;
        }
        
        UseCoin();

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