using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFind_Jump : PathFind
{
    [SerializeField] protected float reversePenalty_X;
    [SerializeField] protected float reversePenalty_Y;
    [SerializeField] int costPerJump;
    [SerializeField] int averageLengthPerLink;

    PriorityList<FindPoint> priorityList = new PriorityList<FindPoint>();
    Dictionary<Vector2Int, bool> pathdic = new Dictionary<Vector2Int, bool>();
    FindPoint lastPoint;

    public override List<Vector2Int> FindPath<T>(Vector2Int start, Vector2Int end, T map)
    {
        var dic = map as Dictionary<Vector2Int, PathPoint>;
        priorityList.Clear();
        pathdic.Clear();
        //
        FindPoint pathPoint = new FindPoint(null, start, 0, (end-start).sqrMagnitude);
        priorityList.Add(pathPoint, pathPoint.totalCost);
        pathdic.Add(pathPoint.pos, true);
        //
        while (priorityList.Count() > 0) 
        {
            var point = (FindPoint)priorityList.Pop();
            priorityList.Remove();
            lastPoint = point;
            if (point.pos == end)
            {
                return Link(point);
            }
            var list = dic[point.pos].nextPointPos;
            
            for (int i = 0; i < list.Count; i++) 
            {
                if (pathdic.ContainsKey(list[i])) 
                {
                    continue;
                }
                FindPoint p0 = new FindPoint(point, list[i], costPerJump * ((list[i] - point.pos).y > 0 ? 1 : 0) , (Mathf.Abs((list[i] - end).x) + Mathf.Abs((list[i] - end).y))/averageLengthPerLink);
                priorityList.Add(p0, p0.totalCost);
                pathdic.Add(p0.pos, true);
            }
        }
        Debug.LogError(string.Format("Can't Arrive,start: {0},end: {1}", start, end));
        return Link(lastPoint);
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

    public override Vector2Int FindNearestEdge<T>(Vector2Int start, Vector2Int end, T map)
    {
        var dic = map as Dictionary<Vector2Int, PathPoint>;
        noCheck.Clear();
        hasChecked.Clear();
        //
        noCheck.Add(start);

        var minus = end - start;
        float minExpense = 1000000;
        float presentExpense;
        Vector2Int aim = Vector2Int.zero;

        foreach (var i in dic)
        {
            presentExpense = Mathf.Abs(i.Key.x - start.x) + Mathf.Abs(i.Key.y - start.y) + reversePenalty_X * (minus.x * (i.Key.x - start.x) < 0 ? 1 : 0) + reversePenalty_Y * (minus.y * (i.Key.y - start.y) < 0 ? 1 : 0);
            if (presentExpense < minExpense)
            {
                minExpense = presentExpense;
                aim = i.Key;
            }
        }
        return aim;
    }
}
