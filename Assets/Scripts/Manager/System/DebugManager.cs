using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : Single<DebugManager>
{
    [SerializeField] bool isUIDebug = true;
    [SerializeField] bool isLogicDebug = true;

    public void UIDebug(object ob)
    {
        if (isUIDebug)
        {
            Debug.Log(ob.ToString());
        }
    }
}
