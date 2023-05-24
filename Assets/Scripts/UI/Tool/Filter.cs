using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    [SerializeField] bool isHorizontal;
    [SerializeField] float space;
    RectTransform rectTransform;
    //
    WaitForEndOfFrame WaitForEndOfFrame;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        WaitForEndOfFrame = new WaitForEndOfFrame();
    }
    private void OnEnable()
    {
        StartCoroutine("Wait");
    }
    IEnumerator Wait()
    {
        yield return WaitForEndOfFrame;
        int i = 0;
        if (!isHorizontal)
        {
            float totalHight = 0;
            for (; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    totalHight += transform.GetChild(i).GetComponent<RectTransform>().rect.height;
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, totalHight + (transform.childCount - 1) * space);
        }
        else
        {
            float totalWidth = 0;
            for (; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    totalWidth += transform.GetChild(i).GetComponent<RectTransform>().rect.width;
            }
            rectTransform.sizeDelta = new Vector2(totalWidth + (transform.childCount - 1) * space, rectTransform.rect.height);
        }
    }
}
