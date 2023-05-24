using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate_Refuge : BaseMapCreate
{
    [Tooltip("Slice map into cubes")]
    [SerializeField] int lineLength;
    //
    [SerializeField] float radius;
    [SerializeField] int maxConnectDistance;
    [SerializeField] int minSpace;
    [SerializeField] int randoRoomCount;
    [SerializeField] Vector2Int[] fixedRoom;

    [SerializeField] List<Vector2Int> listPos1 = new List<Vector2Int>();

    public override void Create()
    {
        //1.Fill all grid
        FloodMap();
        //2.Create Line End
        listPos1.Clear();
        CreateCircleCenter();
        for (int i = 0; i < listPos1.Count; i++)
            CreateCircle(listPos1[i]);
        //3.Create null grid according to line

        CreateEachLine();
        //5.Fill
        FillMap();
        //
       // KeepMinGap();
    }
    void FloodMap()
    {
        for(int i = 0; i < length; i++)
        {
            for(int j = 0; j < height; j++)
            {
                map[i, j] = (byte)GroundMapType.Ground;
            }
        }
    }
    void CreateCircleCenter()
    {
        for(int i = 0; i < fixedRoom.Length; i++)
        {
            listPos1.Add(fixedRoom[i]);
        }
        int sqrMinSpace = minSpace * minSpace;
        bool isBreak = true;
        for (int i = 0; i < randoRoomCount; i++) 
        {
            Vector2Int pos = new Vector2Int(RandomMethod.Range(10, length-10), RandomMethod.Range(10, height-10));
            isBreak = false;
            for(int j = 0; j < listPos1.Count; j++)
            {
                if ((listPos1[j] - pos).sqrMagnitude < sqrMinSpace)
                {
                    //ReCreate pos
                    i--;
                    isBreak = true;
                    break;
                }
            }
            if (!isBreak)
                listPos1.Add(pos);
        }
    }

    void CreateCircle(Vector2Int center)
    {
        int r = (int)radius;
        float sqrR = radius * radius;
        for(int i = -r; i <= r; i++)
        {
            for(int j = -r; j <= r; j++)
            {
                if (i * i + j * j <= sqrR) 
                {
                    map[center.x + i, center.y + j] = (byte)GroundMapType.Null;
                }
            }
        }
    }
    void CreateEachLine()
    {
        int sqrMaxLink = maxConnectDistance * maxConnectDistance;
        for(int i = 0; i < listPos1.Count; i++)
        {
            for(int j = i+1; j < listPos1.Count; j++)
            {
                if ((listPos1[i] - listPos1[j]).sqrMagnitude < sqrMaxLink)
                {
                    CreateLine(listPos1[i], listPos1[j], lineLength);
                }
            }
        }
    }
    void CreateLine(Vector2Int a, Vector2Int b, int lineLength)
    {
        int ori_x = b.x > a.x ? 1 : -1;
        int ori_y = b.y > a.y ? 1 : -1;
        Vector2Int s = Vector2Int.Min(a, b);
        Vector2Int e = Vector2Int.Max(a, b);

        float dy = e.y - s.y;
        float dx = e.x - s.x;
        if (dx < dy)
        {
            for (int i = 0; i <= e.y - s.y; i++)
            {
                for (int j = 0; j < lineLength && a.x + j + Mathf.RoundToInt(i / dy * (b.x - a.x)) < length - 1; j++)
                {
                    map[a.x + j + Mathf.RoundToInt(i / dy * (b.x - a.x)), a.y + ori_y * i] = (byte)GroundMapType.Null;
                }
            }
        }
        else
        {
            for (int i = 0; i <= e.x - s.x; i++)
            {
                for (int j = 0; j < lineLength && a.y + j + Mathf.RoundToInt(i / dx * (b.y - a.y)) < height - 1; j++)
                {
                    map[a.x + ori_x * i, a.y + j + Mathf.RoundToInt(i / dx * (b.y - a.y))] = (byte)GroundMapType.Null;
                }
            }
        }
    }

    void FillMap()
    {
        for (int j = 1; j < height - 2; j++) 
        {
            for (int i = 2; i < length - 2; i++)
            {
                if (map[i, j] == (byte)GroundMapType.Null)
                {
                    Fill(i, j);
                }
            }
        }
    }
   
}
