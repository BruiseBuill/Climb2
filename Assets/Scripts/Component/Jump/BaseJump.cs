using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseJump : MonoBehaviour
{
    [SerializeField] protected bool canJump = true;

    [SerializeField] protected int jumpCount;

    [SerializeField] protected bool isOnGround;
    [SerializeField] protected float jumpSpeed;

    [SerializeField] protected int maxJumpCount = 2;
    protected Rigidbody2D rb;

    public int RemainJumpCount
    {
        get => (maxJumpCount - jumpCount);
    }

    public bool IsOnGround
    {
        get { return IsOnGround; }
        set {
                if (value == true)
                {
                    jumpCount = 0;
                    isOnGround = true;
                }
                else
                isOnGround = false;
        }
    }
    public bool CanJump
    {
        set => canJump = value;
    }

    
    public virtual void Initialize(Rigidbody2D rb)
    {
        this.rb = rb;
    }
    public virtual void Open()
    {
        canJump = true;
    }
    public virtual void Close()
    {

    }

    public virtual void Jump()
    {

    }

}
