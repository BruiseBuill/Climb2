using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDieCP : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [Header("Ïú»Ù·½Ê½")]
    [SerializeField] bool isDestroy;
    [SerializeField] bool isSetActiveFalse;
    [SerializeField] bool isReturnPool;
    WaitForSeconds wait_Die;

    private void Awake()
    {
        wait_Die = new WaitForSeconds(lifeTime);
    }
    private void OnEnable()
    {
        StartCoroutine("Die");
    }
    IEnumerator Die()
    {
        yield return wait_Die;
        if (isDestroy)
        {
            Destroy(gameObject);
        }
        else if (isSetActiveFalse)
        {
            gameObject.SetActive(false);
        }
        else if (isReturnPool)
        {
            PoolManager.Instance().Recycle(gameObject);
        }
    }
}
