using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseMapCreate :MonoBehaviour
{
    protected int length;
    protected int height;
    protected byte[,] map;

    

    public byte[,] Map
    {
        get { return map; }
    }

    public virtual void Initialize(int length,int height)
    {
        this.length = length;
        this.height = height;
        map = new byte[length, height];
    }
    public virtual void Create()
    {
        //1.ClearPastMap
        ClearCache();
        //2.Create Wall
        CreateWall();

    }
    protected void ClearCache()
    {
        for (int j = 1; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
                map[i, j] = (byte)GroundMapType.Null;
            }
        }
    }
    protected void CreateWall()
    {
        for (int i = 0; i < length; i++)
        {
            map[i, 0] = (byte)GroundMapType.Wall;
            map[i, height - 1] = (byte)GroundMapType.Wall;
        }
        for (int j = 1; j < height; j++)
        {
            map[0, j] = (byte)GroundMapType.Wall;
            map[length - 1, j] = (byte)GroundMapType.Wall;
        }
    }

    protected virtual void Fill(int i, int j)
    {
        byte r = 0;
        if (map[i, j + 1] != (byte)GroundMapType.Null)
            r += 1;
        if ((map[i, j - 1] & (byte)GroundMapType.Null) == 0)
            r += 2;
        if (map[i - 1, j] != (byte)GroundMapType.Null)
            r += 4;
        if (map[i + 1, j] != (byte)GroundMapType.Null)
            r += 8;
        if ((r % 4 == 3) || (r % 16 >= 12))
        {
            map[i, j] = (byte)GroundMapType.Ground;
            return;
        }
        if ((r & 3) != 0 && (r & 12) != 0)
            return;
        if ((r & 1) != 0)
        {
            if ((map[i + 1, j - 1] & (byte)GroundMapType.Null) == 0 || (map[i - 1, j - 1] & (byte)GroundMapType.Null) == 0)
            {
                map[i, j] = (byte)GroundMapType.Ground;
                return;
            }
        }
        if ((r & 2) != 0)
        {
            if ((map[i + 1, j + 1] & (byte)GroundMapType.Null) == 0 || (map[i - 1, j + 1] & (byte)GroundMapType.Null) == 0)
            {
                map[i, j] = (byte)GroundMapType.Ground;
                return;
            }
        }
        if ((r & 4) != 0)
        {
            if ((map[i + 1, j + 1] & (byte)GroundMapType.Null) == 0 || (map[i + 1, j - 1] & (byte)GroundMapType.Null) == 0)
            {
                map[i, j] = (byte)GroundMapType.Ground;
                return;
            }
        }
        if ((r & 8) != 0)
        {
            if ((map[i - 1, j + 1] & (byte)GroundMapType.Null) == 0 || (map[i - 1, j - 1] & (byte)GroundMapType.Null) == 0)
            {
                map[i, j] = (byte)GroundMapType.Ground;
                return;
            }
        }
    }
    
}
