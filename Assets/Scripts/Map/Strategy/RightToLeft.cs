using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightToLeft : LinkStrategy
{
    public override bool IfCanLink(Vector2Int start, Vector2Int end, float gridWidth, float jumpLength, float jumpHeight, float HDLDL)
    {
        if (start.y == end.y)
        {
            if (start.x < end.x)
            {
                return IfCanReach(start, end, gridWidth, jumpLength, jumpHeight, HDLDL);
            }
            return false;
        }
        else if (start.y < end.y)
        {
            if (start.x < end.x)
            {
                return IfCanReach(start, end, gridWidth, jumpLength, jumpHeight, HDLDL);
            }
            return false;
        }
        else
        {
            if (start.x < end.x)
            {
                return IfCanReach(start, end, gridWidth, jumpLength, jumpHeight, HDLDL);
            }
            return false;
        }
    }
}
