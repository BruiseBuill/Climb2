using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using PlatformGame.Map;

public class AIBrain : BaseBrain
{
    public UnityAction<Vector2Int> onMove = delegate { };
    public UnityAction onJump = delegate { };
    public UnityAction onAttack = delegate { };
    //
    public UnityAction onStunOver = delegate { };
    //

    [Header("AICondition")]
    [SerializeField] protected AICondition condition;

    [Header("Patrol")]
    [SerializeField] protected int maxPatrolPointSum;
    [Tooltip("巡逻区域的左下角的坐标")]
    [SerializeField] protected Vector2Int patrolPoint_Min;
    [Tooltip("巡逻区域的右上角的坐标")]
    [SerializeField] protected Vector2Int patrolPoint_Max;
    [SerializeField] protected int minPointDistance;
    [SerializeField] protected int maxPointDistance;

    [Header("ReadOnly")]
    [SerializeField] protected int presentPatrolPointIndex;
    [Tooltip("巡逻路线")]
    [SerializeField] protected Vector2Int[] patrolLine;

    [Header("Character")]
    [SerializeField] protected float flexibility = 1f;
    [SerializeField] protected int level = 1;

    [Header("FightParameter")]
    [SerializeField] protected float attackPosYOffset;
    [SerializeField] protected float minRefreshPlayerBreak;
    public int Level
    {
        get { return level; }
    }   

    protected Transform model;
    protected Transform target;

    
    WaitForSeconds wait_Player;

    protected virtual void Awake()
    {
        wait_Player = new WaitForSeconds(minRefreshPlayerBreak);
    }
    
    public override void Open()
    {
        base.Open();
        condition = AICondition.Patrol;

        CreatePatrolLine();
        CompleteMove();
    }
    public override void Close()
    {
        base.Close();
    }
    protected virtual void ChangeCondition(AICondition aICondition)
    {
        if (aICondition == condition)
            return;
        switch (aICondition)
        {
            case AICondition.Attack:
                condition = AICondition.Attack;
                AttackThink();
                break;
            case AICondition.Patrol:
                condition = AICondition.Patrol;
                Move(patrolLine[presentPatrolPointIndex]);
                break;
        }
    }
    protected virtual void AttackThink()
    {
        StartCoroutine("RefreshPlayerPos");
    }
    protected virtual void PatrolThink()
    {
        presentPatrolPointIndex++;
        presentPatrolPointIndex %= patrolLine.Length;
        Move(patrolLine[presentPatrolPointIndex]);
    }
    protected IEnumerator RefreshPlayerPos()
    {
        while (true)
        {
            Move(target.position);
            yield return wait_Player;
        }
    }

    public virtual void BulletApproach(Vector3 bulletPos){}
    public virtual void BeAttacked(Vector3 pos){}
    public virtual void FindPlayer(Transform target){}
    public virtual void LostPlayer(){}
    public virtual void Die(){}
    public virtual void CompleteMove()
    {
        if (!canThink)
            return;
        switch (condition)
        {
            case AICondition.Patrol:
                StopCoroutine("RefreshPlayerPos");
                PatrolThink();
                break;
            case AICondition.Attack:
                
                break;
        }
    }
    protected void CreatePatrolLine()
    {
        patrolLine = new Vector2Int[Random.Range(2, maxPatrolPointSum + 1)];
        for (int i = 0; i < patrolLine.Length; i++)
        {
            patrolLine[i].Set(Random.Range(patrolPoint_Min.x, patrolPoint_Max.x), Random.Range(patrolPoint_Min.y, patrolPoint_Max.y));
            if (i != 0 && ((patrolLine[i] - patrolLine[i - 1]).sqrMagnitude < minPointDistance * minPointDistance || (patrolLine[i] - patrolLine[i - 1]).sqrMagnitude > maxPointDistance * maxPointDistance))
            {
                i--;
            }
        }
    }
    protected void Move(Vector3 pos)
    {
        onMove.Invoke(MapManager.Instance().WorldPosToGridPos(pos));
    }
    protected void Move(Vector2Int pos)
    {
        onMove.Invoke(pos);
    }
}
