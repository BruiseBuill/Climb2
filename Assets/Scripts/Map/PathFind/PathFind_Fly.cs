using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathFind_Fly :PathFind
{
    PriorityList<FindPoint> priorityList = new PriorityList<FindPoint>();
    byte listCheckValue;
    byte[,] checkValue;
    int length;
    int height;
    [SerializeField] int moveCostMultiple;
    [SerializeField] int remainCostMultiple;

    public override List<Vector2Int> FindPath<T>(Vector2Int start, Vector2Int end, T map)
    {
        //1.Clear
        var array = map as byte[,];
        priorityList.Clear();
        listCheckValue++;
        //2.Initialize
        if (checkValue == null)
        {
            length = array.GetLength(0);
            height = array.GetLength(1);
            checkValue = new byte[length, height];
        }

        FindPoint pathPoint = new FindPoint(null, start, 0, (int)((end - start).magnitude * remainCostMultiple));
        priorityList.Add(pathPoint, pathPoint.totalCost);
        checkValue[pathPoint.pos.x, pathPoint.pos.y]++;
        var lastPoint = pathPoint;
        //3.FindPath
        while (priorityList.Count() > 0)
        {
            //4.Pop min Cost point
            var point = (FindPoint)priorityList.Pop();
            priorityList.Remove();
            lastPoint = point;
            //5.Check if arrive
            if (point.pos == end)
            {
                return Link(point);
            }
            //6.Check grid:bounds, if null, if has checked
            AddPoint(point.pos.x - 1, point.pos.y, point, array, end);
            AddPoint(point.pos.x , point.pos.y - 1, point, array, end);
            AddPoint(point.pos.x + 1, point.pos.y, point, array, end);
            AddPoint(point.pos.x , point.pos.y+1, point, array, end);
            AddPoint(point.pos.x - 1, point.pos.y-1, point, array, end);
            AddPoint(point.pos.x+1, point.pos.y - 1, point, array, end);
            AddPoint(point.pos.x + 1, point.pos.y+1, point, array, end);
            AddPoint(point.pos.x-1, point.pos.y + 1, point, array, end);
        }
        //
        Debug.LogError(string.Format("Can't Arrive,start: {0},end: {1}", start, end));
        return Link(lastPoint);
    }
    public override Vector2Int FindNearestEdge<T>(Vector2Int start, Vector2Int end, T map)
    {
        var array = map as byte[,];
        List<Vector2Int> list = new List<Vector2Int>();
        list.Add(start);
        int presentDistance = -3;
        while (list.Count != 0)
        {
            var point = list[0];
            list.RemoveAt(0);
            //
            if(array[point.x,point.y] == (byte)GroundMapType.Null)
            {
                return point;
            }
            //
            if((point - start).sqrMagnitude < presentDistance)
            {
                continue;
            }
            presentDistance++;
            if (point.x - 1 >= 0) 
            {
                list.Add(new Vector2Int(point.x - 1, point.y));
            }
            if (point.x < length ) 
            {
                list.Add(new Vector2Int(point.x + 1, point.y));
            }
            if (point.y - 1 >= 0 )
            {
                list.Add(new Vector2Int(point.x , point.y - 1));
            }
            if (point.y < height ) 
            {
                list.Add(new Vector2Int(point.x, point.y + 1));
            }
        }
        Debug.LogError("1:" + start.ToString());
        return Vector2Int.zero;
    }
    void AddPoint(int x, int y, FindPoint last, byte[,] array, Vector2Int end)
    {
        if (IfWithin(x,y)&& array[x, y] == (byte)GroundMapType.Null && checkValue[x, y] != listCheckValue)
        {
            FindPoint p0 = new FindPoint(last, new Vector2Int(x, y),(int)((new Vector2Int(x, y) - last.pos).magnitude * moveCostMultiple), (int)(Mathf.Sqrt((end.x - x) * (end.x - x) + (end.y - y) * (end.y - y)) * remainCostMultiple) );
            priorityList.Add(p0, p0.totalCost);
            checkValue[x, y] = listCheckValue;
        }
    }
    bool IfWithin(int x,int y)
    {
        return x >= 0 && x < length && y >= 0 && y < height;   
    }
    List<Vector2Int> Link(FindPoint p)
    {
        List<Vector2Int> paths = new List<Vector2Int>();
        paths.Add(p.pos);
        while (p.previousPos != p.pos)
        {
            paths.Add(p.previousPos);
            p = p.previousPoint;
        }
        paths.Reverse();
        return paths;
    }
}
