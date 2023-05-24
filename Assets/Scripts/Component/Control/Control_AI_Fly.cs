using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_AI_Fly : Control_AI
{
    public override void Initialize()
    {
        base.Initialize();
        (cerebellum as AICerebellum_Fly).onMove += (move as Move_Fly).Move;
        (cerebellum).onJump += () => (move as Move_Fly).Move(Vector3.up);
        
    }
}
