using PlatformGame.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vision_Men : BaseVision
{
    public UnityAction<Vector3> onAboveGround = delegate { };
    protected Transform foot1;
    protected Transform foot2;
    //
    protected bool isOnGround;
    [SerializeField] protected float footRayLength;
    //

    public override void Initialize<T0, T1>(T0 arg0, T1 arg1)
    {
        foot1= arg0.transform;
        foot2 = arg1.transform;
        //
        collisionLayer = LayerMask.GetMask("Ground");
        
    }
    public override void Open()
    {
        isOnGround = false;
        //
        var r = FindObjectOfType<MapControl_Ruin>();
        Debug.Log(r);
        onAboveGround += r.Delete;
    }
    public override void Close()
    {
        base.Close();
        var r = FindObjectOfType<MapControl_Ruin>();
        onAboveGround -= r.Delete;
    }
    protected virtual void Update()
    {
        CheckOnGround();
    }
    protected virtual void CheckOnGround()
    {
        var hit1 = Physics2D.Raycast(foot1.position, Vector2.down, footRayLength, collisionLayer);
        var hit2 = Physics2D.Raycast(foot2.position, Vector2.down, footRayLength, collisionLayer);
        if (hit1 || hit2)
        {
            if (!isOnGround)
            {
                isOnGround = true;
                onArriveGround.Invoke();
            }
        }
        else
        {
            if (isOnGround)
            {
                isOnGround = false;
                onLeaveGround.Invoke();
            }
        }
        if (hit1)
        {
            onAboveGround.Invoke(foot1.position + Vector3.down * footRayLength);
        }
        if (hit2)
        {
            onAboveGround.Invoke(foot2.position + Vector3.down * footRayLength);
        }
    }
}
