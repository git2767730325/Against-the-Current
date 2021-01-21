using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    private Collider weaponColL;
    private Collider weaponColR;
    public GameObject weaponHandleRight;
    public GameObject weaponHandleLeft;
    public WeaponControl wcL;
    public WeaponControl wcR;
    private void Awake()
    {
        weaponHandleRight = transform.DeepFind("rightweapon").gameObject;
        weaponColR = weaponHandleRight.GetComponentInChildren<Collider>();  
        weaponHandleLeft = transform.DeepFind("leftweapon").gameObject;
        weaponColL = weaponHandleLeft.GetComponentInChildren<Collider>();
        wcL = BindWeaponController(weaponHandleLeft);
        wcR = BindWeaponController(weaponHandleRight);
    }

    public WeaponControl BindWeaponController(GameObject obj)
    {
        WeaponControl temp;
        temp = obj.GetComponent<WeaponControl>();
        if (temp == null)
            temp = obj.AddComponent<WeaponControl>();
        temp.wm = this;
        return temp;
    }

    public void UpdateCollider()
    {
        weaponHandleRight = transform.DeepFind("rightweapon").gameObject;
        weaponColR = weaponHandleRight.GetComponentInChildren<Collider>();
    }

    public void DownWeapon()
    {
        foreach (Transform w in wcR.transform)
        {
            Destroy(w.gameObject);//foreach 删除???????????????????????
        }
    }

    public void WeaponEnable()
    {
        weaponColR.enabled = true;
        //weaponCol.isTrigger = true;
    }

    public void WeaponDisable()
    {
        //
        weaponColR.enabled = false;
       // weaponCol.isTrigger = false;
    }
}
