using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Fly : BaseMove
{
    [SerializeField] Vector3 presentSpeedOrient;
    [SerializeField] float multiplePerFrame;

    public override void Move(Vector3 orient)
    {
        if (!canMove)
            return;
        speedMultiple = 1;
        int ori = orient.x > 0 ? 1 : -1;
        if (ori != presentOrient)
        {
            presentOrient = ori;
            model.localScale = new Vector3(ori * Mathf.Abs(model.localScale.x), model.localScale.y, 1);
            onChangeOrient.Invoke(ori > 0 ? true : false);
        }
        //
        presentSpeedOrient = Vector3.Slerp(presentSpeedOrient,orient,multiplePerFrame);
        rb.velocity = presentSpeedOrient * ActualSpeed;
    }
    public override void StopMove()
    {
        speedMultiple = 0;
        presentSpeedOrient = Vector3.zero;
        rb.velocity = Vector3.zero; 
    }
}
