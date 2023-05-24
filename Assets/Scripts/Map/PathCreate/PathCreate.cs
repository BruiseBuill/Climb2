using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PathPoint
{
    public Vector2Int pos;
    public List<Vector2Int> nextPointPos;
    public PathPointType type;
}

[Serializable]
public class PathCreate 
{
    protected byte[,] map;
    protected int length;
    protected int height;
    protected float jumpHeight;
    protected float jumpLength;
    protected float HDLDL;
    protected float gridWidth = 1;
    protected float maxSqrLinkDistance;

    public byte[,] Map
    {
        get => map;
    }
    //
    List<PathPoint> list = new List<PathPoint>();
    StrategyFactory strategyFactory;
    Dictionary<Vector2Int, PathPoint> pathPointDic = new Dictionary<Vector2Int, PathPoint>();

    public Dictionary<Vector2Int, PathPoint> PathPointDic
    {
        get => pathPointDic;
    }

    public void Initialize(int length, int height, float jumpLength, float jumpHeight,float maxLinkDistance)
    {
        strategyFactory = new StrategyFactory();
        strategyFactory.Initialize();
        this.jumpLength = jumpLength;
        this.jumpHeight = jumpHeight;
        this.length = length;
        this.height = height;
        map = new byte[length, height];
        HDLDL = jumpHeight / jumpLength / jumpLength;
        maxSqrLinkDistance = maxLinkDistance * maxLinkDistance;
    }
    public void CreatePath(byte[,] map)
    {
        //1.CopyMap
        CopyMap(map);

        //2.ClearPastInformation
        list.Clear();
        pathPointDic.Clear();

        //3.TransformGridTypeToPathPointType
        AnalysisGridType();

        //4.LinkEachPathPoint
        LinkEachPathPoint();

        //foreach (var i in list)
        {
            //Debug.Log(i.pos.ToString() + i.nextPointPos.Count.ToString());
        }
#if UNITY_EDITOR        
        Debug.Log("Initial List Length :" + list.Count.ToString());
#endif
        //5.As a platform game,your can drop from high grid.
        for (int i = 0; i < list.Count; i++)
        {
            DownLink(list[i]);
        }
        //
#if UNITY_EDITOR
        Debug.Log("Eventual List Length :" + list.Count.ToString());
#endif
        //foreach (var i in list)
        {
            //Debug.Log(i.pos.ToString() + i.nextPointPos.Count.ToString());
        }
    }
    void CopyMap(byte[,] map)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
                this.map[i, j] = map[i, j];
            }
                
        }
    }
    void Grid2PathPoint_Null(int i,int j)
    {
        //j==0,map[i,j]==Obstacle
        if (j == 0)
        {
            Debug.LogError(1);
        }
            
        if ((GroundMapType)map[i, j - 1] == GroundMapType.Null)       {
            map[i, j] = (byte)PathPointType.MidAir;
        }
        else if(i == 0)
        {
            if (map[i + 1, j - 1] != (byte)GroundMapType.Null)
                map[i, j] = (byte)PathPointType.LeftEdge;
            else
                map[i, j] = (byte)PathPointType.SoleGround;
        }
        else if (i == length - 1)
        {
            if (map[i - 1, j - 1] != (byte)GroundMapType.Null)
                map[i, j] = (byte)PathPointType.RightEdge;
            else
                map[i, j] = (byte)PathPointType.SoleGround;
        }
        else if ((GroundMapType)map[i - 1, j - 1] != GroundMapType.Null && (GroundMapType)map[i + 1, j - 1] != GroundMapType.Null)
            map[i, j] = (byte)PathPointType.GroundBelow;
        //Edge
        else
        {
            PathPoint point;
            point.pos = new Vector2Int(i, j);
            point.nextPointPos = new List<Vector2Int>();
            if ((GroundMapType)map[i - 1, j - 1] != GroundMapType.Null)
                map[i, j] = (byte)PathPointType.RightEdge;
            else if ((GroundMapType)map[i + 1, j - 1] != GroundMapType.Null)
                map[i, j] = (byte)PathPointType.LeftEdge;
            else
                map[i, j] = (byte)PathPointType.SoleGround;
            point.type = (PathPointType)map[i, j];
            list.Add(point);
            pathPointDic.Add(new Vector2Int(i, j), point);
        }
    }
    void AnalysisGridType()
    {
        for (int j = height - 1; j >= 0; j--)
        {
            for (int i = 0; i < length; i++)
            {
                switch ((GroundMapType)map[i, j])
                {
                    case GroundMapType.Null:
                        Grid2PathPoint_Null(i, j);
                        break;
                    default:
                        map[i, j] = (byte)PathPointType.Obstacle;
                        break;
                }
            }
        }
    }
    void LinkEachPathPoint()
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if ((list[i].pos - list[j].pos).sqrMagnitude > maxSqrLinkDistance && (list[i].type == list[j].type || list[i].type == PathPointType.SoleGround || list[j].type == PathPointType.SoleGround)) 
                {
                    continue;
                }
                    if (IfTheoryLink(list[i], list[j]) && IfHitObstacle(list[i], list[j])) 
                {
                    list[i].nextPointPos.Add(list[j].pos);
                }
            }
        }
    }
    bool IfTheoryLink(PathPoint start, PathPoint end)
    {
        switch (start.type)
        {
            case PathPointType.LeftEdge:
                switch (end.type)
                {
                    case PathPointType.LeftEdge:
                        return strategyFactory.ReturnStrategy(LinkMethod.LeftToLeft).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.RightEdge:
                        
                        if (IfDirectLinked(start, end))
                            return true;
                        return strategyFactory.ReturnStrategy(LinkMethod.LeftToRight).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.SoleGround:
                        return strategyFactory.ReturnStrategy(LinkMethod.LeftToSole).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                }
                break;
            case PathPointType.RightEdge:
                switch (end.type)
                {
                    case PathPointType.LeftEdge:
                        if (IfDirectLinked(start, end))
                        {
                            return true;
                        }
                        return strategyFactory.ReturnStrategy(LinkMethod.RightToLeft).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.RightEdge:
                        return strategyFactory.ReturnStrategy(LinkMethod.RightToRight).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.SoleGround:
                        return strategyFactory.ReturnStrategy(LinkMethod.RightToSole).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                }
                break;
            case PathPointType.SoleGround:
                switch (end.type)
                {
                    case PathPointType.LeftEdge:
                        return strategyFactory.ReturnStrategy(LinkMethod.SoleToLeft).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.RightEdge:
                        return strategyFactory.ReturnStrategy(LinkMethod.SoleToRight).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                    case PathPointType.SoleGround:
                        if (start.pos.y == end.pos.y && start.pos.x == end.pos.x)
                            return false;
                        else
                            return strategyFactory.ReturnStrategy(LinkMethod.SoleToSole).IfCanLink(start.pos, end.pos, gridWidth, jumpLength, jumpHeight, HDLDL);
                }
                break;
        }
        return false;
    }
    bool IfHitObstacle(PathPoint start, PathPoint end)
    {
        if (end.pos.y < start.pos.y)
        {
            switch (start.type)
            {
                case PathPointType.LeftEdge:
                    return !IfObstacleInLine(new Vector2Int(start.pos.x - 1, start.pos.y), end.pos);
                case PathPointType.RightEdge:
                    return !IfObstacleInLine(new Vector2Int(start.pos.x + 1, start.pos.y), end.pos);
                case PathPointType.SoleGround:
                    return !IfObstacleInLine(new Vector2Int(start.pos.x - 1, start.pos.y), end.pos) || !IfObstacleInLine(new Vector2Int(start.pos.x + 1, start.pos.y), end.pos);
            }
        }
        else if (end.pos.y > start.pos.y)
        {
            switch (end.type)
            {
                case PathPointType.LeftEdge:
                    return !IfObstacleInLine(start.pos, new Vector2Int(end.pos.x - 1, end.pos.y));
                case PathPointType.RightEdge:
                    return !IfObstacleInLine(start.pos, new Vector2Int(end.pos.x + 1, end.pos.y));
                case PathPointType.SoleGround:
                    return !IfObstacleInLine(start.pos, new Vector2Int(end.pos.x - 1, end.pos.y)) || !IfObstacleInLine(start.pos, new Vector2Int(end.pos.x + 1, end.pos.y));
            }
        }
        return !IfObstacleInLine(start.pos, end.pos);
    }
    bool IfObstacleInLine(Vector2Int a, Vector2Int b)
    {
        bool hasObstacle = false;
        int ori_x = b.x > a.x ? 1 : -1;
        int ori_y = b.y > a.y ? 1 : -1;
        Vector2Int s = Vector2Int.Min(a, b);
        Vector2Int e = Vector2Int.Max(a, b);
        if (s.x < 0 || e.x > length - 1)
        {
            return true;
        }
        float dy = e.y - s.y;
        float dx = e.x - s.x;
        if (dx < dy)
        {
            for (int i = 0; i <= e.y - s.y; i++)
            {
                if (map[a.x + (int)(i / dy * (b.x - a.x)), a.y + ori_y * i] == (int)PathPointType.Obstacle)
                {
                    hasObstacle = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i <= e.x - s.x; i++)
            {
                if (map[a.x + ori_x * i, a.y + (int)(i / dx * (b.y - a.y))] == (int)PathPointType.Obstacle)
                {
                    hasObstacle = true;
                    break;
                }
            }
        }
        return hasObstacle;
    }
    
    void DownLink(PathPoint edge)
    {
        switch (edge.type)
        {
            case PathPointType.LeftEdge:
                if (edge.pos.x > 0)
                {
                    IfDownLink(edge.pos.x - 1, edge.pos.y, edge);
                }
                break;
            case PathPointType.RightEdge:
                if (edge.pos.x < map.GetLength(0) - 1)
                {
                    IfDownLink(edge.pos.x + 1, edge.pos.y, edge);
                }
                break;
            case PathPointType.SoleGround:
                if (edge.pos.x > 0 && edge.pos.x < map.GetLength(0) - 1)
                {
                    IfDownLink(edge.pos.x - 1, edge.pos.y, edge);
                    IfDownLink(edge.pos.x + 1, edge.pos.y, edge);
                }
                break;
        }
    }
    
    void IfDownLink(int x, int y, PathPoint point)
    {
        if (map[x, y] != (byte)PathPointType.MidAir)
        {
            return;
        }
        for (int j = 1; j <= y; j++)
        {
            if (map[x, y - j] != (byte)PathPointType.MidAir)
            {
                if (map[x, y - j] >= (byte)PathPointType.LeftEdge && map[x, y - j] <= (byte)PathPointType.SoleGround)
                {
                    var down = pathPointDic[new Vector2Int(x, y - j)];
                    if (!point.nextPointPos.Contains(down.pos))
                    {
                        point.nextPointPos.Add(down.pos);
                        if (IfTheoryLink(down, point))
                        {
                            down.nextPointPos.Add(point.pos);
                        }
                    }
                }
                else if (map[x, y - j] == (byte)PathPointType.GroundBelow)
                {
                    PathPoint edgePoint;
                    edgePoint.pos = new Vector2Int(x, y - j);
                    edgePoint.nextPointPos = new List<Vector2Int>();
                    map[x, y - j] = (byte)PathPointType.SoleGround;
                    edgePoint.type = PathPointType.SoleGround;
                    list.Add(edgePoint);
                    pathPointDic.Add(new Vector2Int(x, y - j), edgePoint);
                    //
                    point.nextPointPos.Add(edgePoint.pos);
                    //
                    if (IfTheoryLink(edgePoint, point))
                    {
                        edgePoint.nextPointPos.Add(point.pos);
                    }
                    //Á¬½Ó×óÓÒ
                    PathPoint left = RelinkLeftAndRightEdge(x, y - j, edgePoint, -1);
                    PathPoint right = RelinkLeftAndRightEdge(x, y - j, edgePoint, 1);
                    if (left.pos != edgePoint.pos && right.pos != edgePoint.pos)
                    {
                        left.nextPointPos.Remove(right.pos);
                        right.nextPointPos.Remove(left.pos);
                    }
                }
                break;
            }
        }
    }
    PathPoint RelinkLeftAndRightEdge(int x, int y, PathPoint edge, int orient)
    {
        PathPoint left = edge;
        int px;
        for (int i = 1; ; i++)
        {
            px = x + orient * i;
            if (px < 0 || px >= length || map[px, y] == (byte)PathPointType.Obstacle)
            {
                break;
            }
            else if (map[px, y] >= (byte)PathPointType.LeftEdge && map[px, y] <= (byte)PathPointType.SoleGround)
            {
                left = pathPointDic[new Vector2Int(px, y)];
                left.nextPointPos.Add(edge.pos);
                edge.nextPointPos.Add(left.pos);
                break;
            }
        }
        return left;
    }
    bool IfDirectLinked(PathPoint i, PathPoint j)
    {
        if (i.pos.y != j.pos.y)
        {
            return false;
        }
        else
        {
            int y = i.pos.y;
            bool isDirectLinked = true;
            for (int s = Mathf.Min(i.pos.x, j.pos.x); s <= Mathf.Max(i.pos.x, j.pos.x); s++)
            {
                if ((PathPointType)map[s, y - 1] != PathPointType.Obstacle || (PathPointType)map[s, y] == PathPointType.Obstacle) 
                {
                    isDirectLinked = false;
                    break;
                }
            }
            return isDirectLinked;
        }
    }
}
/**/



