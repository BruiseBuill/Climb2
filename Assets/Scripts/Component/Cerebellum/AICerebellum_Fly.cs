using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using PlatformGame.Map;

public class AICerebellum_Fly : AICerebellum
{
    new public UnityAction<Vector3> onMove = delegate { };
    [SerializeField] int suspendHeight;
    
    [SerializeField] bool isCorrectedByNext;

    private void Update()
    {
        if (!canThink)
            return;
        Moving();
    }
    public override void Open()
    {
        base.Open();
        lastEdgePoint = MapManager.Instance().FindNearestEdge(Pos, Pos, FindPathType.Fly);
    }
    public override void Move(Vector2Int pos)
    {
        aim = pos + new Vector2Int(0, suspendHeight);
        FindPath();
    }
    protected override void FindPath()
    {
        if (!canFindPath)
            return;
        //
#if UNITY_EDITOR
        Debug.Log("FindPath:" + Time.time);
#endif
        Vector3 modelPos = model.position;
        Task task = new Task(() =>
        {
            var pos = MapManager.Instance().WorldPosToGridPos(modelPos);
            var a = lastEdgePoint;
            var b = MapManager.Instance().FindNearestEdge(aim, pos, FindPathType.Fly);
            var l = MapManager.Instance().FindPath(a, b, FindPathType.Fly);
            list.Clear();
            for (int i = 0; i < l.Count; i++)
            {
                list.Add(l[i]);
            }
        });
        task.Start();
        //Reset time of canFindPath
        StopCoroutine("FindPathBreak");
        StartCoroutine("FindPathBreak");

        StopCoroutine("ReFindPath");
        StartCoroutine("ReFindPath");
    }
    protected override void Moving()
    {
        if (list.Count == 0)
            return;
        Vector3 m = MapManager.Instance().GridPosToWorldPos(list[0]);
        if ((m - model.position).sqrMagnitude < minPathOffset * minPathOffset) 
        {
            RemoveFromList();
            return;
        }
        var orient = (m - model.position).normalized;

        onMove.Invoke(orient);
    }
    protected override void RefreshLastEdge()
    {
        lastEdgePoint = MapManager.Instance().FindNearestEdge(Pos, Pos, FindPathType.Fly);
    }
}
