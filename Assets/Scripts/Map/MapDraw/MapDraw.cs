using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDraw : BaseMapDraw
{
    [Header("绘制地图的各种砖块")]
    [SerializeField] protected List<TileBase> tiles;

    public override void ClearMap()
    {
        tileMap[(int)TileMapKind.Map].ClearAllTiles();
    }
    public override void DrawMap(byte[,] map, Vector3 pos)
    {
        base.DrawMap(map, pos);
        for (int i = 1; i < length - 1; i++) 
        {
            for (int j = 1; j < height - 1; j++) 
            {
                if (map[i, j] == (byte)GroundMapType.Null) 
                {
                    //null 
                    continue;
                }
                tileMap[(int)TileMapKind.Map].SetTile(new Vector3Int(i, j, 0), tiles[map[i, j]]);
            }
        }
        for (int i = -50; i < length +50; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                if (i<1||i>length-2||j<1||j>height-2)
                {
                    tileMap[(int)TileMapKind.Wall].SetTile(new Vector3Int(i, j, 0), tiles[(int)GroundMapType.Wall]);
                }
                
            }
        }

    }
    
}
