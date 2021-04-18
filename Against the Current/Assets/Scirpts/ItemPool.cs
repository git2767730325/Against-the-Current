using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool
{
    public GameObject[] items = new GameObject[8];
    private void Awake()
    {
        durTime = 30f;
        FillPool(8);
        int i = 0;
        foreach (var item in objectsPool)
        {
            
            items[i]=item;
            i++;
        }
    }
    private void Update()
    {
        if (objectsPool.Count <= 0)
        {
            ReturnPool(items[Random.Range(0, 7)]);
        }
    }
}
