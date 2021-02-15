using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : ObjectPool
{
    void Awake()
    {
        FillPool(10);
    }

    // Update is called once per frame
    void Update()
    {
        if(objectsPool.Count<=0)
        {
            FillPool();
        }
    }
}
