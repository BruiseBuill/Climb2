using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class MapStore : BaseMapStore
{
    [SerializeField] MapChunk mapChunk;
    //
    byte[,] groundMap;
    int mapLength;
    int mapHeight;
    //
    Dictionary<Vector2Int, PathPoint> edgeDic = new Dictionary<Vector2Int, PathPoint>();

    public override void Initialize(int mapLength, int mapHeight)
    {
        this.mapLength = mapLength;
        this.mapHeight = mapHeight;
        groundMap = new byte[mapLength, mapHeight];
        //
        mapChunk = (MapChunk)ScriptableObject.CreateInstance("MapChunk");
        Load();
    }
    public bool CanLoadMap(int offset = 0)
    {
        return mapChunk.Count() > mapChunk.presentChunkID + offset;
    }
    public void AddMap(byte[,] groundMap, Dictionary<Vector2Int, PathPoint> edgeDic)
    {
        mapChunk.Add(groundMap, edgeDic);
    }
    public void AddMap(byte[,] groundMap)
    {

    }
    public void ClearMap()
    {
        while (mapChunk.Count() > 2)
        {
            mapChunk.Remove();
        }
        mapChunk.presentChunkID = 0;
        Save();
    }
    #region EditorOnly
    public void ClearAllMap()
    {
        FileStream file = File.Open(Application.persistentDataPath + "/Map/data.txt", FileMode.Truncate);
        file.SetLength(0);
        file.Close();
    }
    #endregion
    public override void Save()
    {
        //JsonHandle.Save()
        if (!Directory.Exists(Application.persistentDataPath + "/Map"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Map");
            File.Create(Application.persistentDataPath + "/Map/data.txt").Dispose();
        }
#if UNITY_EDITOR
        Debug.Log("Save");
#endif
        BinaryFormatter name1 = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Map/data.txt", FileMode.Truncate);
        var json = JsonUtility.ToJson(mapChunk);
        name1.Serialize(file, json);
        file.Close();
    }
    public override void Load()
    {
        BinaryFormatter name1 = new BinaryFormatter();
        if (Directory.Exists(Application.persistentDataPath + "/Map"))
        {
#if UNITY_EDITOR
            Debug.Log("Load");
            Debug.Log(Application.persistentDataPath);
#endif
            FileStream file = File.Open(Application.persistentDataPath + "/Map/data.txt", FileMode.Open);
            if (file.Length != 0)
            {
                JsonUtility.FromJsonOverwrite((string)name1.Deserialize(file), mapChunk);
            }
            file.Close();
        }
    }
    public Dictionary<Vector2Int, PathPoint> ReturnEdgeDic()
    {
        edgeDic.Clear();
        var a = mapChunk.Pop();
        for (int j = 0; j < a.list1.Count; j++)
        {
            edgeDic.Add(a.list1[j], a.list2[j]);
        }
        return edgeDic;
    }
    public byte[,] ReturnMap(int offset = 0)
    {
        var a = mapChunk.PopNext(offset);
        for (int j = 0; j < mapHeight; j++)
        {
            for (int i = 0; i < mapLength; i++)
            {
                groundMap[i, j] = a.groundMap[j * mapLength + i];
            }
        }
        return groundMap;
    }
    public void ChangePresentIndex(int i)
    {
        mapChunk.presentChunkID += i;
        //Save();
    }
}
