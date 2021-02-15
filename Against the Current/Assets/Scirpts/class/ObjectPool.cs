using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> objectsPool = new Queue<GameObject>();
    public GameObject prefab;

    public void FillPool(int number=5)
    {
        for(int i=0;i<number;i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.parent = transform;
            ReturnPool(obj);
        }
    }
    //可重载
    
    //public void FillPool(Transform trans,int number = 5)
    //{
    //    for (int i = 0; i < number; i++)
    //    {
    //        GameObject obj = GameObject.Instantiate(prefab, trans.position
    //            , trans.rotation, transform.parent);
    //        ReturnPool(obj);
    //    }
    //}

    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
        objectsPool.Enqueue(obj);
    }

    public GameObject DeQueue()
    {
        if (objectsPool.Count <= 0)
            return null;
        GameObject obj=objectsPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }



}
