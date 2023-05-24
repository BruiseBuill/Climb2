using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Single<CursorManager>
{
    public Action onJump = delegate { };
    public Action<int> onMove = delegate { };

    [SerializeField] int ADstate = 0;

    private void Awake()
    {
        InputManager.onClickW += OnJump;
        InputManager.onPressA += () =>
        {
            ADstate = ADstate | 1;
            OnMove();
        };
        InputManager.onPressD += () =>
        {
            ADstate = ADstate | 2;
            OnMove();
        };
        InputManager.onRedoPressA += () =>
        {
            ADstate = ADstate & 2;
            OnMove();
        };
        InputManager.onRedoPressD += () =>
        {
            ADstate = ADstate & 1;
            OnMove();
        };
    }

    void OnJump()
    {
        onJump.Invoke();
    }
    void OnMove()
    {
        if (ADstate == 1)
        {
            onMove.Invoke(-1);
        }
        else if (ADstate == 2) 
        {
            onMove.Invoke(1);
        }
        else 
        {
            onMove.Invoke(0);
        }
    }
}
