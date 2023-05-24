using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkStrategy 
{
    public abstract bool IfCanLink(Vector2Int start, Vector2Int end, float gridWidth, float jumpLength, float jumpHeight, float HDLDL);
    protected bool IfCanReach(Vector2Int start, Vector2Int end, float gridWidth, float jumpLength, float jumpHeight, float HDLDL)
    {
        float Dx = Mathf.Abs(start.x - end.x) * gridWidth;
        float Dy = (end.y - start.y) * gridWidth;
        return Dx < jumpLength ? (Dy < jumpHeight) : (Dy < jumpHeight - HDLDL * (jumpLength - Dx) * (jumpLength - Dx));
    }
}