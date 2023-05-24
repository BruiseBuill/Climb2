using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlatformGame.Map;

public class MapEditor : EditorWindow
{
    [SerializeField] bool a = false;

    [MenuItem("Tools/MapWindow")]
    static void Create()
    {
        GetWindow(typeof(MapEditor));
    }
    private void OnGUI()
    {
        GUILayout.Label("MapEditor");
        //
        string presentIndex = "";
        presentIndex = GUILayout.TextField(presentIndex);
        if (GUILayout.Button("TestRender"))
        {
            MapManager.Instance().Test_RenderMap();
        }
        if (GUILayout.Button("TestClear"))
        {
            MapManager.Instance().Test_DeleteMap();
        }
        if (GUILayout.Button("Initialize"))
        {
            MapManager.Instance().LoadInitialMap();
        }
        if (GUILayout.Button("ChangeMap"))
        {
            MapManager.Instance().ChangeMap();
        }
    }
}
