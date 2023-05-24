using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class JsonHandle 
{
    static string persistentDataPath = "C:/Users/Lenovo/AppData/LocalLow/DefaultCompany/Unity_Climb2";
    public static void Save<T>(T obj, string directoryPath,string fileName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(persistentDataPath + "/" + directoryPath);
        if (!Directory.Exists(builder.ToString())) 
        {
            Directory.CreateDirectory(builder.ToString());
            builder.Append("/" + fileName + ".txt");
            File.Create(builder.ToString()).Dispose();
        }
        else
        {
            builder.Append("/" + fileName + ".txt");
            if (!File.Exists(builder.ToString()))
            {
                File.Create(builder.ToString()).Dispose();
            }
        }
#if UNITY_EDITOR
        Debug.Log("Save");
#endif
        BinaryFormatter name1 = new BinaryFormatter();
        FileStream file = File.Open(builder.ToString(), FileMode.Truncate);
        var json = JsonUtility.ToJson(obj);
        name1.Serialize(file, json);
        file.Close();
    }  
    public static void Load<T>(T obj, string directoryPath, string fileName)
    {
        BinaryFormatter name1 = new BinaryFormatter();
        StringBuilder builder = new StringBuilder();
        builder.Append(persistentDataPath + "/" + directoryPath);
        if (Directory.Exists(builder.ToString())) 
        {
#if UNITY_EDITOR
            Debug.Log("Load");
            Debug.Log(persistentDataPath);
#endif
            builder.Append("/" + fileName + ".txt");
            if (File.Exists(builder.ToString()))
            {
                FileStream file = File.Open(builder.ToString(), FileMode.Open);
                if (file.Length != 0)
                {
                    JsonUtility.FromJsonOverwrite((string)name1.Deserialize(file), obj);
                }
                file.Close();
            }
        }
    }
}
