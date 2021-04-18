using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public bool isTheLastLevel = false;
    public float timeScale = 1f;
    public PlayerHandle ph;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public GameObject bag;
    public GameObject miniMap;
    public GameObject gunTarget;
    //破盾特效
    public GameObject breakShieldVFX;
    [Header("======  主菜单 =======")]
    //游戏菜单
    public GameObject menu;
    public GameObject menuSetting;
    public Button closeGame;
    public Button menuCloseBtn;
    public Button menuSaveBtn;
    public Button menuSetBtn;
    public Slider sensitivity;
    public Slider voiceValue;
    [Header("===== 体力条 ====")]
    public ActorManager am;
    public Image panel0;
    public Image panel1;
    public Image panel2;
    public Image panel3;
    [Header("=====     人物面板、血量等    =====")]
    public GameObject characterUI;
    public GameObject allhpwpbp;
    public Image hpPanel;
    public Image wpPanel;
    public Image bpPanel;
    public Text attackNum;
    public Text defenceNum;
    public Text currentDurable;

    [Header("====      背刺，开箱，拾取物品,对话疾风突袭 显示可用      ===")]
    public GameObject itemPickTip;
    public Text pickTip;
    private MyTimer durTimer=new MyTimer();
    public PickItem pickItem;
    public GameObject interFace;
    public Image interImg;
    public Text interfaceText;
    public Dialogue dialogue;
    private float i = 120;
    [Header("===      任务，背包     交易UI        ===")]
    public GameObject rewardTip;
    public GameObject business;
    [Header("=========    药水使用交互   ========== ")]
    public BagPanel bp;
    //private float durationTime=45f;//药水持续时间
    [Header("==============回城UI  (战斗场景拖入) ==================")]
    public GameObject goHome;
    public Button goHomeYes;
    public Button goHomeNo;
    public int sceneNum = 1;
    [Header("=========   临时装备属性   ======== ")]
    public Grid tempGrid;
    [Header("========  技能UI， 复活UI  ===========")]
    public TimeComeBack atcScript;
    public AgainstTheCurrent atcAndTcbScript;
    public GameObject atc;
    public Image atcCD;
    public GameObject timeBack;
    public Image timeBackCD;


    public Image timeleft;
    public GameObject resurrection;
    public bool canResume = true;
    public bool isResumeing = false;
    //public float durable;
    private void Awake()
    {
        menuCloseBtn.onClick.AddListener(CloseMenu);
        closeGame.onClick.AddListener(CloseGame);
        menuSaveBtn.onClick.AddListener(SaveGame);
        if(menuSetting!=null)
        menuSetBtn.onClick.AddListener(SettingGame);
        if(goHome!=null)
        {
            goHomeYes.onClick.AddListener(GoHomeYes);
            goHomeNo.onClick.AddListener(GoHomeNo);
        }
    }

    void Start()
    {
        if(ph==null)
            ph = GameObject.FindWithTag("Player").GetComponent<PlayerHandle>();
        am = ph.transform.GetComponent<ActorManager>();
        atcScript = am.transform.GetComponent<TimeComeBack>();
        atcAndTcbScript = am.transform.GetComponent<AgainstTheCurrent>();
        //GM绑定
        GameManager.GM.BindBag(bag);
        panel1.fillOrigin = 2;
        panel2.fillOrigin = 2;
        //体力应该是可序列化的,暂时用float测试

    }
    private void CloseGame()
    {
        Debug.Log("暂时取消关闭游戏内功能");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    private void SettingGame()
    {
        menuSetting.SetActive(true);
    }
    private void SaveGame()
    {
        GameManager.GM.SaveByJson();
    }

    //外部调用
    public void SetVoiceValue()
    {
        Debug.Log(voiceValue.value);
        AudioManager.SetVoiceValue(voiceValue.value);
    }
    public void UpdateVoiceValue()
    {
        voiceValue.value = GameManager.GM.voiceValue;
    }

    // Start is called before the first frame update
    private void Update()
    {
        //音量调节
        //检查是否需要锁定鼠标？
        if (bp.gameObject.activeSelf|| menu.activeSelf)//business.gameObject.activeSelf||menu.activeSelf||goHome.activeSelf)
        {
            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
        else
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        //修改鼠标灵敏度

        //if((bag.gameObject.activeSelf||menu.activeSelf))
        //{
        //    Cursor.lockState = CursorLockMode.Confined;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        //逆流+时光倒流UI
        timeBackCD.fillAmount=1-atcAndTcbScript.timeIndex /(float)atcAndTcbScript.maxIndex;
        //判断逆流是否显示
        if(atcAndTcbScript!=null)
        {
            if (atcAndTcbScript.enabled&&atcAndTcbScript.separationQue.Count < 5)
            {
                if(atc.activeSelf)
                {
                    atc.SetActive(false);
                    if(atcScript.enabled)
                    atcScript.enabled = false;
                }
            }
            else
            {
                if (!atc.activeSelf)
                {
                    atc.SetActive(true);
                    if (!atcScript.enabled)
                        atcScript.enabled =true;
                }
            }
        }
        //逆流UI
        if(atcScript.cdTimer.state!=MyTimer.State.RUN)
        {
            atcCD.fillAmount = 0;
        }
        else
        {
            atcCD.fillAmount = 1;
        }

        if (ph.BtnUI4.OnPressed&&!menu.activeSelf)//后期菜单物品都要时间暂停
        {
            menu.SetActive(true);
            timeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        durTimer.Tick();//计时器,药水应该放到fixupdate里
        //HP,WP,BP显示
        hpPanel.fillAmount = am.sm.Hp / am.sm.HpMax;
        bpPanel.fillAmount = (float)am.sm.ammo / (float)am.sm.ammoMax;
        wpPanel.fillAmount = am.sm.weaponPower / am.sm.weaponMaxPower;
        //攻击力，防御力,耐久面板,耐久
        if (am.wm.wcR.wd != null)
            am.sm.ATK = am.wm.wcR.GetATK();
        attackNum.text = am.sm.ATK.ToString();
        defenceNum.text = am.sm.DEF.ToString();
        currentDurable.text = am.sm.durable.ToString();

        //按键开启背包，人物信息，小地图
        if (ph.BtnUI1.OnPressed)
        {
            bag.SetActive(!bag.activeSelf);
        }
        else if(ph.BtnUI2.OnPressed)
        {
            characterUI.SetActive(!characterUI.activeSelf);
        }
        else if(ph.BtnUI3.OnPressed)
        {
            miniMap.SetActive(!miniMap.activeSelf);
        }
        //体力相关UI
        if(am.sm.canUseStamina&&am.sm.stamina>=200)
        {
            panel3.gameObject.SetActive(false);
            panel0.gameObject.SetActive(false);
            panel1.gameObject.SetActive(false);
            panel2.gameObject.SetActive(false);
        }
        else if(!panel1.gameObject.activeSelf)
        {
            panel3.gameObject.SetActive(true);
            panel0.gameObject.SetActive(true);
            panel2.gameObject.SetActive(true);
            if(am.sm.canUseStamina)
            panel1.gameObject.SetActive(true);
        }
        //判断红条
        panel1.fillAmount = Mathf.Lerp(panel1.fillAmount,panel2.gameObject.activeSelf?panel2.fillAmount:am.sm.stamina/150, 0.02f);
        panel2.fillAmount = am.sm.stamina / 200;
        if (am.sm.stamina < 0)
        {
            panel2.gameObject.SetActive(false);
        }
        ///判断何时交互
        if (am.im.ecmList.Count > 0 &&Time.timeScale==1.0f)
        {
                if (i > 240)
                {
                    i = 120;
                }
                else
                    i = i + 3;
                interImg.color = new Color(1, 1, 1, i / 255.0f);
            //更新UI文字和显示，容易BUG
            if (!interFace.activeSelf)
            {
                interFace.SetActive(true);
            }
            if (am.im.ecmList[0].eventName == "pickup")
                interfaceText.text = "拾取";
            else if (am.im.ecmList[0].eventName == "talk")
            {
                interfaceText.text = "对话";
            }
            else if (am.im.ecmList[0].eventName == "seal")
            {
                interfaceText.text = "封印";
            }
            else if (am.im.ecmList[0].eventName == "gohome")
            {
                interfaceText.text = "回城";
            }
            else
            {
                interfaceText.text = "和他讲武德(需蹲下)";
            }
                //
            if (am.im.ecmList[0].eventName == "pickup" && ph.Btn1.OnPressed)
                {
                    //判断是否添加物品成功，决定显示的信息
                    bool f = false;
                    try
                    {
                        f = bp.AddItem(pickItem.itemID);
                        AddItemTip(f);
                        pickItem.SetFalse();
                        am.im.ListUpdate();//更新列表，防止卡了一直有事件
                    }
                    catch
                    {
                        Debug.Log("fankaBUG");
                        AddItemTip(f);
                        pickItem.SetFalse();
                        am.im.ListUpdate();//更新列表，防止卡了一直有事件
                    }
                }
            //如果对话，有奖励物品的话
            else if (am.im.ecmList[0].eventName == "talk" && ph.Btn1.OnPressed)
            {
                dialogue.Dialog();
                RewardItem thisReward = dialogue.GetComponent<RewardItem>();
                if (thisReward != null)
                {
                    if (thisReward.canReward && thisReward.itemNum >=1)
                    {
                        if (thisReward.itemID >= 0)
                        {
                            if (bp.AddItem(thisReward.itemID))
                            {
                                if(thisReward.coinNum>0)
                                bp.AddCoin(thisReward.coinNum);
                                Debug.Log("恭喜");
                                thisReward.GetReward();
                                AddItemTip(true);
                                //pickItem.SetFalse();
                            }
                            else
                            {
                                AddItemTip(false);
                                pickItem.SetFalse();
                                Debug.Log("背包满了");
                            }
                        }
                        else
                        {
                            bp.AddCoin(thisReward.coinNum);
                            Debug.Log("恭喜");
                            thisReward.GetReward();
                            AddItemTip(true);
                        }
                    }
                    else if (thisReward.canReward)
                    { 
                        //更新已完成任务状况，设置奖励情况
                        GameManager.GM.UpdateTaskState(dialogue.dialogueAsset_1.name);
                        //thisReward.GetReward();
                        //检查是否有其他任务完成，更换对话文本以及奖励情况
                        dialogue.ChangeAsset(GameManager.GM.CheckTask());
                        Debug.Log("更换文本");
                            //更换奖励条件
                        }
                    }
                }
            else if(am.im.ecmList[0].eventName == "gohome" && ph.Btn1.OnPressed)
            {
            GameManager.GM.ChangeScene(1);
            }
        }
        else if(interFace.activeSelf)
        {
            interFace.SetActive(false);
        }
        else if(am.im.ecmList.Count<=0)
        {
            CloseBusinessPanel();
        }
        if (Time.timeScale == 0.2f|| Time.timeScale == 0.1f)//通过时间流动判断交互
        {
            if (!interFace.activeSelf)
            {
                interFace.SetActive(true);
                interfaceText.text = "疾风突袭";
            }
            if(ph.KeyC.OnPressed)
            {
                am.ac.SetTrigger("windy");
            }
        }
        ////耐久
        ///
        Debug.LogError("记得拖上破盾特效");
        //碎盾特效
        if(breakShieldVFX)
        {
            if (!am.sm.isBreakShield)
                breakShieldVFX.SetActive(false);
        }
        else
        {
            if (am.sm.isBreakShield)
                breakShieldVFX.SetActive(true);
        }
        if (am.sm.durable<=2f&&tempGrid!=null&&am.sm.durable!=1)
        {
            am.sm.durable = 0f;
            tempGrid.SetDurable(1);
            ItemPanel.itemP.grid = tempGrid;
            bp.DelItem(tempGrid);
        }
        if(tempGrid!=null)
        {
            //整理后会tempGrid出错》？
            if (am.sm.durable != tempGrid.GetCount())
            {
                tempGrid.SetDurable((int)am.sm.durable);
                //次数多余了,导致重复登陆??，使用bool判断受击，以便于控制sendmessage
                bp.UpdateWeaponDurCount(tempGrid);
                am.sm.durable = tempGrid.GetCount();
            }
 
        }
        if (am.ac.CheckAnimatorTag("gun", "Gun"))
            gunTarget.SetActive(true);
        else
            gunTarget.SetActive(false);

        //物品增加提示:
        if(itemPickTip.activeSelf)
        {
            if(durTimer.state==MyTimer.State.RUN)
            {
                durTimer.Tick();
            }
            else
            {
                if(durTimer.state == MyTimer.State.FINISHED)
                    itemPickTip.SetActive(false);
                durTimer.StartTimer(4f);
            }
        }

        //复活界面提示
        if (canResume&&am.sm.isDie&&!resurrection.activeSelf&&!am.sm.isResume)
        {
            if (!isResumeing)
            {
                Invoke("Resume", 3f);
                isResumeing = true;
            }
        }
        else if(resurrection.activeSelf&& am.sm.isDie)
        {
            if (timeleft.fillAmount > 0)
            {
                timeleft.fillAmount -= 1 / 300f;
                am.sm.Hp += 0.5f;
            }
            else
            {
                resurrection.SetActive(false);
                //游戏结束
                canResume = false;
            }    
        }
        //
        else if(!canResume && am.sm.isDie)
        {
            Debug.Log("无法复活，游戏结束");
            //如果是最后一关，读取存档，黑幕返回
            if(isTheLastLevel)
            {
                return;
            }
            //提示

            //3s返回上一存档
            Invoke("ReturnTitle", 3f);
        }
        else
        {
            //状态恢复！
            if (resurrection.activeSelf)
            {
                resurrection.SetActive(false);
            }
        }
    }


    public void SaveSetting()
    {
        GameManager.GM.mouseSensitivity =sensitivity.value;
        GameManager.GM.voiceValue = voiceValue.value;
        Debug.Log(sensitivity.value);
        GameManager.GM.SaveSetting();
    }

    //回到标题画面
    public void ReturnTitle()
    {
        GameManager.GM.ChangeScene(7);
    }

    public void  AddItemTip(bool value)
    {
        if (value)//有物品添加
        {
            pickTip.text = "获得物品了！";
        }
        else
        {
            pickTip.text = "请检查是否背包满了？";
        }
        itemPickTip.SetActive(true);
    }


    //弹出回城对话框，游戏时间变0
    public void TryGoHome()
    {
        if (am.sm.Hp > 0)
        {
            am.sm.Hp =am.sm.HpMax;
            //回城UI
            Time.timeScale = 0;
            //是按钮的是和否
            goHome.SetActive(true);
        }
    }
    //是按钮的是和否
    public void GoHomeYes()
    {
        goHome.SetActive(false);
        Time.timeScale = 1;
        GameManager.GM.ChangeScene(sceneNum);
        //呼叫GM，保存数据，检查，更换 场景

    }
    //是按钮的是和否
    public void GoHomeNo()
    {
        goHome.SetActive(false);
        Time.timeScale = 1;
        timeScale = 1;
    }


    //打开商店
    public void OpenBusinessPanel(bool value)
    {
        if (!business.activeSelf)
        {
            Debug.Log("sdsd");
            business.SetActive(true);
        }
        if (value)
        {
            business.SendMessage("SellEquipment");
        }
        else
        {
            business.SendMessage("SellConsum");
        }
        bag.SetActive(true);
    }
    //关闭商店
    public void CloseBusinessPanel()
    {
        business.SetActive(false);
    }

    //复活
    public void Resume()
    {
        //resurrectionVFX.SetActive(true);
        resurrection.SetActive(true);
        timeleft.fillAmount = 1;
        isResumeing = false;
        am.sm.Hp += 0.1f;
        //应该有碰撞体或者锁定重新激活
    }

    //持有物品
    public void HaveItem()
    {
        am.sm.DEF += 5;
    }
    public void UseItem(Grid _grid)//让BP调用，调节人物状态
    {
        int id = _grid.GetItemID();
        if(id==0)
        {
            tempGrid = _grid;
            GameManager.GM.AddWeapon(1);
            am.sm.durable = _grid.GetCount();
        }
        else if(id==1)
        {
            tempGrid = _grid;
            GameManager.GM.AddWeapon(2);
            am.sm.durable = _grid.GetCount();
        }
        else if (id == 4)
        {
            am.sm.DEF += 1;
            Debug.Log("useitem4");
        }
        else if (id == 5)
        {
            am.sm.DEF += 3;
        }
        else if (id==6)
        {
            am.sm.Hp =am.sm.HpMax;
            am.sm.weaponPower = am.sm.weaponMaxPower;
        }
        else if (id==7)
        {
            am.sm.Hp += 100;
        }
        else if (id==8)
        { 
            am.sm.weaponPower +=40;
        }
        else if (id==9)
        {
            am.sm.stamina = 200f;
        }
    }
    public void ChangeUseItemPos(Grid _grid)
    {
        //tempGrid = null;
        int itemId = 2;
        if (_grid != null)
        {
            tempGrid = _grid;
            itemId = tempGrid.GetItemID();
        }
        //tempGrid.GetGridID();
        if (itemId <= 1)
        {
            GameManager.GM.AddWeapon(itemId + 1);
            am.sm.durable = _grid.GetCount();
        }
        else
        {
            am.sm.durable = 0;
            GameManager.GM.AddWeapon(3);
        }
        
    }
    public void CloseMenu()
    {
        Time.timeScale =timeScale;
        menu.SetActive(false);
    }
    /*public void ChangeWeaponA()
    {
        int a = 1;
        GameManager.GM.AddWeapon(a);
    }   
    public void ChangeWeaponB()
    {
        int b = 2;
        GameManager.GM.AddWeapon(b);
    }    */
    public void ChangeWeaponC(Grid _gird)//删除物品装备
    {
        if (tempGrid == _gird)
        {
            am.sm.durable = 0;
            _gird.SetDurable(1);
            tempGrid = null;
            int c = 3;
            GameManager.GM.AddWeapon(c);
        }
    }
}
