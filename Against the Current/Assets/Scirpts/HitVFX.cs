using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVFX : ObjectPool
{
    private void Awake()
    {
        FillPool();
    }

    private void Update()
    {
        if(objectsPool.Count<=0)
        {
            FillPool();
        }
    }

}
