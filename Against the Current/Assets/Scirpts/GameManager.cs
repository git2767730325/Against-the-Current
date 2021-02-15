using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static JsonData apJD = new JsonData();//账号密码
    public LoginManager lm;
    public BagPanel bp;
    public static GameManager GM;
    private static Queue<JsonData> queue=new Queue<JsonData>();
    public WeaponManager tempwm;
    private DataBase weaponDB;
    private WeaponFactory weaponFactory;
    public GameObject bag;
    //private GameObject prefab;

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
        //prefab = Resources.Load("sword") as GameObject;

        //print(weaponDB.weaponDataBase);
        //GameObject a=weaponFactory.CreateWeapon("sword",transform.position,Quaternion.identity);
       // GameObject aa = weaponFactory.CreateWeapon("sword",tempwm);
       //后面两个有用，但是不能在这里用
       // Collider col=weaponFactory.CreateWeapon("sword", tempwm);//col没用，如果更新不输入
       // tempwm.UpdateCollider();
    }
    // Update is called once per frame
    void Update()
    {
        if (queue.Count > 0)
            ReadMessage();
    }


    public  void BindBP(BagPanel bp)
    {
        GM.bp = bp;
    } 
    public  void BindBag(GameObject _bag)
    {
        GM.bag = _bag;
    }

    public void BindWM(WeaponManager wm)
    {
        GM.tempwm = wm;
    }



    //发送json消息到服务端
    public static void SendMessages(JsonData _jd)
    {
        JsonData jd = _jd;
        if((int)_jd["function"]==9)
        {
            //切换场景
            apJD = _jd;
        }
        else if((int)_jd["function"] == 8)
        {
            //重登陆
            jd["account"] = apJD["account"];
            jd["passwords"] = apJD["passwords"];
        }
        Client.Send(jd);
        Debug.Log(jd.ToJson());
    }
    public static void AddMessages(JsonData _jd)
    {
        queue.Enqueue(_jd);
    }
    public void ReadMessage()
    {

        JsonData jd = queue.Dequeue();
        DisposeFuntion(jd);
    }
    private void DisposeFuntion(JsonData _jd)
    {   //解析消息
        Debug.Log(_jd.ToJson());
        int re = -1;
        if ((int)_jd["function"] == 1)
        {
            re=LoginOrRegister(_jd);
            lm.LinkState(re);
        }
        else if(((int)_jd["function"] == 8))
        {
             Debug.Log("功能8");
            if (bp != null)//确定到了装备场景
            {
                Debug.Log(_jd.ToJson());
                bp.GridItemUpdate(_jd);//发送更新消息
            }
        }
    }
    private static int LoginOrRegister(JsonData _jd)
    {
        if ((int)_jd["mode"] == 3)
        {
            if ((int)_jd["result"] == 1)
                return 1;
            else if ((int)_jd["result"] == 0)
                return 0;
            else if ((int)_jd["result"] == 2)
                return 2;
            else if ((int)_jd["result"] == 3)
            {
                return 3;
            }
            else if ((int)_jd["result"] == 4)
                return 4;
            else if ((int)_jd["result"] == 5)
                return 5;
        }
        else
            ;
            return -1;
    }
    //更换场景
    public static void ChangeScene(int a)
    {
        SceneManager.LoadScene(a);
    }



    public void AddWeapon(int id)
    {
        switch (id)
        {
            case 1:
                tempwm.DownWeapon();
                Collider col = weaponFactory.CreateWeapon("shielda", tempwm);//col没用，如果更新不输入
                tempwm.UpdateCollider();
                break;
            case 2: 
                tempwm.DownWeapon();
                Collider col2 = weaponFactory.CreateWeapon("shieldb", tempwm);//col没用，如果更新不输入
                tempwm.UpdateCollider();
                break;
            case 3://卸下装备
                tempwm.DownWeapon();
                break;
    }
    }

    /*
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
    *///Ongui的调用武器
    //初始化
    void InitDataBase()
    {
        weaponDB = new DataBase();
    }
    void InitWeaponFactory()
    {
        weaponFactory = new WeaponFactory(weaponDB);
    }


}

