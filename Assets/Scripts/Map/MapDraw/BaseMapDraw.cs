using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseMapDraw : MonoBehaviour
{
    protected int length;
    protected int height;
    [SerializeField] protected Tilemap[] tileMap;
    protected Vector3 startPos;
    protected Vector3 cellSize;

    public virtual void Initialize(int length, int height)
    {
        this.length = length;
        this.height = height;
        //tileMap.transform.position = pos;
    }
    public virtual void DrawMap(byte[,] map, Vector3 pos)
    {
        length = map.GetLength(0);
        height = map.GetLength(1);
        tileMap[(int)TileMapKind.Map].transform.position = pos;
        tileMap[(int)TileMapKind.Wall].transform.position = pos;
        tileMap[(int)TileMapKind.Decorate].transform.position = pos;
        startPos = tileMap[(int)TileMapKind.Map].CellToWorld(Vector3Int.zero);
        cellSize = tileMap[(int)TileMapKind.Map].cellSize;
        cellSize.z = 0;
    }
    public virtual void ClearMap()
    {

    }
    public Vector2Int WorldPosToGridPos(Vector3 pos)
    {
        Vector3 offset = pos - startPos;
        return new Vector2Int((int)(offset.x / cellSize.x), (int)(offset.y / cellSize.y));
    }
    public Vector3 GridPosToWorldPos(Vector2Int pos)
    {
        return new Vector3(pos.x * cellSize.x, pos.y * cellSize.y, 0) + cellSize * 0.5f + startPos;
    }
}
