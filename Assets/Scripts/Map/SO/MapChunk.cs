using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Chunk
{
    public byte[] groundMap;
    public int[] itemMap;
    public int[] creatureMap;
    public List<Vector2Int> list1 = new List<Vector2Int>();
    public List<PathPoint> list2 = new List<PathPoint>();
    public Chunk(byte[,] a, Dictionary<Vector2Int, PathPoint> d)
    {
        groundMap = new byte[a.GetLength(0)* a.GetLength(1)];
        for (int j = 0; j < a.GetLength(1); j++)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                groundMap[j * a.GetLength(0) + i] = a[i, j];
            }
        }
        //
        itemMap = null;
        creatureMap = null;
        //
        list1.Clear();
        list2.Clear();
        foreach(var i in d)
        {
            list1.Add(i.Key);
            list2.Add(i.Value);
        }
    }
}

[CreateAssetMenu(fileName = "MapChunk")]
public class MapChunk : ScriptableObject
{
    [SerializeField] List<Chunk> chunks = new List<Chunk>();
    /*
    public int[,] groundMap;
    public int[,] itemMap;
    public int[,] creatureMap;
    public Dictionary<Vector2Int, EdgePoint> edgeDic = new Dictionary<Vector2Int, EdgePoint>();*/

    public int presentChunkID = 0;

    public void Add(byte[,] groundMap, Dictionary<Vector2Int, PathPoint> dic)
    {
        var c = new Chunk(groundMap, dic);
        chunks.Add(c);
    }
    public void Clear()
    {
        chunks.Clear();
    }
    public void Remove()
    {
        chunks.RemoveAt(0);
    }
    public Chunk Pop()
    {
        return chunks[presentChunkID];
    }
    public Chunk PopNext(int offset)
    {
        return chunks[presentChunkID + offset];
    }
    public int Count()
    {
        return chunks.Count;
    }
}
