using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using PlatformGame.Map;

public class AICerebellum_Jump : AICerebellum
{
    public Acquire<float> onAcquireMoveSpeed;
    public Acquire<int> onAcquireJumpCount;

    [SerializeField] protected int headObstruction;
    [SerializeField] protected bool needActualArrive;
    [SerializeField] protected float arriveOffset;

    Task task;

    public int HeadObstruction
    {
        set => headObstruction = value;
    }


    private void Update()
    {
        if (!canThink)
            return;
        Moving();
    }
    public override void Open()
    {
        base.Open();
        lastEdgePoint = MapManager.Instance().FindNearestEdge(Pos, Pos, FindPathType.Jump);
    }
    protected override void FindPath()
    {
        if (!canFindPath)
            return;
        //
#if UNITY_EDITOR
        Debug.Log("FindPath:"+Time.time);
#endif
        Vector3 modelPos = model.position;
        task = new Task(() =>
        {
            var pos = MapManager.Instance().WorldPosToGridPos(modelPos);
            var a = lastEdgePoint;
            var b = MapManager.Instance().FindNearestEdge(aim, pos,FindPathType.Jump);
            var l = MapManager.Instance().FindPath(a, b, FindPathType.Jump);
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
        Pos = MapManager.Instance().WorldPosToGridPos(model.position);
        if (Pos.y > list[0].y)
            OverTheAim(Pos);
        else if (Pos.y < list[0].y)
            UnderTheAim(Pos);
        else
            EqualTheAim(Pos);
    }
    protected override void RefreshLastEdge()
    {
        lastEdgePoint = MapManager.Instance().FindNearestEdge(Pos, Pos, FindPathType.Jump);
    }
    #region MoveMethod
    void OverTheAim(Vector2Int pos)
    {
        if (pos.x > list[0].x)
            onMove.Invoke(-1);
        else if (pos.x < list[0].x)
            onMove.Invoke(1);
        else
        {
            Vector2 m = MapManager.Instance().GridPosToWorldPos(list[0]);
            if (Mathf.Abs(model.position.x - m.x) < minPathOffset)
            {
                onMove.Invoke(0);
                if (!needActualArrive && MapManager.Instance().IfDirectArrive(pos, list[0]) && onAcquireJumpCount.Invoke() > 0) 
                    RemoveFromList();
            }
            else if (model.position.x > m.x)
                onMove.Invoke(-1);
            else
                onMove.Invoke(1);
        }
    }
    void UnderTheAim(Vector2Int pos)
    {
        if (Mathf.Abs(pos.x - list[0].x) > 1)
        {
            if (headObstruction == 0)
            {
                onMove.Invoke(pos.x > list[0].x ? -1 : 1);
                onJump.Invoke();
            }
            else if (headObstruction != 2)
            {//反向走位
                onMove.Invoke(-headObstruction);
            }
        }
        else if (pos.x - list[0].x != 0)
        {
            if (headObstruction == 0)
            {
                onMove.Invoke(0);
                onJump.Invoke();
            }
            else if (headObstruction != 2)
            {//反向走位
                onMove.Invoke(-headObstruction);
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("UnderTheAimError");
#endif
            RefreshLastEdge();
            FindPath();
        }
    }
    void EqualTheAim(Vector2Int pos)
    {
        if (pos.x == list[0].x)
        {
            Vector2 m = MapManager.Instance().GridPosToWorldPos(list[0]);
            if (Mathf.Abs(model.position.x - m.x) < minPathOffset)
            {
                onMove.Invoke(0);
                if (!needActualArrive || Mathf.Abs(model.position.y - m.y) < arriveOffset)
                {
                    RemoveFromList();
                }
            }
            else
                onMove.Invoke(model.position.x > m.x ? -1 : 1);
        }
        else
        {
            onMove.Invoke(pos.x > list[0].x ? -1 : 1);
        }
    }
    #endregion
    public void IfJump()
    {
        if (list.Count == 0)
        {
            return;
        }
        if (Mathf.Abs(onAcquireMoveSpeed.Invoke()) > 1f && !(Pos.y - list[0].y > Mathf.Abs(Pos.x - list[0].x) * 2)) 
        {
            onJump.Invoke();
        }
    } 
}
