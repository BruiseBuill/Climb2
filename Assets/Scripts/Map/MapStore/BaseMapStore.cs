using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class BaseMapStore :MonoBehaviour
{
    protected int length;
    protected int height;

    public virtual void Initialize(int length,int height)
    {
        this.length = length;
        this.height = height;
    }
    public virtual int MapCount()
    {
        return 0;
    }
    public virtual void Add(byte[,] map)
    {

    }
    public virtual void Add<T>(byte[,] map,T t)
    {

    }
    public virtual void Delete()
    {

    }
    public virtual void Save()
    {

    }
    public virtual void Load()
    {

    }
    public virtual byte[,] GetMapData()
    {
        return null;
    }
}