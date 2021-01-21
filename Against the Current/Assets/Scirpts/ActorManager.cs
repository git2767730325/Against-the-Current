using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    public WeaponManager wm;
    public BattleManager bm;
    public StateManager sm;
    public DirectorManager dm;
    public InterActorManager im;

    public RuntimeAnimatorController a1;
    public AnimatorOverrideController a2;

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
        if(Input.GetKeyDown(KeyCode.P))
        {
            ac.anim.runtimeAnimatorController = a1;
        }
    }


    public void DoAction()
    {
        if (im.ecmList.Count > 0)
        {
            if (im.ecmList[0].eventName == "stab")
            {
                dm.PlayTimeline("stab", this, im.ecmList[0].am);
            }
            else if(im.ecmList[0].eventName == "openbox")
            {
                dm.PlayTimeline("openbox", this, im.ecmList[0].am);
            }
        }
    }
    public void Die()
    {
        ac.SetTrigger("die");
    }

    public void Hit()
    {
        ac.SetTrigger("hit");
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

    public void TryDoDamage(WeaponControl _wc)
    {

        //闪避等无敌状态
        if (sm.isImmortal)
            return;
        if(Vector3.Angle(_wc.wm.am.ac.model.transform.forward, ac.model.transform.forward)>150)//盾反角度条件
        if (sm.isCounterBack)
        {
            _wc.wm.am.BreakStun();
            return;
        }
        if (sm.isDefence)
        {
            ac.SetLayerWeight("denfense", 0);
            Blocked();
        }
        else
        {
            
            if (sm.Hp > 0)
            {
                sm.AddHP(-5f);
                if (sm.Hp > 0)
                    Hit();
                else
                    Die();
            }
            else
            {
                //already dead;
            }
        }
        //

        //
    }

    public void LockAC()
    {
        ac.ChangeBool("lock",true);
    } 
    public void UnlockAC()
    {
        ac.ChangeBool("lock",false);
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
