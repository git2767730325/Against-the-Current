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
    public GameObject lightSword;
    public GameObject vfxTarget;
    [Header("bool，避免频繁切换武器")]
    public bool gunState = false;
    public bool nlState = false;
    [Header("=======    需要对象池的特效      ==========")]
    public SwordVFX svfx;
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
        if (transform.parent.gameObject.tag == "Player")
        {
            AudioManager.SetPlayerSource(4);
            AudioManager.PlayPlayeraudioSource();
            GameObject obj = svfx.UseTimePool();
            obj.transform.position = vfxTarget.transform.position;
            obj.transform.rotation = vfxTarget.transform.rotation;
        }
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
        //过于频繁
        if(isGun&&!gunState)
        {
            wcR.wd.gameObject.SetActive(false);
            wcR.wd = gun.GetComponent<WeaponData>();
            wcR.wd.gameObject.SetActive(true);
            gunState = true;
            nlState = false;
        }
        else if(!isGun&&!nlState)
        {
            wcR.wd.gameObject.SetActive(false);
            wcR.wd = sword.GetComponent<WeaponData>();
            wcR.wd.gameObject.SetActive(true);
            nlState = true;
            gunState = false;
        }
    }


    //脚步声音相关
    public void AudioMove()
    {
        if (transform.parent.gameObject.tag != "Player")
            return;
            Debug.Log("声音");
        AudioManager.SetPlayerSource(0);
        AudioManager.PlayPlayeraudioSource();
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

    //更换光剑
    public void PreDoomOath()
    {
        if (lightSword == null)
            return;
        Time.timeScale = 0.25f;
    }
    public void DoomOath()
    {
        if (lightSword == null)
            return;
        Time.timeScale = 1f;
        if (lightSword == null)
            return;
        wcR.wd.gameObject.SetActive(false);
        wcR.wd = lightSword.GetComponent<WeaponData>();
        wcR.wd.gameObject.SetActive(true);
    }

}
