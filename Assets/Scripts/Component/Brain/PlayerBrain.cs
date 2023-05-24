using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBrain : BaseBrain
{
    [Tooltip("Character")]
    [SerializeField] protected float flexibility = 1f;
    [SerializeField] protected int level = 1;
    [SerializeField] protected float attackPosYOffset;

    public int Level
    {
        get { return level; }
    }
}
