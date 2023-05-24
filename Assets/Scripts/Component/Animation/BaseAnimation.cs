using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    protected Animator animator;

    public virtual void Initiaiize(Animator animator)
    {
        this.animator = animator;
    }
    public virtual void Open()
    {

    }
    public virtual void Close() 
    {
        
    }

}
