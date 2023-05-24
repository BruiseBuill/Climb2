using PlatformGame.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGizmos : MonoBehaviour
{
    [SerializeField] bool isGizmos;
    bool hasInitialized;
    List<Vector2Int> list;
    [SerializeField] Vector2 startPos;
    [SerializeField] float gridWidth = 1f;

    public void Initialize(List<Vector2Int> list)
    {
        hasInitialized = true;
        this.list = list;
    }
    private void OnDrawGizmos()
    {
        if (!hasInitialized)
            return;
        for(int i = 0; i < list.Count - 1; i++)
        {
            DrawJumpLine(list[i], list[i + 1]);
        }
    }
    void DrawJumpLine(Vector2Int i,Vector2Int j)
    {
        if (!isGizmos)
            return;
        Gizmos.color = new Color(0.1f, 0.9f, 0.3f);
        Gizmos.DrawLine(
        new Vector3(startPos.x + i.x * gridWidth, startPos.y + i.y * gridWidth, 0f) + MapManager.Instance().MapPos ,
        new Vector3(startPos.x + j.x * gridWidth, startPos.y + j.y * gridWidth, 0f) + MapManager.Instance().MapPos);
    }
}
