using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCerebellum : BaseCerebellum
{
    [SerializeField] float cacheTime;
    [SerializeField] List<float> commandTimeList = new List<float>();
    Dictionary<CommandType, Action> dic = new Dictionary<CommandType, Action>();
    

    public override void Initialize()
    {
        base.Initialize();
        commandTimeList.Add(0);
        commandTimeList.Add(0);
        dic.Add(CommandType.Jump, () => { onJump.Invoke(); });
    }
    public override void Open()
    {
        base.Open();
        for (int i = 0; i < commandTimeList.Count; i++)
        {
            //make sure Time.time - commandTimeList[i] > cacheTime when Time.time == 0
            commandTimeList[i] = -cacheTime - 1;
        }
    }
    public void OnInputJump()
    {
        commandTimeList[(int)CommandType.Jump] = Time.time;
    }
    public void OnInputMove(int ori)
    {
        onMove.Invoke(ori);
    }
    private void Update()
    {
        if (canThink)
        {
            for (int i = 0; i < commandTimeList.Count; i++)
            {
                if (Time.time - commandTimeList[i] < cacheTime)
                {
                    dic[(CommandType)i].Invoke();
                }
            }
        }
    }
}
