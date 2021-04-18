using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFX : ObjectPool
{
    GameObject[] vfxs = new GameObject[6];
    //int ia;
    void Awake()
    {
        durTime = 0.7f;
        FillPool(6);
        //for (int i = 0; i < 6; i++)
        //{
        //    vfxs[i]= transform.GetChild(i).gameObject;
        //}
    }

    // Update is called once per frame
    /*void Update()
    {
        if (objectsPool.Count <= 0 && ia ==0)
        {
            StartCoroutine("fbxwait");
        }
    }*/

    /*
    IEnumerator fbxwait()
    {
        ia = 1;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 6; i++)
        {
            ReturnPool(vfxs[i]);
            ia = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }
    */
}
