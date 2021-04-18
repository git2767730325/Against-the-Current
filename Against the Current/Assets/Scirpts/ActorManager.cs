using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public MyTimer scaleTimer=new MyTimer();
    public MyTimer scalewindyTimer=new MyTimer();
    public ActorController ac;
    public WeaponManager wm;
    public BattleManager bm;
    public StateManager sm;
    public DirectorManager dm;
    public InterActorManager im;

    public RuntimeAnimatorController a1;
    public AnimatorOverrideController a2;
    //debug
    private int debugTimelineI = 0;
    [Header("temp use dunfangvfx")]
    public GameObject vfx;
    [Header("=======  对象池生成掉落物品手动拖动player用的 =======")]
    public ItemPool itemPool;
    [Header("=======  受击特效，手动拖动enemy用的 =======")]
    public HitVFX hitVFX;

    private void Awake()
    {
        ac = GetComponent<ActorController>();
        //使用泛型,如果没有脚本自动新建
        GameObject sensor = transform.Find("sensor").gameObject;
        bm=Bind<BattleManager>(sensor);
        GameObject model = ac.model.gameObject;
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        if(gameObject.tag=="Player")
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

        else if ((scalewindyTimer.state == MyTimer.State.RUN&&sm.isGround)||scalewindyTimer.state == MyTimer.State.FINISHED)//还有其他判断条件
        {
            Time.timeScale = 1f;
            ac.ChangeBool("timeend",true);
            //耦合高
            if(gameObject.tag=="Player")
            if(ac.camcontrol.lockState==true)
            ac.camcontrol.CancelLock();
            scalewindyTimer.state = MyTimer.State.IDLE;
        }
        if(gameObject.tag!="Player"&& gameObject.tag !="Separation")
        {
            if(Time.timeScale==1f)
                ac.ChangeBool("timeend", true);
            else
            {
                ac.ChangeBool("timeend", false);
            }
        }
        else if(Time.timeScale==1)
        {
            if (vfx != null)
                if (vfx.activeSelf)
                    vfx.SetActive(false);
                else
                    Debug.Log("检查是否有特效");
        }
    }

    private void FixedUpdate()
    {
        scaleTimer.Tick();
        scalewindyTimer.Tick();
    }
    IEnumerator PlayTimeLineAgain(bool isSeal=false)
    {
        yield return new WaitForSeconds(0.2f);
        if(!isSeal)
        dm.PlayTimeline("stab", this, im.ecmList[0].am);
        else
        dm.PlayTimeline("seal", this, im.ecmList[0].am);
    }
    public void DoAction()
    {
        if (im.ecmList.Count > 0)
        {
            if (im.ecmList[0].eventName == "stab"&&ac.CheckAnimatorTag("crouch"))
            {
                ac.OnEnterBattle();
                if (debugTimelineI <= 0)
                {
                    debugTimelineI++;
                    dm.PlayTimeline("stab", this, im.ecmList[0].am);
                    StartCoroutine("PlayTimeLineAgain", false);
                }
                else
                {
                    dm.PlayTimeline("stab", this, im.ecmList[0].am);
                }
                    Invoke(("DropItem"), 5f);
                sm.weaponPower = sm.weaponMaxPower;
                ac.model.transform.forward=Vector3.Normalize(new Vector3((-transform.position+im.ecmList[0].transform.position).x,0,
                    (-transform.position + im.ecmList[0].transform.position).z));
                //对象次掉落物品
                    //任务敌人
                    GameManager.GM.backStabEnemyNum++;
                    GameManager.GM.sealEnemyNum++;

                //GameObject eitem=itemPool.DeQueue();
                //eitem.transform.position = im.ecmList[0].transform.position;
            }
            else if(im.ecmList[0].eventName=="seal"&&im.ecmList[0].active==true)
            {
                if (wm.wcR.wd.gameObject.activeSelf)
                {
                    wm.wcR.wd.gameObject.SetActive(true);
                }
                if (debugTimelineI <= 0)
                {
                    debugTimelineI++;
                    StartCoroutine("PlayTimeLineAgain", true);
                    dm.PlayTimeline("seal", this, im.ecmList[0].am);
                }
                else
                {
                    dm.PlayTimeline("seal", this, im.ecmList[0].am);
                    //生成掉落物品
                }
                    Invoke(("DropItem"), 2f);
                    sm.weaponPower = sm.weaponMaxPower;
                    im.ecmList[0].active = false;
                    GameManager.GM.sealEnemyNum++;
            }
            else if(im.ecmList[0].eventName == "openbox")
            {
                dm.PlayTimeline("openbox", this, im.ecmList[0].am);
            }
        }
    }//委托

    //掉落物品
    public void DropItem()
    {
        if (gameObject.activeSelf)
        {
            GameObject eitem = itemPool.UseTimePool();
            eitem.transform.position = transform.position;
        }
    }

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
        if(cc!=null&&cc.gameObject.tag!="Player")//别人不能锁定
        cc.enabled = false;
        if (gameObject.tag == "Player")
        {
            ac.camcontrol.CancelLock();
            AudioManager.SetPlayerSource(3);
            AudioManager.PlayPlayeraudioSource();
        }
    }
    public void Hit()
    {
        ac.SetTrigger("hit");
        if (this.tag == "Player")
        {
            AudioManager.SetPlayerSource(2);
            AudioManager.PlayPlayeraudioSource();
        }
    }

    public void HitOrDie(WeaponControl _wc)
    {
        Vector3 backForce = -Vector3.Normalize(_wc.wm.am.transform.position-transform.position);
        Rigidbody rig =transform.GetComponent<Rigidbody>();
        rig.AddForce(backForce*10f,ForceMode.Impulse);
        if (sm.Hp > 0)
        {   //敌人攻击力，*此物体减伤,玩家的武器能量条
            float percent = _wc.wm.am.sm.weaponPower / _wc.wm.am.sm.weaponMaxPower;
            if (percent >= 0.2f)
            {
                if (_wc.wm.am.sm.isNLAttack3)
                {
                    sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * (2f));
                    Debug.Log("三段");
                }
                sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * (sm.isInGun ? 0.1f : 1f));
                if(hitVFX!=null)
                {
                    GameObject g = hitVFX.UseTimePool();
                    if (g == null)
                    {
                        Debug.Log("取不出HIT特效");
                    }
                    else
                    {
                        g.transform.position = _wc.transform.position;
                    }
                }
            }
            else
                sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * 0.1f);
            if (sm.Hp > 0)
                Hit();
            else
            {
                Die();
                if (gameObject.tag != "Player")
                    if (_wc.wm.am.ac.camcontrol != null)
                        _wc.wm.am.ac.camcontrol.CancelLock();//攻击者取消锁定
                    else
                        Debug.Log("同类相残");
            }
        }
        else if(!sm.isDie)
        {
            //already dead;
            Die();
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
            if (this.gameObject.tag == "Player")
            {
                if (sm.canWindy)
                {
                    Time.timeScale = 0.2f;
                    scalewindyTimer.StartTimer(2.4f);
                    ac.ChangeBool("timeend", false);
                    _wc.wm.am.ac.SetTrigger("windyidle");
                    //疾风突袭放到UI管理里做？？感觉很不好
                    return;
                }
                else
                    sm.AddHP(-GetAttackerATK(_wc, _wc.wm.am.sm) * (sm.isInGun ? 0.1f : 1f));//敌人攻击力，*此物体减伤
            }
            else
            {
                Time.timeScale = 0.4f;
                scalewindyTimer.StartTimer(1f);
                ac.ChangeBool("timeend", false);
                //_wc.wm.am.ac.SetTrigger("windyidle");
                ac.SetTrigger("windy");
                ac.SetTrigger("windy");
            }
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
                vfx.SetActive(true);
                sm.durable += 200;
                GameManager.GM.backStabEnemyNum++;
                //特效vfx,放在哪里比较好？
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
                    AudioManager.SetPlayerSource(1);
                    AudioManager.PlayPlayeraudioSource();
                }
                else 
                {
                    BreakBlocked();
                    //碎盾特效,不够高效,改在UI判断耐久度里判断

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
    public void TryDoDamage(int i=-2000)
    {
        if (tag == "Player" || tag == "Separation")
            return;
        Debug.Log(i);
        if(sm.isDefence)
        {
            Blocked();
            return;
        }
        else if(sm.isBlocked)
        {
            BreakBlocked();
        }
        if (sm.Hp >0)
        {
            //如果某状态下，伤害翻倍
            Debug.Log(tag+"收到了枪的攻击");
            if(tag!="Player")
            if (transform.GetComponent<AI_Enemy>().atc_Player.separationQue.Count < 5)
                i = -10;
            sm.AddHP(i);
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
        if (gameObject.tag == "Player")
            ac.ChangeBool("lock", true);
        else
        {
            if(!ac.CheckAnimatorState("die"))
            ac.SetTrigger("die");
        }

    }
    public void UnLockAC()
    {
        if (gameObject.tag == "Player")
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
