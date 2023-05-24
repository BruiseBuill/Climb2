using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_AI : Control_Men
{
    protected Transform eye;
    protected AIGizmos gizmos;
    protected override void Awake()
    {
        base.Awake();
        
        eye = model.Find("Eye");
        gizmos = GetComponentInChildren<AIGizmos>();
        //
        Initialize();
    }
    private void Start()
    {
        Open();
    }
    public override void Initialize()
    {
        brain.Initialize(model);
        move.Initialize(rb, model);
        jump.Initialize(rb);
        vision.Initialize(foot1, foot2, eye);
        cerebellum.Initialize(model);
        //
        cerebellum.onMove += move.Move;
        (cerebellum as AICerebellum).onArrive += (brain as AIBrain).CompleteMove;

        (brain as AIBrain).onMove += cerebellum.Move;
        
        vision.onArriveGround += () => jump.IsOnGround = true;
        vision.onLeaveGround += () => jump.IsOnGround = false;
        
        (vision as Vision_AI).onFindPlayer += (brain as AIBrain_Normal).FindPlayer;
        (vision as Vision_AI).onLostPlayer += (brain as AIBrain_Normal).LostPlayer;
    }
    public override void Open()
    {
        move.Open();
        jump.Open();
        vision.Open();
        cerebellum.Open();
        brain.Open();
        gizmos?.Initialize((cerebellum as AICerebellum).MoveList);
    }
    public override void Close()
    {
        
    }
}
