using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlatformGame.Map;

public class AICerebellum : BaseCerebellum
{
    public UnityAction onArrive = delegate { };
    [Header("FindPath")]
    [SerializeField] protected bool canFindPath = true;
    [Tooltip("该时间后强制重新寻路")]
    [SerializeField] protected float reFindPathTime;
    [Tooltip("一般情况下，寻路函数的最短调用间隔")]
    [SerializeField] protected float minFindPathBreak;
    [SerializeField] protected float minPathOffset;

    protected Transform model;
    
    [Header("PositionInfo")]
    [SerializeField] protected Vector2Int Pos;
    [SerializeField] protected bool faceOrient;
    [SerializeField] protected Vector2Int lastEdgePoint;

    [Header("移动")]
    [SerializeField] protected Vector2Int aim;
    [Tooltip("移动的路径点")]
    [SerializeField] protected List<Vector2Int> list = new List<Vector2Int>();

    protected WaitForSeconds wait_FindPathBreak;
    protected WaitForSeconds wait_ReFindPathTime;

    public List<Vector2Int> MoveList
    {
        get => list;
    }
    
    public override void Initialize<T0>(T0 t0)
    {
        this.model = t0 as Transform;

        wait_FindPathBreak = new WaitForSeconds(minFindPathBreak);
        wait_ReFindPathTime = new WaitForSeconds(reFindPathTime);
    }
    public override void Open()
    {
        base.Open();
        Pos = MapManager.Instance().WorldPosToGridPos(model.position);
        canFindPath = true;
    }
    public override void Close()
    {
        base.Close();
        canFindPath = false;
        list.Clear();
    }
    public override void Move(Vector2Int pos)
    {
        aim = pos;
        FindPath();
    }
    protected virtual void FindPath()
    {

    }  
    protected virtual void Moving()
    {
        
    }
    protected void RemoveFromList()
    {
        if (list.Count > 1)
            lastEdgePoint = list[1];
        list.RemoveAt(0);
        StopCoroutine("ReFindPath");
        StartCoroutine("ReFindPath");
        if (list.Count == 0)
            onArrive.Invoke();
    }
    protected virtual void RefreshLastEdge()
    {

    }
    protected IEnumerator FindPathBreak()
    {
        canFindPath = false;
        yield return wait_FindPathBreak;
        canFindPath = true;
    }
    //Reset timer of re routing when it arrive next edge in expected time 
    protected IEnumerator ReFindPath()
    {
        yield return wait_ReFindPathTime;
#if UNITY_EDITOR
        Debug.Log("ReFindPath");
#endif
        RefreshLastEdge(); 
        FindPath();
        StartCoroutine("ReFindPath");
    }
}
