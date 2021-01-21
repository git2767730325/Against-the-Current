using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager GM;
    public WeaponManager tempwm;
    private GameObject prefab;
    private DataBase weaponDB;
    private WeaponFactory weaponFactory;
    // Start is called before the first frame update
    void Awake()
    {
        //非标准单例模式
        if(transform.tag=="GM"&&GM==null)
        {
            GM = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
    }
    private void Start()
    {
        InitDataBase();
        InitWeaponFactory();
        prefab = Resources.Load("sword") as GameObject;
        print(weaponDB.weaponDataBase);
        //GameObject a=weaponFactory.CreateWeapon("sword",transform.position,Quaternion.identity);
       // GameObject aa = weaponFactory.CreateWeapon("sword",tempwm);
        Collider col=weaponFactory.CreateWeapon("sword", tempwm);//col没用，如果更新不输入
        tempwm.UpdateCollider();
    }
    // Update is called once per frame
    void Update()
    {
        //测试UI
        if (Input.GetKeyDown(KeyCode.U))
        {
        }

    }
    private void OnGUI()//不是游戏用UI，只是工程用
    {
        if(GUI.Button(new Rect(10,10,150,30),"sword"))
        {
            tempwm.DownWeapon();
            Collider col = weaponFactory.CreateWeapon("sword", tempwm);//col没用，如果更新不输入
            tempwm.UpdateCollider();
        }

        if (GUI.Button(new Rect(10, 50, 150, 30), "blade"))
        {
            Collider col = weaponFactory.CreateWeapon("blade", tempwm);//col没用，如果更新不输入
            tempwm.UpdateCollider();
        }
        if (GUI.Button(new Rect(10, 90, 150, 30), "clearweapon"))
        {
            tempwm.DownWeapon();
        }
    }


    void InitDataBase()
    {
        weaponDB = new DataBase();
    }
    void InitWeaponFactory()
    {
        weaponFactory = new WeaponFactory(weaponDB);
    }


}

