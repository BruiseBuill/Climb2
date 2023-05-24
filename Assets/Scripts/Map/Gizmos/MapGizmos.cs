using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGizmos : MonoBehaviour
{
    [SerializeField] bool isShowEdgeGrid;
    [SerializeField] bool isShowJumpLine;
    [SerializeField] bool isShowDropOrMoveLine;
    [SerializeField] Vector2Int detailGrid;
    [SerializeField] List<Vector2Int> nextPos;
    

    [SerializeField] float gridWidth = 1f;
    //
    [SerializeField] bool hasInitialized = false;
    Dictionary<Vector2Int, PathPoint> dic;
    [SerializeField] Vector2 startPos;
    //

    public void Initialize(Dictionary<Vector2Int, PathPoint> dic)
    {
        this.dic = dic;
        hasInitialized = true;
    }
    private void OnDrawGizmos()
    {
        if (!hasInitialized)
            return;
        foreach (var j in dic)
        {
            if (isShowEdgeGrid)
                DrawGrid(j.Value);
            if (isShowJumpLine)
                DrawJumpLine(j.Value);
            if (isShowDropOrMoveLine)
                DrawDropLine(j.Value);
            DrawDetail();
        } 
    }
    void DrawGrid(PathPoint i)
    {
        switch (i.type)
        {
            case PathPointType.LeftEdge:
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(new Vector3(startPos.x + i.pos.x * gridWidth, startPos.y + i.pos.y * gridWidth, 0f), Vector3.one);
                break;
            case PathPointType.RightEdge:
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(startPos.x + i.pos.x * gridWidth, startPos.y + i.pos.y * gridWidth, 0f), Vector3.one);
                break;
            case PathPointType.SoleGround:
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(startPos.x + i.pos.x * gridWidth, startPos.y + i.pos.y * gridWidth, 0f), Vector3.one);
                break;
        }
    }
    void DrawJumpLine(PathPoint i)
    {
        foreach (var l in i.nextPointPos)
        {
            if (l.y > i.pos.y )
            {
                Gizmos.color = new Color(0.8f, 0.3f, 0.5f);
                Gizmos.DrawLine(
                new Vector3(startPos.x + i.pos.x * gridWidth, startPos.y + i.pos.y * gridWidth, 0f) + Vector3.one * 0.5f,
                new Vector3(startPos.x + l.x * gridWidth, startPos.y + l.y * gridWidth, 0f));
            }
        }
    }
    void DrawDropLine(PathPoint i)
    {
        foreach (var l in i.nextPointPos)
        {
            if (l.y <= i.pos.y)
            {
                Gizmos.color = new Color(1, 0.5f, 0.2f);
                Gizmos.DrawLine(
                new Vector3(startPos.x + i.pos.x * gridWidth, startPos.y + i.pos.y * gridWidth, 0f) + Vector3.one * 0.5f,
                new Vector3(startPos.x + l.x * gridWidth, startPos.y + l.y * gridWidth, 0f));
            }
        }
    }
    void DrawDetail()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(new Vector3(startPos.x + detailGrid.x * gridWidth, startPos.y + detailGrid.y * gridWidth, 0f), 0.5f);
        if (dic.ContainsKey(detailGrid))
        {
            
            nextPos = dic[detailGrid].nextPointPos;
        }
        
    }
}
