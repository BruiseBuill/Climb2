using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlider :MonoBehaviour
{
    float virtualProgress;
    float virtualProgressIncreasePerFrame = 0.01f;
    //
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        virtualProgress = 0f;
        StartCoroutine("Load");
    }
    IEnumerator Load()
    {
        yield return null;
        while (Mathf.Min(GameManager.Instance().ReturnLoadProgress(), virtualProgress) < 0.9f) 
        {
            virtualProgress += virtualProgressIncreasePerFrame;
            slider.value = Mathf.Min(GameManager.Instance().ReturnLoadProgress(), virtualProgress) * 1.1f;
            yield return null;
        }
        GameManager.Instance().SetAllowSceneActivation();
    }
}
