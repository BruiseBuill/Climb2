using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Map
{
    public class MapControl_Ruin : MapControl_Normal
    {
        public void Delete(Vector3 pos)
        {
            (draw as MapDraw_Ruin).Delete(pos);
        }
    }
}

