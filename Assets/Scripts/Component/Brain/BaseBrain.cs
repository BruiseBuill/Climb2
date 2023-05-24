using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseBrain : MonoBehaviour
{
    //public UnityAction<int> onMove = delegate { };
    //public UnityAction onJump = delegate { };

    [SerializeField] protected bool canThink = false;
    public bool CanThink
    {
        set
        {
            canThink = value;
        }
    }

    public virtual void Initialize()
    {
        
    }
    public virtual void Initialize<T0>(T0 t0) where T0 : Component
    {

    }
    public virtual void Open()
    {
        canThink = true;
    }
    public virtual void Close()
    {
        canThink = false;
    }

}
