using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PlatformGame.Map
{
    public class MapControl_Normal : BaseMapControl
    {
        [SerializeField] PathCreate pathCreate;
        List<PathFind> pathFind;
        [SerializeField] MapGizmos gizmos;
        [SerializeField] float maxJumpHeight;
        [SerializeField] float maxJumpLength;
        [SerializeField] float maxLinkDistance;

        protected override void Awake()
        {
            base.Awake();
            pathFind = new List<PathFind>();
            pathFind.Add(GetComponentInChildren<PathFind_Jump>());
            pathFind.Add(GetComponentInChildren<PathFind_Fly>());
        }
        public override void Initialize(int length, int height)
        {
            create.Initialize(length, height);
            draw.Initialize(length, height);
            store.Initialize(length, height);
            pathCreate.Initialize(length, height, maxJumpLength, maxJumpHeight, maxLinkDistance);

            if (store.MapCount() < 2)
            {
                Create();
            }
        }
        public override void Create()
        {
            ThreadStart start = new ThreadStart(() =>
            {
                Debug.Log("StartCreate:" + System.DateTime.Now.Second);
                create.Create();
                pathCreate.CreatePath(create.Map);
                store.Add(create.Map, pathCreate.PathPointDic);
                store.Save();

                Debug.Log("EndCreate:" + +System.DateTime.Now.Second);
            });
            Debug.Log(gameObject.name);
            Thread thread = new Thread(start);
            thread.Start();
        }
        public override void Open()
        {

        }
        public override void Close()
        {
            draw.ClearMap();
            store.Delete();
            if (store.MapCount() == 0)
            {
                Create();
            }
        }
        public override void LoadMapData()
        {
            if (store.MapCount() == 0)
            {
                Create();
                Debug.Log("No map");
                return;
            }
            var m = store.GetMapData();
            map = m;
        }
        public override void RenderMap(Vector3 pos)
        {
            if (store.MapCount() == 0)
            {
                Create();
                Debug.Log("No map");
                return;
            }
            var m = store.GetMapData();
            draw.DrawMap(m, pos);
#if UNITY_EDITOR
            gizmos.Initialize((store as MapStore_Jump).GetPathPointDic());
#endif
        }
        public override Vector2Int FindNearestEdge(Vector2Int start, Vector2Int end, FindPathType type)
        {
            switch (type)
            {
                case FindPathType.Fly:
                    return pathFind[(int)FindPathType.Fly].FindNearestEdge(start, end, store.GetMapData());
                case FindPathType.Jump:
                    return pathFind[(int)FindPathType.Jump].FindNearestEdge(start, end, (store as MapStore_Jump).GetPathPointDic());
            }
#if UNITY_EDITOR
            Debug.LogError("1");
#endif
            return Vector2Int.zero;
        }
        public override List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, FindPathType type)
        {
            switch (type)
            {
                case FindPathType.Fly:
                    return pathFind[(int)FindPathType.Fly].FindPath(start, end, store.GetMapData());
                case FindPathType.Jump:
                    return pathFind[(int)FindPathType.Jump].FindPath(start, end, (store as MapStore_Jump).GetPathPointDic());
            }
#if UNITY_EDITOR
            Debug.LogError("1");
#endif
            return null;
        }
    }
}
