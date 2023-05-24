using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCerebellum : MonoBehaviour
{
    public UnityAction<int> onMove = delegate { };
    public UnityAction onJump = delegate { };

    [SerializeField] protected bool canThink;

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
    public virtual void Initialize<T>(T t)
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
    public virtual void Move(Vector2Int pos)
    {

    }
    public virtual void Jump()
    {

    }
}
