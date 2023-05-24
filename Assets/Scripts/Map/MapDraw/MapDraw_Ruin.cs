using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDraw_Ruin : MapDraw
{
    [SerializeField] float delayTime;
    [SerializeField] float restoreTime;
    [SerializeField] List<Vector2Int> posList = new List<Vector2Int>();
    [SerializeField] List<TileBase> tileList = new List<TileBase>();
    WaitForSeconds wait_DelayTime;
    WaitForSeconds wait_RestoreTime;
    //

    LayerMask layer;

    public override void Initialize(int length, int height)
    {
        base.Initialize(length, height);
        wait_DelayTime = new WaitForSeconds(delayTime);
        wait_RestoreTime = new WaitForSeconds(restoreTime);

        layer = LayerMask.GetMask("Player", "Enemy");
    }
    public void Delete(Vector3 pos)
    {
        var grid = WorldPosToGridPos(pos);
        if (posList.Contains(grid))
        {
            return;
        }
        posList.Add(grid);
        tileList.Add(tileMap[(int)TileMapKind.Map].GetTile((Vector3Int)grid));
              
        StartCoroutine("Deleting", grid);
    }
    IEnumerator Deleting(Vector2Int pos)
    {
        yield return wait_DelayTime;
        tileMap[(int)TileMapKind.Map].SetTile((Vector3Int)pos, null);
        StartCoroutine("Restoring", pos); 
    }
    IEnumerator Restoring(Vector2Int pos)
    {
        yield return wait_RestoreTime;
        var col = Physics2D.OverlapBox(GridPosToWorldPos(pos), cellSize, 0, layer);
        if (col == null)
        {
            int index = posList.IndexOf(pos);
            tileMap[(int)TileMapKind.Map].SetTile((Vector3Int)pos, tileList[index]);
            Restore(index);  
        }
        else
        {
            yield return StartCoroutine("Restoring", pos);
        }
    }
    void Restore(int index)
    {
        posList.RemoveAt(index);
        tileList.RemoveAt(index);
    }
}
