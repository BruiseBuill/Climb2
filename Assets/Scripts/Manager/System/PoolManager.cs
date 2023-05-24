using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Single<PoolManager>
{
    [SerializeField] Pool[] bulletPool;

    Dictionary<string, Pool> dictionary = new Dictionary<string, Pool>();

    protected void Awake()
    {
        int i = 0;
        for (; i < bulletPool.Length; i++)
        {
            bulletPool[i].Initialize(transform);
            dictionary.Add(bulletPool[i].prefab.name, bulletPool[i]);
        }
    }
    public GameObject Release(string a)
    {
        return dictionary[a].GetFromPool();
    }
    public void Recycle(GameObject a)
    {
        dictionary[a.name].BackToPool(a);
    }
}
