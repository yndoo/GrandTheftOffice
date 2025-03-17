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

    public static T LoadData<T>(string filePath)
    {
        if(File.Exists(Application.persistentDataPath + filePath))
        {
            string loaded = File.ReadAllText(Application.persistentDataPath + filePath);
            if (loaded == null)
            {
                throw new System.NullReferenceException();
            }

            string context = Decrypt(loaded);
            T Data = JsonConvert.DeserializeObject<T>(context);

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
            aes.GenerateIV();

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);

            // 암호화 
            byte[] encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            // IV를 데이터 앞에 덧붙임
            byte[] result = new byte[aes.IV.Length + encryptedData.Length];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);

            return Convert.ToBase64String(result);
        }
    }

    public static string Decrypt(string encryptedString)
    {
        byte[] encryptedData = Convert.FromBase64String(encryptedString);

        using(Aes aes = Aes.Create())
        {
            aes.Key = key;

            //IV 복구
            byte[] iv = new byte[aes.IV.Length];
            byte[] realEncrytedContext = new byte[encryptedData.Length - iv.Length];
            Array.Copy(encryptedData, 0, iv, 0, iv.Length);
            Array.Copy(encryptedData, iv.Length, realEncrytedContext, 0, realEncrytedContext.Length);

            // 복호화 
            aes.IV = iv;
            ICryptoTransform decrytor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decryptedData = decrytor.TransformFinalBlock(realEncrytedContext, 0, realEncrytedContext.Length);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}

public class GameData
{
    public int LastClearedChapter;
}
