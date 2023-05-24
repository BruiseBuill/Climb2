using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlatformGame.Map
{
    public class MapManager : Single<MapManager>
    {
        [Tooltip("Availble Map Control")]
        [SerializeField] List<BaseMapControl> list = new List<BaseMapControl>();
        [Tooltip("当前层数")]
        [SerializeField] int presentFloor;
        [Tooltip("the order of mapControl after disruption")]
        [SerializeField] int[] indicesFloor;

        [Tooltip("地图尺寸")]
        [SerializeField] Vector2Int mapSize;

        Vector3 mapPos;
        public Vector3 MapPos
        {
            get => mapPos;
        }
        

        private void Awake()
        {
            //2.Disruption
            indicesFloor = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                indicesFloor[i] = i;
            }
            RandomMethod.Disruption<int>(indicesFloor);
            //
        }
        private void Start()
        {
            //1.Initialize
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Initialize(mapSize.x, mapSize.y);
            }
            //list[0].Initialize(mapSize.x, mapSize.y);
        }
        public void MoveMapPos()
        {
            mapPos += new Vector3(0, mapSize.y, 0);
        }
        public void LoadInitialMap()
        {
            mapPos = Vector3.zero;
            presentFloor = 0;
            RenderMap(indicesFloor[presentFloor], mapPos);
            RenderMap(indicesFloor[presentFloor + 1], mapPos + new Vector3(0, mapSize.y, 0));
        }
        public void ChangeMap()
        {
            DeleteMap(indicesFloor[presentFloor]);
            mapPos += new Vector3(0, mapSize.y, 0);
            presentFloor++;
            presentFloor %= indicesFloor.Length;
            RenderMap(indicesFloor[(presentFloor + 1) % indicesFloor.Length], mapPos + new Vector3(0, mapSize.y, 0));
        }
        void RenderMap(int controlIndex, Vector3 startPos)
        {
            list[controlIndex].RenderMap(startPos);
            list[controlIndex].LoadMapData();
        }
        void DeleteMap(int controlIndex)
        {
            list[controlIndex].Close();
        }
#if UNITY_EDITOR
        public void Test_RenderMap()
        {
            list[indicesFloor[presentFloor]].RenderMap(mapPos);
        }
        public void Test_DeleteMap()
        {
            list[indicesFloor[presentFloor]].Close();
        }
#endif
        public Vector2Int WorldPosToGridPos(Vector3 pos)
        {
            return list[indicesFloor[presentFloor]].WorldPosToGridPos(pos);

        }
        public Vector3 GridPosToWorldPos(Vector2Int pos)
        {
            return list[indicesFloor[presentFloor]].GridPosToWorldPos(pos);
        }
        public Vector2Int FindNearestEdge(Vector2Int start, Vector2Int end, FindPathType type)
        {
            return list[indicesFloor[presentFloor]].FindNearestEdge(start, end, type);
        }
        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, FindPathType type)
        {
            return list[indicesFloor[presentFloor]].FindPath(start, end, type);
        }
        public int MaxSuspendHeight(Vector2Int pos, int maxHeight)
        {
            var map = list[indicesFloor[presentFloor]].Map;
            int i = 0;
            for(; i <= maxHeight; i++)
            {
                if (map[pos.x, pos.y + i] != (byte)GroundMapType.Null)
                {
                    break;
                }
            }
            return i;
        }
        public bool IfDirectArrive(Vector2Int higher, Vector2Int lower)
        {
            var map = list[indicesFloor[presentFloor]].Map;
            for (int i = 0; i < higher.y - lower.y; i++)
            {
                if (map[higher.x, lower.y + i] != (byte)GroundMapType.Null || (higher.x - 1 < 0 || map[higher.x - 1, lower.y + i] != (byte)GroundMapType.Null) || (higher.x + 1 > map.GetLength(0) - 1 || map[higher.x + 1, lower.y + i] != (byte)GroundMapType.Null))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

