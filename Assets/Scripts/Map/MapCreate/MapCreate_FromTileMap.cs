using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreate_FromTileMap : BaseMapCreate
{
    [SerializeField] Tilemap tilemap;
    [Tooltip("Tile in the tilemap")]
    [SerializeField] TileBase[] tiles;
    [SerializeField] GroundMapType tileType;
    Dictionary<TileBase, byte> dic = new Dictionary<TileBase, byte>();

    public override void Initialize(int length, int height)
    {
        base.Initialize(length, height);
        for(int i = 0; i < tiles.Length; i++)
        {
            dic.Add(tiles[i], (byte)tileType);
        }
    }
    public override void Create()
    {
        for (int j = 1; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
                var t = tilemap.GetTile(new Vector3Int(i, j, 0));
                if (dic.ContainsKey(t))
                {
                    map[i, j] = dic[t];
                }
                else
                {
                    map[i, j] = (byte)GroundMapType.Null;
                }
            }
        }
    }
}
