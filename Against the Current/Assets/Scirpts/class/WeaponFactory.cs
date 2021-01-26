using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory
{
    private DataBase weaponDB;
    public WeaponFactory(DataBase _weaponDB)
    {
        weaponDB = _weaponDB;
    }
    public GameObject CreateWeapon(string weaponName,Vector3 pos,Quaternion rot)
    {
        GameObject weapon = Resources.Load(weaponName) as GameObject;
        GameObject obj=GameObject.Instantiate(weapon,pos,rot);
        obj.AddComponent<WeaponData>();
        WeaponData wdata = obj.AddComponent<WeaponData>();//给武器添加脚本
        wdata.ATK = weaponDB.weaponDataBase["weapon"][weaponName]["ATK"].f;//可以封装
        return obj;
    }
    public Collider CreateWeapon(string weaponName,WeaponManager _wm)
    {
        //这不是好做法
        WeaponControl wc;
        wc = _wm.wcR;
        GameObject weapon = Resources.Load(weaponName) as GameObject;
        GameObject obj=GameObject.Instantiate(weapon);
        obj.transform.parent = wc.transform;

        obj.transform.localPosition = Vector3.zero;//本地
        obj.transform.localRotation = Quaternion.identity;
        WeaponData wdata=obj.AddComponent<WeaponData>();//给武器添加脚本
        //wdata.ATK = weaponDB.weaponDataBase[weaponName]["ATK"].f;//可以封装
        return obj.GetComponent<Collider>();
    }

}
