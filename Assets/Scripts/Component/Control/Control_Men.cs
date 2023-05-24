using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_Men : BaseControl
{
    protected BaseJump jump;
    protected BaseMove move;
    protected BaseVision vision;
    protected BaseCerebellum cerebellum;
    //protected BaseHealth health;
    //protected BaseAttack attack;

    protected override void Awake()
    {
        base.Awake();

        jump = GetComponentInChildren<BaseJump>();
        move = GetComponentInChildren<BaseMove>();
        vision = GetComponentInChildren<BaseVision>();
        cerebellum = GetComponentInChildren<BaseCerebellum>();
        //health = GetComponentInChildren<BaseHealth>();
        //attack = GetComponentInChildren<BaseAttack>();
    }

}
