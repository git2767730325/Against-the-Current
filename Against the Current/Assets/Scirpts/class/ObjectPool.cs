using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> objectsPool = new Queue<GameObject>();
    public GameObject prefab;
    public float durTime=3f;
    public void FillPool(int number=5)
    {
        for(int i=0;i<number;i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.parent = transform;
            ReturnPool(obj);
        }
    }

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
    protected IEnumerator ReturnPoolOnTime(GameObject _g)
    {
        GameObject g = _g;
        yield return new WaitForSeconds(durTime);
        if(g.activeSelf)
        ReturnPool(g);
    }

    public GameObject UseTimePool()
    {
        GameObject g = DeQueue();
        StartCoroutine("ReturnPoolOnTime", g);
        return g;
    }

}
