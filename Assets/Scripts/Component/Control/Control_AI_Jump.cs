using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_AI_Jump : Control_AI
{
    public override void Initialize()
    {
        base.Initialize();
        cerebellum.onJump += jump.Jump;
        (cerebellum as AICerebellum_Jump).onAcquireMoveSpeed += () => move.ActualSpeed;
        (cerebellum as AICerebellum_Jump).onAcquireJumpCount += () => jump.RemainJumpCount;
        //(cerebellum as AICerebellum_Jump).onAcquireRemainJumpCount+=
       (vision as Vision_AI).onBelowGround += (obs) => (cerebellum as AICerebellum_Jump).HeadObstruction = obs;
        (vision as Vision_AI).onHalfOnGround += (cerebellum as AICerebellum_Jump).IfJump;
    }
}
