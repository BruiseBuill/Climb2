using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseMove : MonoBehaviour
{
    public UnityAction<bool> onChangeOrient = delegate { };

    protected Rigidbody2D rb;
    protected Transform model;

    [SerializeField] protected int presentOrient;
    [SerializeField] protected bool canMove = true;   //
    [SerializeField] protected float speed;
    [SerializeField] protected float speedOfOutside;
    [Tooltip("speedMultiple==0 when canMove==false or stopMove")]
    [SerializeField] protected float speedMultiple;
    [Tooltip("character's additional speed")]
    [SerializeField] protected float speedIncrement;

    public float ActualSpeed
    {
        get => ((speed + speedIncrement) * speedMultiple + speedOfOutside);
    }

    public bool CanMove
    {
        set
        {
            canMove = value;
            if (canMove == false)
                StopMove();
        }
    }

    public virtual void Initialize(Rigidbody2D rb, Transform model)
    {
        this.rb = rb;
        this.model = model;
    }
    public virtual void Open()
    {
        canMove = true;
        speedOfOutside = 0;
        speedMultiple = 1;
        speedIncrement = 0;
    }
    public virtual void Close()
    {

    }
    public virtual void Move(int orient)
    {
          
    }
    public virtual void Move(Vector3 orient)
    {

    }
    public virtual void StopMove()
    {
        
    }
}
