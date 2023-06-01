using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalSave : ISave
{
    public string recordFilePath = "";
    private void CheckRecordFileExists()
    {
        recordFilePath = Application.persistentDataPath + "/record.json";
        if (!File.Exists(recordFilePath))
        {
            JObject newRecordFile = new JObject(new JProperty("Growth", 0));
            newRecordFile.Add(new JProperty("Record", 0));
            File.WriteAllText(recordFilePath, newRecordFile.ToString());
        }
    }
    public void SetGrowthLevel(int level)
    {
        CheckRecordFileExists();
        JObject recordJson;
        using (StreamReader file = File.OpenText(recordFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                recordJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(recordJson["Growth"].ToString());
                curCount = Math.Max(curCount, level);
                recordJson["Growth"] = curCount;
            }
        }
        File.WriteAllText(recordFilePath, recordJson.ToString());
    }

    public void SetRecordLevel(int level)
    {
        CheckRecordFileExists();
        JObject recordJson;
        using (StreamReader file = File.OpenText(recordFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                recordJson = (JObject)JToken.ReadFrom(reader);

                int curCount = int.Parse(recordJson["Record"].ToString());
                curCount = Math.Max(curCount, level);
                recordJson["Record"] = curCount;
            }
        }
        File.WriteAllText(recordFilePath, recordJson.ToString());
    }

    public int GetGrowthLevel()
    {
        CheckRecordFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(recordFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject recordJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(recordJson["Growth"].ToString());
            }
        }

        return ret;
    }

    public int GetRecordLevel()
    {
        CheckRecordFileExists();
        int ret = 0;
        using (StreamReader file = File.OpenText(recordFilePath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject recordJson = (JObject)JToken.ReadFrom(reader);

                ret = int.Parse(recordJson["Record"].ToString());
            }
        }

        return ret;
    }
}
