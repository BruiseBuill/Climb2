using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Move_Men : BaseMove
{
    public override void Move(int orient)
    {
        if (!canMove)
            return;
        if (orient == 0)
        {
            StopMove();
            return;
        }
        else
            speedMultiple = 1;
        if (orient != presentOrient)
        {
            presentOrient = orient;
            model.localScale = new Vector3(orient * Mathf.Abs(model.localScale.x), model.localScale.y, 1);
            onChangeOrient.Invoke(orient > 0 ? true : false);
        }
        rb.velocity = new Vector2(ActualSpeed * orient, rb.velocity.y);
    }
    public override void StopMove()
    {
        speedMultiple = 0;
        rb.velocity = new Vector2(ActualSpeed * presentOrient, rb.velocity.y);
    }
}
