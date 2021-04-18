﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wd;
    private void Awake()
    {
        wd = GetComponentInChildren<WeaponData>();
    }
    public float GetATK()
    {
        if(wd!=null)
            return wd.ATK;
        Debug.Log("null");
        return 0;
    }
    //public float GetDurable()
    //{
    //    return wd.durable;
    //}
}
