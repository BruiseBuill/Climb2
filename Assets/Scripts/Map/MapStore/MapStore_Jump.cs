using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapStore_Jump : BaseMapStore
{
    [Serializable]
    public class Data
    {
        public byte[] groundMap;
        public List<Vector2Int> posList = new List<Vector2Int>();
        public List<PathPoint> valueList = new List<PathPoint>();

        public Data(byte[,] m, Dictionary<Vector2Int, PathPoint> d)
        {
            groundMap = new byte[m.GetLength(0) * m.GetLength(1)];
            for (int j = 0; j < m.GetLength(1); j++)
            {
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    groundMap[j * m.GetLength(0) + i] = m[i, j];
                }
            }
            posList.Clear();
            valueList.Clear();
            foreach (var i in d)
            {
                posList.Add(i.Key);
                valueList.Add(i.Value);
            }
        }
    }
    public class DataList : ScriptableObject
    {
        [SerializeField] List<Data> datas = new List<Data>();
        

        public int presentChunkID = 0;

        public void Add(byte[,] groundMap, Dictionary<Vector2Int, PathPoint> dic)
        {
            var c = new Data(groundMap, dic);
            datas.Add(c);
        }
        public void Clear()
        {
            datas.Clear();
        }
        public void Remove()
        {
            datas.RemoveAt(0);
        }
        public Data Pop(int offset)
        {
            return datas[presentChunkID + offset];
        }
        public int Count()
        {
            return datas.Count;
        }
    }

    [SerializeField] DataList dataList;
    [SerializeField] string fileName;

    byte[,] map;
    Dictionary<Vector2Int, PathPoint> dic;
    bool hasChanged_Map;
    bool hasChanged_Dic;

    public override void Initialize(int length, int height)
    {
        base.Initialize(length, height);
        dataList = ScriptableObject.CreateInstance<DataList>();
        Load();
        hasChanged_Map = true;
        hasChanged_Dic = true;
    }
    public override int MapCount()
    {
        if (dataList == null)
        {
            return 0;
        }
        return dataList.Count();
    }
    public override void Add<T>(byte[,] map,T t)
    {
        dataList.Add(map, t as Dictionary<Vector2Int, PathPoint>);
        Save();
    }
    public override void Delete()
    {
        dataList.Remove();
        Save();
        hasChanged_Map = true;
        hasChanged_Dic = true;
    }
    public override void Save()
    {
        JsonHandle.Save(dataList, "Map", fileName);
    }
    public override void Load()
    {
        JsonHandle.Load(dataList, "Map", fileName);
    }
    public override byte[,] GetMapData()
    {
        if (hasChanged_Map)
        {
            var data = dataList.Pop(0).groundMap;
            byte[,] m = new byte[length, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    m[i, j] = data[j * length + i];
                }
            }
            map = m;
            hasChanged_Map = false;
        }
        return map;
    }
    public Dictionary<Vector2Int,PathPoint> GetPathPointDic()
    {
        if (hasChanged_Dic)
        {
            var data = dataList.Pop(0);
            Dictionary<Vector2Int, PathPoint> d = new Dictionary<Vector2Int, PathPoint>();
            for (int i = 0; i < data.posList.Count; i++)
            {
                d.Add(data.posList[i], data.valueList[i]);
            }
            dic = d;
            hasChanged_Dic = false;
        }
        return dic;
    }
}
