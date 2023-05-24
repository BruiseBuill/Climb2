using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapCreate :BaseMapCreate
{
    [Header("地图风格")]
    
    [Tooltip("初始的浮空层数目")] [SerializeField] int initialGridSum;
    [Tooltip("初始的浮空层离地面的 最小高度,最大高度")] [SerializeField] Vector2 initialGridHeight;
    [Tooltip("初始的浮空层离固定锚点的 最大水平偏移")] [SerializeField] float initialGridOffset;
    [Tooltip("浮空层之间的 最小高度间距,最大高度间距")][SerializeField] Vector2 gridHeightGap;
    [Tooltip("浮空锚点之间的 最小长度间距,最大长度间距")][SerializeField] Vector2 gridLengthGap;
    [Tooltip("浮空层锚点的 最小分裂数,最大分裂数")] [SerializeField] Vector2 divisionRate;

    [Tooltip("浮空层宽度向两边延伸的 概率")] [SerializeField] float stretchPossibility;
    [Tooltip("浮空层向单边延伸的 最大宽度")] [SerializeField] int maxStretchGridSum;
    [Tooltip("浮空层厚度")][SerializeField] Vector2 thickness;
    [Tooltip("最高的浮空层与地图最高值的高度距离")] [SerializeField] int highestFromTop;
    //
    [Header("OtherParameter")]
    [SerializeField] protected int minGap_X;
    [SerializeField] protected int minGap_Y;
    //
    List<Vector2Int> noCheck = new List<Vector2Int>();
    List<Vector2Int> hasChecked = new List<Vector2Int>();
    

    public override void Create()
    {
        base.Create();
        //3.
        CreateInitialGrid();
        //4.
        Vector2Int grid = new Vector2Int(0, 0);
        while (noCheck.Count > 0)
        {
            CreateNewGrid(noCheck[0].x, noCheck[0].y);
            grid.Set(noCheck[0].x, noCheck[0].y);
            hasChecked.Add(grid);
            noCheck.RemoveAt(0);
        }
        while (hasChecked.Count > 0)
        {
            StretchGrid(-1, hasChecked[0].x, hasChecked[0].y);
            StretchGrid(1, hasChecked[0].x, hasChecked[0].y);
            hasChecked.RemoveAt(0);
        }
        //5.
        Thickness();
        FillMap();
        //*
        KeepMinGap();
    }

    protected void CreateInitialGrid()
    {
        noCheck.Clear();
        hasChecked.Clear();
        for (int i = 0; i < initialGridSum; i++) 
        {
            float present_X = (i + 0.5f) / initialGridSum * length;
            int a = (int)RandomMethod.Range(Mathf.Max(present_X - initialGridOffset, 0f), Mathf.Min(present_X + initialGridOffset, length - 1));
            int b = (int)RandomMethod.Range(initialGridHeight.x, initialGridHeight.y);
            if (map[a, b] == (byte)GroundMapType.Null)
            {
                noCheck.Add(new Vector2Int(a, b));
                map[a, b] = (byte)GroundMapType.Ground;
            }
        }
    }
    protected void CreateNewGrid(int x,int y)
    {
        if (x < 1 || x > length - 2 ) 
        {
            return;
        }
        int count = (byte)RandomMethod.Range(divisionRate.x, divisionRate.y);
        for (int i = 0; i < count; i++) 
        {
            Vector2Int a;
            if (count == 1)
                a = ReturnLinkGrid(x, y);
            else
                a = ReturnLinkGrid(x, y, 2 * i - 1);
            if (a.x < 0 || a.x > length - 1)
                return;
            if (a.y > height - highestFromTop)
                return;
            if (map[a.x, a.y] == (byte)GroundMapType.Null) 
            {
                map[a.x, a.y] = (byte)GroundMapType.Ground;
                noCheck.Add(new Vector2Int(a.x, a.y));
            }
        }
    }
    void StretchGrid(int orient, int x, int y)
    {
        for(int i = 0; i < maxStretchGridSum; i++)
        {
            int x2 = x + orient * (i + 1);
            if (x2 < 0 || x2 > length - 1 || map[x2, y] != (byte)GroundMapType.Null)
                return;
            float p = RandomMethod.Range(0f, 1f);
            if (p < stretchPossibility)
            {
                map[x2, y] = map[x, y];
            }
            else
                return;
        }
    }
    void Thickness()
    {
        for(int j = 2; j < height; j++)
        {
            for (int i = 1; i < length - 1; i++) 
            {
                int t = (int)RandomMethod.Range(thickness.x, thickness.y);
                 for(int k = 1; k < t; k++)
                {
                    if (map[i, j] != (byte)GroundMapType.Null)
                    {
                        map[i, j - k] = map[i, j];
                    }
                }
            }
        }
    }
    void FillMap()
    {
        for(int j = 1; j < height -highestFromTop; j++)
        {
            for(int i = 2; i < length - 2; i++)
            {
                if (map[i,j] == (byte)GroundMapType.Null)
                {
                    Fill(i, j);
                }   
            }
        }
    }
    Vector2Int ReturnLinkGrid(int x,int y)
    {
        float h = RandomMethod.Range(gridHeightGap.x, gridHeightGap.y);
        float l = RandomMethod.Range(gridLengthGap.x, gridLengthGap.y) * (RandomMethod.Range(-1f, 1f) > 0 ? 1 : -1);
        Vector2Int a = new Vector2Int((int)(x + l), (int)(y + h));
        return a;
    }
    Vector2Int ReturnLinkGrid(int x, int y, int ori)
    {
        float h = RandomMethod.Range(gridHeightGap.x, gridHeightGap.y);
        float l = RandomMethod.Range(gridLengthGap.x, gridLengthGap.y) * ori;
        return new Vector2Int((int)(x + l), (int)(y + h));
    }
    protected void KeepMinGap()
    {
        int count = 0;//null grid count
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] != (byte)GroundMapType.Null)
                {
                    MinGap_Y(ref count, ref i, ref j);
                }
                else
                    count++;
            }
            count = 0;
        }
        for (int j = 2; j < height; j++)
        {
            for (int i = 0; i < length - minGap_X; i++)
            {
                if (map[i, j] != (byte)GroundMapType.Null)
                {
                    MinGap_X(ref count, ref i, ref j);
                }
                else
                    count++;
            }
            count = 0;
        }
    }
    void MinGap_X(ref int count, ref int i, ref int j)
    {
        if (count != 0 && count < minGap_X)
        {
            for (int k = count; k < minGap_X; k++)
            {
                map[i, j] = (byte)GroundMapType.Null;
                map[i, j - 1] = (byte)GroundMapType.Null;
                i++;
            }
            count = minGap_X;
        }
        else
            count = 0;
    }
    void MinGap_Y(ref int count, ref int i, ref int j)
    {
        if (count != 0 && count < minGap_Y)
        {
            for (int k = count; k < minGap_Y; k++)
            {
                map[i, j] = (byte)GroundMapType.Null;
                j++;
            }
            count = minGap_Y;
        }
        else
            count = 0;
    }
}
