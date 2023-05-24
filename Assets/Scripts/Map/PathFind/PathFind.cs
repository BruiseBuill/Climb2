using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFind : MonoBehaviour
{
    protected List<Vector2Int> noCheck = new List<Vector2Int>();
    protected List<Vector2Int> hasChecked = new List<Vector2Int>();

    public virtual List<Vector2Int> FindPath<T>( Vector2Int start,Vector2Int end, T dic) where T : class
    {
        return null;
    }
    public virtual Vector2Int FindNearestEdge<T>(Vector2Int start, Vector2Int end, T dic) where T: class
    {
        return Vector2Int.zero;
    }
}
public class FindPoint
{
    public FindPoint previousPoint;
    public Vector2Int previousPos;
    public Vector2Int pos;

    public int moveCost = 0;
    public int remainCost = 0;
    public int totalCost = 0;

    public FindPoint(FindPoint a, Vector2Int b, int moveCost, int remainCost)
    {
        if (a != null)
        {
            previousPoint = a;
            previousPos = a.pos;
            pos = b;
            this.moveCost = moveCost + a.moveCost;
        }
        else
        {
            previousPos = b;
            pos = b;
            moveCost = 0;
        }

        this.remainCost = remainCost;
        totalCost = moveCost + remainCost;
    }
}
