using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field,Inherited =true)]
public class SceneNameAttribute :PropertyAttribute
{
    public SceneNameAttribute()
    {

    }
}

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class TDArrayAttribute : PropertyAttribute
{
    public int length;
    public int height;
    public TDArrayAttribute(int length,int height)
    {
        this.length = length;
        this.height = height;
    }
}