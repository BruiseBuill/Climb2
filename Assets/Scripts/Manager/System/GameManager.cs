using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

/*
#if UNITY_EDITOR
using UnityEditor;
#endif*/

public class GameManager : Single<GameManager>
{
    AsyncOperation async;
    int sceneIndex;

    private void Awake()
    {
        if (Instance().gameObject != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        //
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneAppear;
    }
    private void OnSceneAppear(Scene arg0, LoadSceneMode arg1)
    {
        //
    }
    public void LoadScene(int index)
    {
        if (async == null)
        {
            async = SceneManager.LoadSceneAsync(index);
            async.allowSceneActivation = false;
            sceneIndex = index;
        }
    }
    public void SetAllowSceneActivation(bool a = true)
    {
        async.allowSceneActivation = a;
        async = null;
    }
    public float ReturnLoadProgress()
    {
        return async.progress;
    }
    public int ReturnSceneIndex()
    {
        return sceneIndex;
    }
    public void ExitGame()
    {
        /*
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif*/
        Application.Quit();
    }
}
