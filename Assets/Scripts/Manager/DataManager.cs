using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public class DataManager : Singleton<DataManager>
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("dI19vfsdf2341bP2");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("0rB5mCzYdfngf0G8");
    public static T LoadData<T>(string filePath)
    {
        if(File.Exists(Application.persistentDataPath + filePath))
        {
            string loaded = File.ReadAllText(Application.persistentDataPath + filePath);
            if (loaded == null)
            {
                throw new System.NullReferenceException();
            }

            T Data = JsonConvert.DeserializeObject<T>(Decrypt(loaded));

            return Data;
        }
        else
        {
            return default;
        }
    }

    public static void SaveData<T>(T data, string filePath)
    {
        string context = JsonConvert.SerializeObject(data);
        context = Encrypt(context);
        File.WriteAllText(Application.persistentDataPath + filePath, context);
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            return Convert.ToBase64String(encryptedData);
        }
    }

    public static string Decrypt(string encryptedString)
    {
        using(Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decrytor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] encryptedData = Convert.FromBase64String(encryptedString);
            byte[] decryptedData = decrytor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}

public class GameData
{
    public int LastClearedChapter;
}
