using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Map
{
    public class BaseMapControl : MonoBehaviour
    {
        protected BaseMapCreate create;
        protected BaseMapDraw draw;
        protected BaseMapStore store;

        protected byte[,] map;
        public byte[,] Map
        {
            get => map;
        }

        protected virtual void Awake()
        {
            create = GetComponentInChildren<BaseMapCreate>();
            draw = GetComponentInChildren<BaseMapDraw>();
            store = GetComponentInChildren<BaseMapStore>();
        }
        public virtual void Initialize(int length, int height) { }
        public virtual void Open() { }
        public virtual void Close() { }
        public virtual void Create() { }
        public virtual void Save()
        {
            store.Save();
        }
        public virtual void RenderMap(Vector3 pos) { }
        public virtual void LoadMapData() { }

        public Vector2Int WorldPosToGridPos(Vector3 pos)
        {
            return draw.WorldPosToGridPos(pos);
        }
        public Vector3 GridPosToWorldPos(Vector2Int pos)
        {
            return draw.GridPosToWorldPos(pos);
        }
        public virtual Vector2Int FindNearestEdge(Vector2Int start, Vector2Int end, FindPathType type)
        {
            return Vector2Int.zero;
        }
        public virtual List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, FindPathType type)
        {
            return null;
        }
    }
}

