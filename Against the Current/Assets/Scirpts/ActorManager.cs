using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public MyTimer scaleTimer=new MyTimer();
    public ActorController ac;
    public WeaponManager wm;
    public BattleManager bm;
    public StateManager sm;
    public DirectorManager dm;
    public InterActorManager im;

    public RuntimeAnimatorController a1;
    public AnimatorOverrideController a2;

    [Header("temp use")]
    public GameObject vfx;

    private void Awake()
    {
        ac = GetComponent<ActorController>();
        //使用泛型,如果没有脚本自动新建
        GameObject sensor = transform.Find("sensor").gameObject;
        bm=Bind<BattleManager>(sensor);
        GameObject model = ac.model.gameObject;
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InterActorManager>(sensor);
    }
    // Start is called before the first frame update
    void Start()
    {
        ac.OnAction += DoAction;
    }

    // Update is called once per frame
    void Update()
    {
        if(scaleTimer.state==MyTimer.State.FINISHED)//还有其他判断条件
        {
            Time.timeScale = 1f;
            scaleTimer.state = MyTimer.State.IDLE;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            ac.anim.SetBool("timeend",true);//暂时测试用；
            ac.camcontrol.CancelLock();
        }
        if(sm.isWindy&&Time.timeScale==0.2f)
        {
            if (this.name == "Player")
            {
                //ac.anim.speed = 3f;
                Time.timeScale = 0.21f;
            }
        }
    }

    private void FixedUpdate()
    {
        scaleTimer.Tick();
    }
    public void DoAction()
    {
        if (im.ecmList.Count > 0)
        {
            if (im.ecmList[0].eventName == "stab"&&ac.CheckAnimatorTag("crouch"))
            {
                ac.OnEnterBattle();
                dm.PlayTimeline("stab", this, im.ecmList[0].am);
            }
            else if(im.ecmList[0].eventName=="seal"&&im.ecmList[0].active==true)
            {
                if (wm.wcR.wd.gameObject.activeSelf)
                {
                    wm.wcR.wd.gameObject.SetActive(true);
                }
                dm.PlayTimeline("seal", this, im.ecmList[0].am);
                im.ecmList[0].active = false;
            }
            else if(im.ecmList[0].eventName == "openbox")
            {
                dm.PlayTimeline("openbox", this, im.ecmList[0].am);
            }
        }
    }//委托

    public void SetOrCollectWeapon(bool value)
    {
        if(wm.wcR.wd.gameObject.activeSelf != value)
            wm.wcR.wd.gameObject.SetActive(value);
        //如果盾牌不存在
        if(wm.wcL.wd==null)
        {
            return;
        }
        if (wm.wcL.wd.gameObject.activeSelf!=value)
            wm.wcL.wd.gameObject.SetActive(value);
    }
    public void Die()
    {
        ac.SetTrigger("die");
        CapsuleCollider cc = bm.transform.parent.GetComponent<CapsuleCollider>();
        if(cc!=null&&cc.gameObject.name!="Player")//别人不能锁定
        cc.enabled = false;
        if(gameObject.name=="Player")
        ac.camcontrol.CancelLock();
    }
    public void Hit()
    {
        ac.SetTrigger("hit");
    }

    public void HitOrDie(WeaponControl _wc)
    {
        Vector3 backForce = -Vector3.Normalize(_wc.wm.am.transform.position-transform.position);
        Rigidbody rig =transform.GetComponent<Rigidbody>();
        rig.AddForce(backForce*10f,ForceMode.Impulse);
        Debug.Log("jitui");
        if (sm.Hp > 0)
        {   //敌人攻击力，*此物体减伤,玩家的武器能量条
            float percent = _wc.wm.am.sm.weaponPower / _wc.wm.am.sm.weaponMaxPower;
            if (percent >= 0.2f)
                sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * (sm.isInGun ? 0.1f : 1f));
            else
                sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * 0.1f);
            if (sm.Hp > 0)
                Hit();
            else
            {
                Die();
                _wc.wm.am.ac.camcontrol.CancelLock();//攻击者取消锁定
            }
        }
        else
        {
            //already dead;
        }
    }


    public void HitCloseWeapon()
    {
        wm.WeaponDisable();
    }

    public void BreakStun()
    {
        ac.SetTrigger("breakstunned");
    }

    public void Blocked()
    {
        ac.SetTrigger("blocked");
    }
    public void BreakBlocked()
    {
        ac.SetTrigger("breakstunned");
    }
    public float GetAttackerATK(WeaponControl _wc,StateManager _sm)
    {
        return (_wc.wd.ATK + _sm.ATK);
    }
    public void TryDoDamage(WeaponControl _wc)
    {
        if (sm.isFlip)
        {
            //如果可以触发疾风突袭
            if (sm.canWindy)
            {
                Time.timeScale = 0.2f;
                ac.anim.SetTrigger("windy");
                return;
            }
            else
                sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm)*(sm.isInGun?0.1f:1f));//敌人攻击力，*此物体减伤
        }
        //高耦合，低内聚的做法
        //闪避等无敌状态不处理
        if (sm.isImmortal||sm.isWindy)
            return;
        if (Vector3.Angle(_wc.wm.am.ac.model.transform.forward, ac.model.transform.forward) > 150)//盾反角度条件
        {
            if (sm.isCounterBack)
            {
                _wc.wm.am.BreakStun();//攻击者被盾反
                //特效vfx,放在哪里比较好？
                //vfx.SetActive(true);
                if (MyTimer.State.IDLE == scaleTimer.state)//盾反时间
                {
                    Time.timeScale = 0.01f;
                    scaleTimer.StartTimer(0.02f);
                    Debug.Log("startscale");
                }
                return;
            }
        }
        if (sm.isDefence)
        {
            //如果背对敌人.....
            if (Vector3.Angle(_wc.wm.am.ac.model.transform.forward, ac.model.transform.forward) < 120)
            {
                HitOrDie(_wc);
            }
            else
            {
                ac.SetLayerWeight("denfense", 0);
                //盾牌的耐久值减少,减少值是敌人的攻击力的30%
                sm.durable -= GetAttackerATK(_wc, _wc.wm.am.sm) * 0.5f;
                if (sm.durable > 2f)
                {
                    Blocked();//绝对防御
                }
                else 
                {
                    BreakBlocked();
                }
            }
        }
        else
        {
            HitOrDie(_wc);
            if (_wc.wm.gameObject.tag == "Player")
            {
                _wc.wm.am.sm.weaponPower -= 7;
                Mathf.Clamp(_wc.wm.am.sm.weaponPower, 0, _wc.wm.am.sm.weaponMaxPower);
            }
        }
    }

    //被枪射击
    public void TryDoDamage()
    {
        if(sm.isDefence)
        {
            Blocked();
            return;
        }
        else if(sm.isBlocked)
        {
            BreakBlocked();
        }
        if (sm.Hp > 0)
        {
            sm.AddHP(-2f);
            if (sm.Hp > 0)
                Hit();
            else
            {
                Die();
            }
        }
    }

    public void LockAC()
    {
        if (gameObject.name == "Player")
            ac.ChangeBool("lock", true);
        else
        {
            if(!ac.CheckAnimatorState("die"))
            ac.SetTrigger("die");
        }

    }
    public void UnLockAC()
    {
        if (gameObject.name == "Player")
        {
            ac.ChangeBool("lock", false);
        }
        else
            gameObject.SetActive(false);
        if(im.ecmList.Count>0)
        im.ecmList.Remove(im.ecmList[0]);

    }

    //泛型方法
    public T Bind<T> (GameObject sensor) where T:IActorManagerInterface
    {
        //sensor只是代
        T t = sensor.GetComponent<T>();
        if(t==null)
        {
            t = sensor.AddComponent<T>();
        }
        t.am = this;
        return t;
    }
}
