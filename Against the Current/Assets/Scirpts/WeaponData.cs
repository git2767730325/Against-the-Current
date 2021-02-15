using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    private WeaponControl wc;
    public float ATK = 2.0f;
    //public float durable = 100;
    public float DEF = 1.0f;
    private void Start()
    {
        wc = GetComponentInParent<WeaponControl>();
        wc.wd = this;
    }
  
}
