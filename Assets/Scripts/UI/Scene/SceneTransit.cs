using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransit : MonoBehaviour
{
    [SceneName]
    [SerializeField] string presentScene;
    [SceneName]
    [SerializeField] string nextScene;

    public void ChangeScene()
    {
        TransitManager.Instance().Transition(presentScene, nextScene);

    }
}
