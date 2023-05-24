using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Men : BaseJump
{
    [Tooltip("you can jump when actual speed is less than this")]
    [SerializeField] protected float maxSpeedMultiple;
    [SerializeField] protected float dropGravityScale;
    [SerializeField] protected float jumpSpeedOffset;
    

    public override void Open()
    {
        base.Open();
        jumpSpeedOffset = 0;
    }
    public override void Jump()
    {
        if (canJump)
        {
            if (jumpCount < maxJumpCount && rb.velocity.y <= maxSpeedMultiple * jumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpCount++;
            }
        }
    }
    protected virtual void Update()
    {
        if (rb.velocity.y > 0.1f)
        {
            rb.gravityScale = 1;
        }
        else if (rb.velocity.y < -0.1f) 
        {
            rb.gravityScale = dropGravityScale;
        }
    }
    
}
