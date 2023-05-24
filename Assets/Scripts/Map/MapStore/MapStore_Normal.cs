using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapStore_Normal : BaseMapStore
{
    [Serializable]
    public class Data
    {
        public byte[] groundMap;

        public Data(byte[,] m)
        {
            groundMap = new byte[m.GetLength(0) * m.GetLength(1)];
            for (int j = 0; j < m.GetLength(1); j++)
            {
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    groundMap[j * m.GetLength(0) + i] = m[i, j];
                }
            }
        }
    }
    public class DataList : ScriptableObject
    {
        [SerializeField] List<Data> datas = new List<Data>();

        public int presentChunkID = 0;

        public void Add(byte[,] groundMap)
        {
            var c = new Data(groundMap);
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

    public override void Initialize(int length, int height)
    {
        base.Initialize(length, height);
        dataList = ScriptableObject.CreateInstance<DataList>();
        Load();
    }
    public override int MapCount()
    {
        if (dataList == null)
            return 0;
        return dataList.Count();
    }
    public override void Add(byte[,] map)
    {
        dataList.Add(map);
    }
    public override void Delete()
    {
        dataList.Remove();
    }
    public override void Save()
    {
        JsonHandle.Save(dataList, "Map", "data");
    }
    public override void Load()
    {
        JsonHandle.Load(dataList, "Map", "data");
    }
    public override byte[,] GetMapData()
    {
        //base.GetMapData(index);
        var data = dataList.Pop(0).groundMap;
        byte[,] m = new byte[length, height];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
                m[i, j] = data[j * length + i];
            }
        }
        return m;
    }
}
