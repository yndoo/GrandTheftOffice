using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    public static T LoadData<T>(string filePath)
    {
        if(File.Exists(Application.persistentDataPath + filePath))
        {
            string loaded = File.ReadAllText(Application.persistentDataPath + filePath);
            if (loaded == null)
            {
                throw new System.NullReferenceException();
            }

            T Data = JsonConvert.DeserializeObject<T>(loaded);

            return Data;
        }
        else
        {
            return default(T);
        }
    }

    public static void SaveData<T>(T data, string filePath)
    {
        string context = JsonConvert.SerializeObject(data);

        File.WriteAllText(Application.persistentDataPath + filePath, context);
    }
}

public class GameData
{
    public int LastClearedChapter;
}
