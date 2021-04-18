using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SIght : MonoBehaviour
{
    public GameObject enemy;
    public bool isInSight;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player"||col.tag=="Separation")
        {
            isInSight = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player"||col.tag=="Separation")
        {
            isInSight = false;
        }
    }
}
