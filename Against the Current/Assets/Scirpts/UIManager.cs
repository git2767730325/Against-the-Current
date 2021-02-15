using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public float timeScale = 1f;
    public GameObject menu;
    public Button closeGame;
    public Button menuCloseBtn;
    public PlayerHandle ph;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public GameObject bag;
    public GameObject miniMap;
    public GameObject gunTarget;
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
    [Header("=========    药水使用交互   ========== ")]
    public BagPanel bp;
    private float durationTime=45f;//药水持续时间
    [Header("=========   临时装备属性   ======== ")]
    public Grid tempGrid;
    [Header("========  技能UI， 复活UI  ===========")]
    public TimeComeBack atcScript;
    public AgainstTheCurrent atcAndTcbScript;
    public GameObject atc;
    public Image atcCD;
    public GameObject timeBack;
    public Image timeBackCD;


    public GameObject resurrection;
    public Image timeleft;
    //public float durable;
    private void Awake()
    {
        //btn1.onClick.AddListener(ChangeWeaponA);
        //btn2.onClick.AddListener(ChangeWeaponB);
        //btn3.onClick.AddListener(ChangeWeaponC);
        menuCloseBtn.onClick.AddListener(CloseMenu);
        closeGame.onClick.AddListener(CloseGame);
    }

    private void CloseGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    void Start()
    {
        GameManager.GM.BindBag(bag);
        panel1.fillOrigin = 2;
        panel2.fillOrigin = 2;
        //体力应该是可序列化的,暂时用float测试
    }
    // Start is called before the first frame update
    private void Update()
    {
        //检查是否需要锁定鼠标？
        if((bag.gameObject.activeSelf||menu.activeSelf))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //逆流+时光倒流UI

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
                i=i+3;
            interImg.color = new Color(1, 1, 1,i/255.0f);
            if (!interFace.activeSelf)
            {
                interFace.SetActive(true);
                if (am.im.ecmList[0].eventName == "pickup")
                    interfaceText.text = "拾取";
                else if(am.im.ecmList[0].eventName == "talk")
                {
                    interfaceText.text = "对话";
                }
                else if (am.im.ecmList[0].eventName == "seal")
                {
                    interfaceText.text = "封印";
                }
                else
                    interfaceText.text = "和他讲武德(需蹲下)";
            }
            if(am.im.ecmList[0].eventName=="pickup"&&ph.Btn1.OnPressed)
            {
                //判断是否添加物品成功，决定显示的信息
                bool f=false;
                try
                {
                    f=bp.AddItem(pickItem.itemID);
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
            else if(am.im.ecmList[0].eventName == "talk" && ph.Btn1.OnPressed)
            {
                dialogue.Dialog();
                if (dialogue.dialogText.text == "完成任务！")
                {
                    RewardItem thisReward=dialogue.GetComponent<RewardItem>();
                    if (thisReward.canReward)
                    {
                        Debug.Log("ww");
                        thisReward.itemNum -= 1;
                        thisReward.canReward = false;
                        bp.AddItem(thisReward.itemID);
                    }
                    //如果添加成功，数目减一,更换文本
                    //dialogue.InitTextList
                }
            }
        }
        else if(interFace.activeSelf)
        {
            interFace.SetActive(false);
        }
        if (Time.timeScale == 0.2f|| Time.timeScale == 0.21f)//通过时间流动判断交互
        {
            interFace.SetActive(true);
            interfaceText.text = "疾风突袭";
        }
        ////耐久
        if (am.sm.durable<=2f&&tempGrid!=null&&am.sm.durable!=1)
        {
            am.sm.durable = 0f;
            tempGrid.SetDurable(1);
            ItemPanel.itemP.grid = tempGrid;
            bp.DelItem(tempGrid);
        }
        if(tempGrid!=null)
        {
            tempGrid.SetDurable((int)am.sm.durable);
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
        if (am.ac.CheckAnimatorState("die")&&!resurrection.activeSelf)
        {
            Invoke("Resume",3f);
        }
        else if(resurrection.activeSelf&& am.ac.CheckAnimatorState("die"))
        {
            if(timeleft.fillAmount>0)
            timeleft.fillAmount -=1/600f;
            else
            {
            //游戏结束
            }    
        }
        else
        {
            resurrection.SetActive(false);
        }
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

    public void Resume()
    {
        resurrection.SetActive(true);
        timeleft.fillAmount = 1;
    }

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
