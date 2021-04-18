using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static JsonData apJD = new JsonData();//账号密码
    public static GameManager GM;
    private static Queue<JsonData> queue=new Queue<JsonData>();
    private DataBase weaponDB;
    private WeaponFactory weaponFactory;
    [Header("新场景需要重新绑定的脚本和物体")]
    public LoginManager lm;
    public BagPanel bp;
    public WeaponManager tempwm;
    public GameObject bag;//就是banpanel脚本的物3体
    //[Header("场景加载条")]
    public Slider loadSlider;
    public TaskManager tm;
    public ChatManager cm;
    [Header("设置相关")]
    public float mouseSensitivity=20f;
    public float voiceValue = 0.3f;
    [Header("===== 任务相关（需要保存） =====")]
    public int sealEnemyNum=0;
    public int dunEnemyNum=0;
    public int backStabEnemyNum=0;
    public bool passLevel1=false;
    public bool passLevel2 = false;
    [Header("====  存档相关（新场景重新获取） =====")]
    public GameObject player;
    public string saveName;
    public int sceneNum = 0;

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
            Debug.Log("删除了多余的GM");
            Destroy(this.gameObject);
            return;
        }
    }
    private void Start()
    {
        InitDataBase();
        InitWeaponFactory();
        //恢复系统设置，灵敏度，声音大小
        LoadSetting();
        Dictionary<string, Task> ss = new Dictionary<string,Task>();
        Task a = new Task();






        a.taskID = 2;
        a.taskName = "sadasd";
        ss.Add("sb",a);
        //LoadByJson();
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
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
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
            apJD["account"]= _jd["account"];
            apJD["passwords"] = _jd["passwords"];
        }
        else if((int)_jd["function"] == 8)
        {
            //重登陆
            jd["account"] = apJD["account"];
            jd["passwords"] = apJD["passwords"];
        }
        else if ((int)_jd["function"] == 6)
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
        //Debug.Log(_jd.ToJson());
        int re = -1;
        if ((int)_jd["function"] == 1)
        {
            re=LoginOrRegister(_jd);
            lm.LinkState(re);
        }
        else if((int)_jd["function"] == 8)
        {
             Debug.Log("功能8");
            if (bp != null)//确定到了装备场景
            {
                //Debug.Log(_jd.ToJson());
                bp.GridItemUpdate(_jd);//发送更新消息
            }
        }
        else if((int)_jd["function"] == 7)
        {
            bp.coin = (int)_jd["coin"];
            bp.coinCount.text = bp.coin.ToString();
        }
        else if ((int)_jd["function"] == 4)
        {
            Debug.Log("接收聊天消息");
            cm.ReciveChat(_jd);
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
            Debug.Log("无");
            return -1;
    }

    //音效大小，鼠标灵敏度
    public void SaveSetting()
    {
        GameSave gs = new GameSave();
        gs.mouseSensitivity = GM.mouseSensitivity.ToString("f2");
        gs.voiceValue = GM.voiceValue.ToString("f2");
        string s = JsonMapper.ToJson(gs);
        using (StreamWriter sw = new StreamWriter(@"E:\gameSetting.txt"))
        {
            sw.Write(s);
            Debug.Log("SaveSetting");
        }
    }
    public void LoadSetting()
    {
        GameSave gs = null;
        if(File.Exists(@"E:\gameSetting.txt"))
        {
            using (StreamReader sr = new StreamReader(@"E:\gameSetting.txt"))
            {
                string s = sr.ReadToEnd();
                gs = JsonMapper.ToObject<GameSave>(s);
                GM.mouseSensitivity = float.Parse(gs.mouseSensitivity);
                GM.voiceValue = float.Parse(gs.voiceValue);
                AudioManager.SetVoiceValue(GM.voiceValue);
                Debug.Log(GM.mouseSensitivity);
            }
        }
    }


    public  void SaveByJson()
    {
        GameSave gs =new GameSave();
        gs.playerPositionX = player.transform.position.x.ToString("f2");
        gs.playerPositionY = player.transform.position.y.ToString("f2");
        gs.playerPositionZ = player.transform.position.z.ToString("f2");
        gs.playerRotionX = player.transform.eulerAngles.x.ToString("f2");
        gs.playerRotionY = player.transform.eulerAngles.y.ToString("f2");
        gs.playerRotionZ = player.transform.eulerAngles.z.ToString("f2");
        gs.sealEnemyNum =GM.sealEnemyNum;
        gs.dunEnemyNum = GM.dunEnemyNum;
        gs.backStabEnemyNum = GM.backStabEnemyNum;
        gs.passLevel1 = GM.passLevel1;
        gs.passLevel2 = GM.passLevel2;
        gs.sceneNum = SceneManager.GetActiveScene().buildIndex;
        //人物属性
        StateManager _sm = player.GetComponent<StateManager>();
        gs.hp = _sm.Hp.ToString();
        gs.wp = _sm.weaponPower.ToString();
        gs.ammo = _sm.ammo;
        //任务
        if (tm != null)
        {
            //gs.taskList = new List<Task>(tm.taskList);
            gs.taskList = new List<Task>();
            // gs.taskList.Add();
            Debug.Log("任务不对的做法？");

            foreach (var task in tm.taskList)
            {
                Task _task = task.CloneTask();
                gs.taskList.Add(_task);
            }
        }

        //存档类转化为json字符串
        string jsonSave = JsonMapper.ToJson(gs);
        using(StreamWriter sw = new StreamWriter(@"E:\"+saveName+".txt")) 
        {
            sw.Write(jsonSave);
        }
        Debug.Log(jsonSave);
    }
    public void LoadByJson(int function)
    {
        GameSave gs = null;
        if(File.Exists(@"E:\" + saveName + ".txt"))
        {
            using (StreamReader sr = new StreamReader(@"E:\" + saveName + ".txt"))
            {
                string saveJson = sr.ReadToEnd();
                gs = JsonMapper.ToObject<GameSave>(saveJson);
                //先加载场景
                if (function == 0)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Debug.Log(SceneManager.GetActiveScene().buildIndex.ToString());
                    GameManager.GM.sceneNum = gs.sceneNum;
                    ChangeScene(gs.sceneNum);
                }
                Debug.Log(SceneManager.GetActiveScene().buildIndex.ToString());
                //开启协程或别的，延时启动
                StartCoroutine("DelayLoadInfo",gs);

                //人物属性
                //SceneManager.LoadScene(gs.sceneNum);
                //任务
                if (tm != null)
                    tm.taskList = new List<Task>(gs.taskList);
            }
        }
        //如果没有存档
        else
        {
            using (StreamWriter sr = new StreamWriter(@"E:\" + saveName + ".txt"))
            {
                GameSave _gs = new GameSave();
                string s = JsonMapper.ToJson(_gs);
                sr.Write(s);
            }
        }
    }
    //延时更换场景
    IEnumerator DelayLoadInfo(GameSave gs)
    {
        while (GameManager.GM.sceneNum != SceneManager.GetActiveScene().buildIndex)
        {
            yield return new WaitForSeconds(0.3f);
        }
        if (gs.playerPositionX != null)
        {
            player.transform.position = new Vector3(float.Parse(gs.playerPositionX),
        float.Parse(gs.playerPositionY), float.Parse(gs.playerPositionZ));
            player.transform.rotation = Quaternion.Euler(new Vector3(float.Parse(gs.playerRotionX),
                float.Parse(gs.playerRotionY), float.Parse(gs.playerRotionZ)));
            GM.sealEnemyNum = gs.sealEnemyNum;
            GM.dunEnemyNum = gs.dunEnemyNum;
            GM.backStabEnemyNum = gs.backStabEnemyNum;
            GM.passLevel1 = gs.passLevel1;
            GM.passLevel2 = gs.passLevel2;
            StateManager _sm = player.GetComponent<StateManager>();
            _sm.Hp = float.Parse(gs.hp);
            _sm.weaponPower = float.Parse(gs.wp);
            _sm.ammo= gs.ammo;
            Debug.Log("存档读取");
        }
    }

    IEnumerator DelayAutoSaveInfo(bool value=true)
    {
        if (value)
        {
            while (GameManager.GM.sceneNum != SceneManager.GetActiveScene().buildIndex)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
        //回到主场自动保存,其他关卡也保存
        if(GM.sceneNum==1)
            GM.SaveByJson();
        else if(GM.sceneNum != 0&& GM.sceneNum !=7)
        {
            GM.SaveByJson();
        }
    }
        //更新已完成任务状况，更改奖励
        public void UpdateTaskState(string s)
    {
        if (tm != null)
            tm.UpdateTaskState(s);
    }

    //检测已完成任务，并且更换对话脚本
    public string CheckTask()
    {
        string a = tm.CheckTask();
        if(a!=null)
        return a;
        return null;
    }


    //更换场景
    public void ChangeScene(int a)
    {
        GM.sceneNum = a;
        AsyncOperation operation = SceneManager.LoadSceneAsync(a);
        operation.allowSceneActivation = true;
        StartCoroutine(DelayAutoSaveInfo());
    }
    //进度条更换场景
    public void ChangeSceneLoad(int a)
    {
        GM.sceneNum = a;
        StartCoroutine("LoadScene",a);
        StartCoroutine(DelayAutoSaveInfo());
    }

     IEnumerator LoadScene(int a)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(a);
        operation.allowSceneActivation = false;//是否自动跳转，默认是自动的 
        //如果没加载完
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                loadSlider.value += 0.05f;
                if (loadSlider.value <= 0.99)
                {
                    //
                }
                else
                {
                    operation.allowSceneActivation = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                loadSlider.value = operation.progress;
            }
            yield return null;
        }
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

