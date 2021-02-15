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

    [Header("======= 频繁切换的武器  =======")]
    public GameObject gun;
    public GameObject sword;
    public SwordVFX svfx;
    public GameObject vfxTarget;
    private void Awake()
    {
        weaponHandleRight = transform.DeepFind("rightweapon").gameObject;
        weaponColR = weaponHandleRight.GetComponentInChildren<Collider>();  
        weaponHandleLeft = transform.DeepFind("leftweapon").gameObject;
        weaponColL = weaponHandleLeft.GetComponentInChildren<Collider>();
        wcL = BindWeaponController(weaponHandleLeft);
        wcR = BindWeaponController(weaponHandleRight);
    }

    private void Start()
    {
        if (this.gameObject.tag == "Player")
        {
            GameManager.GM.BindWM(this);
            Debug.Log("gmbindwm");
        }
        //if (this.gameObject.name == "ybot")
        //{
        //    GameManager.GM.BindWM(this);
        //    Debug.Log("gbbindwm");
        //}
    }


    //特效对象池
    public void OpenSwordVFX()
    {
        GameObject obj = svfx.DeQueue();
        obj.transform.position = vfxTarget.transform.position;
        obj.transform.rotation = vfxTarget.transform.rotation;
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
        foreach (Transform w in wcL.transform)
        {
            Destroy(w.gameObject);//foreach 删除???????????????????????
        }
    }

    public void ChangeWeapon(bool isGun)
    {
        if(isGun)
        {
            wcR.wd.gameObject.SetActive(false);
            wcR.wd = gun.GetComponent<WeaponData>();
            wcR.wd.gameObject.SetActive(true);
        }
        else
        {
            wcR.wd.gameObject.SetActive(false);
            wcR.wd = sword.GetComponent<WeaponData>();
            wcR.wd.gameObject.SetActive(true);
        }
    }




    public void WindyEnable()
    {
        am.sm.canWindy = true;//不是很好的写法
    }

    public void WindyDisable()
    {
        am.sm.canWindy = false;
    }

    public void CounterbackEnable()
    {
        am.sm.canCounterBack = true;
    }
    public void CounterbackDisable()
    {
        am.sm.canCounterBack = false;
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
