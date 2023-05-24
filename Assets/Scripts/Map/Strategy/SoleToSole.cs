using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoleToSole: LinkStrategy
{
    public override bool IfCanLink(Vector2Int start, Vector2Int end, float gridWidth, float jumpLength, float jumpHeight, float HDLDL)
    {
        if (start.x != end.x)
            return IfCanReach(start, end, gridWidth, jumpLength, jumpHeight, HDLDL);
        else
            return false;
    }
}
