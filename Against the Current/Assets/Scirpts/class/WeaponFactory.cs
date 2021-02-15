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
        wdata.ATK = weaponDB.weaponDataBase["weapon"][weaponName]["DEF"].f;//可以封装
        //wdata.durable= weaponDB.weaponDataBase["weapon"][weaponName]["durable"].f;
        return obj;
    }
    public Collider CreateWeapon(string weaponName,WeaponManager _wm)
    {
        //这不是好做法
        Vector3 localpos;
        Quaternion localrot;
        WeaponControl wc;
        wc = _wm.wcL;
        GameObject weapon = Resources.Load(weaponName) as GameObject;
        localpos = weapon.transform.position;
        localrot = weapon.transform.rotation;
        GameObject obj=GameObject.Instantiate(weapon);
        obj.transform.parent = wc.transform;

        obj.transform.localPosition = localpos;//本地
        obj.transform.localRotation = localrot;
        WeaponData wdata=obj.AddComponent<WeaponData>();//给武器添加脚本
         //wdata.ATK = weaponDB.weaponDataBase[weaponName]["ATK"].f;//可以封装
        wdata.DEF = weaponDB.weaponDataBase["weapon"][weaponName]["DEF"].f;//可以封装
        //wdata.durable = weaponDB.weaponDataBase["weapon"][weaponName]["durable"].f;
        return obj.GetComponent<Collider>();
    }

}
