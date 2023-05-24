using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitManager : Single<TransitManager>
{
    [SceneName]
    [SerializeField] string startScene;
    [SerializeField] bool isFade;
    [SerializeField] CanvasGroup group;
    [SerializeField] float time;

    private void Start()
    {
        Transition(null, startScene);
    }
    public void Transition(string from, string to)
    {
        StartCoroutine(ChangeScene(from, to));
    }
    IEnumerator ChangeScene(string from, string to)
    {
        yield return Fade(1);
        if (from != null)
        {
            yield return SceneManager.UnloadSceneAsync(from);
        }

        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(scene);

        yield return Fade(0);
    }
    IEnumerator Fade(float alpha)
    {
        isFade = true;
        group.blocksRaycasts = true;
        float speed = Mathf.Abs(group.alpha - alpha) / time;

        while (!Mathf.Approximately(group.alpha, alpha))
        {
            group.alpha = Mathf.MoveTowards(group.alpha, alpha, speed * Time.deltaTime);
            yield return null;
        }
        group.blocksRaycasts = false;
        isFade = false;
    }
}
