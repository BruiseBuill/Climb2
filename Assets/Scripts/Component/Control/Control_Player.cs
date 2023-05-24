using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Control_Player : Control_Men
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
        Open();
    }
    public override void Initialize()
    {
        base.Initialize();
        //
        brain.Initialize();
        move.Initialize(rb, model);
        jump.Initialize(rb);
        vision.Initialize(foot1, foot2);
        cerebellum.Initialize();
        //
        cerebellum.onJump += jump.Jump;
        cerebellum.onMove += move.Move;
        
        vision.onArriveGround += () => jump.IsOnGround = true;
        vision.onLeaveGround += () => jump.IsOnGround = false;
    }
    public override void Open()
    {
        base.Open();
        CursorManager.Instance().onJump += (cerebellum as PlayerCerebellum).OnInputJump;
        CursorManager.Instance().onMove += (cerebellum as PlayerCerebellum).OnInputMove;
        //
        move.Open();
        jump.Open();
        vision.Open();
        cerebellum.Open();
        brain.Open();
    }
    public override void Close()
    {
        base.Close();
        CursorManager.Instance().onJump += (cerebellum as PlayerCerebellum).OnInputJump;
        CursorManager.Instance().onMove += (cerebellum as PlayerCerebellum).OnInputMove;
    }
}
