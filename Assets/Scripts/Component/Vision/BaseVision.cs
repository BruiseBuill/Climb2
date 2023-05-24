using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseVision : MonoBehaviour
{
    public UnityAction onArriveGround = delegate { };
    public UnityAction onLeaveGround = delegate { };
    protected LayerMask collisionLayer;

    public virtual void Initialize<T0,T1>(T0 arg0,T1 arg1) where T0 : Component where T1 :Component
    {

    }
    public virtual void Initialize<T0, T1, T2>(T0 t0, T1 t1, T2 t2) where T0 : Component where T1 : Component where T2 : Component
    {

    }
    public virtual void Open()
    {

    }
    public virtual void Close()
    {

    }
}
