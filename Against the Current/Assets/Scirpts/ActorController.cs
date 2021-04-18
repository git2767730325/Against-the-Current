using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public bool canUseGun = true;
    public minimap mini;
    public float blong;
    public ActorController nlAC;//更换状态控制器
    //public ActorController zzAC;//更换状态控制器
    public RuntimeAnimatorController nlRAC;
    public RuntimeAnimatorController zzRAC;
    public ActorManager am;
    public CameraController camcontrol;
    public GameObject model;
    public GameObject resurrectionObj;
    public IUserInput playH;
    public float walkSpeed;
    public float rollSpeed;
    public float jabSpeed;
    private MyTimer stunnedBackTime=new MyTimer();
    [Header("=======      武器拖尾,手动拖,枪的存在      =========")]
    public GameObject trail;
    //public GameObject gun;
    [Header("===== 需要对象池，暂时手动获取  =====")]
    public Bullets bl;//子弹对象池
    [Header("=====  判断斜坡角度  ======")]
    public Vector3 foot;
    public Vector3 body;
    public Vector3 head;
    public float a=0.2f;
    public float b=1.5f;
    public bool isSlope = false;
    public bool isCanClimb = false;
    public bool isCanTop = false;
    public float lonag=0.5f;
    public float headlong = 1f;
    public Vector3 targetClimbTop;
    [Header("======== friction =======")]
    public PhysicMaterial f0;
    public PhysicMaterial f1;
    [Header("无需设置")]
    public Animator anim;
    [Space(10)]
    private Rigidbody rig;
    private Vector3 moveVec;
    private Vector3 thrustVec;
    private bool canAttack = false;
    private bool canActorLock = false;
    private bool canLookForward = false;
    private bool battleState = true;//判断是否拿剑盾，如果是，锁定后模型一直往目标对象看
    private bool gunState = false;
    private bool nlState = false;//逆态
    //子弹时间，攀爬
    public bool isBulletTime = false;
    public bool isRun = false;
    public bool isCrouch = false;
    [Header("子弹生成点，枪位置看点，恢复位置的物体，是否为AI")]
    public GameObject bulletPoint;//子弹生成点
    [Header("看到点")]
    public GameObject gunLook;//看点的物体
    [Header("和上面一个是同一个物体，这个设置对应位置角度")]
    public GameObject rightPos;
    //public GameObject prefabBullet;
    public bool isAI = false;
    private bool isLock = false;
    [Header("==========      时光倒流相关设置      ===============")]
    //敌人和分身共用
    public string actPlaying;
    public int actLayer;
    //分身用
    public string gunActPlaying;
    //
    public Vector3 modelRot;
    public Vector3 objRot;
    //模型的走路
    public float forwardValue;
    public float rightValue;
    //枪的位置修正
    public Vector3 gunFixPos;
    //public float sppea=1;
    //public bool sdsaf;
    //public AnimatorRecorderMode ar;
    //事件
    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;
    //private Vector3 rollVec;
    // Start is called before the first frame update
    private void Awake()
    {
        mini = transform.GetComponentInChildren<minimap>();
        am = GetComponent<ActorManager>();
        IUserInput[] inputType = GetComponents<IUserInput>();
        //多套控制方式
        foreach (var input in inputType)
        {
            if (input.enabled == true)
            {
                playH = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rig = gameObject.GetComponent<Rigidbody>();
        stunnedBackTime = new MyTimer();
        if (gameObject.tag == "Separation")
        {
            bl = GameObject.FindWithTag("bullets").GetComponent<Bullets>();
            Debug.Log("如果对象池没有标签将会报错");
        }
    }
    private void Start()
    {
        if (tag == "Player")
        {
            AudioManager.SetBackGroundClip(1);
            AudioManager.PlayBackGroundSource();
        }
        GameManager.GM.player = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        modelRot = model.transform.eulerAngles;
        forwardValue = anim.GetFloat("forward");
        rightValue = anim.GetFloat("right");
        if (isAI)
        {
            return;
        }//AI则返回，AI自己有控制逻辑
        //地图放大缩小
        if(mini!=null)
        {
            mini.SetMiniMapSize(15+rig.velocity.magnitude*3);
        }
        /*
        if(Input.GetKeyDown(KeyCode.X))
        {
            anim.Play("Move",0,0.7f);
            anim.SetFloat("back",-2f);
        }*/

        //武器切换
        if (nlState)
        {
            if (canUseGun)
            {
                anim.SetBool("isgun", gunState);
            }
            else
                gunState = false;
            if(!gunState&&rightPos.transform.localEulerAngles!=Vector3.zero)
            {
                rightPos.transform.localEulerAngles = Vector3.zero;
            }
            //切换武器
            am.wm.ChangeWeapon(gunState);
        }
        //切换战斗状态
        else
        {
            if(am.sm.isGround)
            anim.SetBool("battle", battleState);
            Debug.Log("battle");
        }
        //爬山状态监测
        TestClimbing();
        //爬山，爬山就返回return ,用函数内就不会返回
        //小心！！！！

        Climb();
        if (CheckAnimatorState("climb"))
            return;
        //模式切换,非锁定下才能切换
        if (!camcontrol.lockState && playH.Btn4.OnPressed && !gunState&&CheckAnimatorState("Move"))
        {
            if (!nlState)
            {
                anim.runtimeAnimatorController = nlRAC;
                anim.SetBool("isground",true);
                anim.Play("Move");
                am.wm.wcR.wd.gameObject.SetActive(true);
                if(am.wm.wcL.wd!=null)
                am.wm.wcL.wd.gameObject.SetActive(false);
            }
            else
            {
                if (!playH.KeyD.IsPressing)
                {

                    anim.runtimeAnimatorController = zzRAC;
                    //武器切换
                    if (am.wm.wcR.wd.gameObject.activeSelf)
                        am.wm.wcR.wd.gameObject.SetActive(false);
                }
                else
                    nlState = !nlState;
            }
            nlState = !nlState;
        }
        //切换枪态
        if (playH.Btn3.OnPressed&& !camcontrol.lockState &&nlState)//切换枪态
        {
            gunState = !gunState;
            if (!canUseGun)
                return;
            Debug.LogError("配置枪");
            am.wm.ChangeWeapon(gunState);
        }
        //走路跑步过渡平滑,两种移动，锁死视角和解锁
        if(!gunState)
        {
            //转向平滑;相机锁定否
            camcontrol.transform.parent.localPosition =new Vector3(0, camcontrol.transform.parent.localPosition.y,0f);
            //进入战斗状态否？
            if (playH.KeyD.OnPressed && !CheckAnimatorState("die") && !nlState)
            {
                battleState = true;
                anim.SetBool("battle", battleState);
                if (!am.wm.wcR.wd.gameObject.activeSelf)
                    anim.SetTrigger("set");
                camcontrol.LockUnLock();
            }
            else if (playH.attack && canAttack&&!nlState)
            {
                if (!am.wm.wcR.wd.gameObject.activeSelf)
                {
                    battleState = true;
                    anim.SetBool("battle", battleState);
                    anim.SetTrigger("set");
                }
            }
            //非锁定下
            if (!camcontrol.lockState)
            {
                if (!nlState)
                {
                    canLookForward = !(playH.Dmo > 0.01f && playH.inputEnable);//判断视野是否可以面向模型前方
                    if (playH.Dmo > 0.01f && playH.inputEnable)//有移动量时按键正前方和视角对齐
                    {
                        anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                        anim.SetFloat("right", 0);
                        //跑步取消武器
                        if (playH.run)
                            if (am.wm.wcR.wd.gameObject.activeSelf)
                            {
                                //写成方法
                                battleState = false;
                                anim.SetBool("battle",battleState);
                                anim.SetTrigger("set");
                            }
                        Vector3 targetForward = Vector3.Slerp(anim.transform.forward, playH.Dvec, 0.3f);
                        anim.transform.forward = targetForward;
                    }
                    else if (playH.inputEnable)
                    {
                        anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                        anim.SetFloat("right", 0);
                    }//避免一直卡动画
                }
                else//逆态，新动画控制器
                {
                    //如果是玩家才有后续
                    if (gameObject.tag == "Player")
                    {
                        //拖尾特效
                        if (trail != null)
                            if (gameObject.tag == "Player" && trail.activeSelf != am.sm.isAttack)
                            {
                                trail.SetActive(am.sm.isAttack);
                            }
                        if (playH.Dmo > 0.1f)
                        {
                            anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), 2.0f, 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                            anim.SetFloat("right", 0);
                            Vector3 targetForward = Vector3.Slerp(anim.transform.forward, playH.Dvec, 0.3f);
                            anim.transform.forward = targetForward;
                        }
                        else if (playH.inputEnable)
                        {
                            anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                            anim.SetFloat("right", 0);
                        }//避免一直卡动画
                        if (playH.inputEnable && playH.KeyA.OnPressed)
                        {
                            anim.SetTrigger("roll");
                        }
                    }
                }
            }
            //锁定下
            else
            {
                //松开按键取消锁定,疾风突袭下不取消，未做
                if (playH.KeyD.OnReleased)
                {
                    //如果疾风突袭下不取消
                    if(!am.sm.isWindy&&!CheckAnimatorState("flip"))
                        camcontrol.CancelLock();
                }
                if (battleState == true)
                {
                    if (playH.run)
                    {
                        battleState = false;//跑步收回武器和盾，解除武装状态
                        anim.SetBool("battle", battleState);
                        if(am.wm.wcR.wd.gameObject.activeSelf)
                        anim.SetTrigger("set");
                    }
                    else if (playH.jump)
                        anim.SetTrigger("flip");
                }//如果武装态，决定是否取消武装
                if (!canActorLock && playH.Dvec.normalized != Vector3.zero)
                {
                    model.transform.forward = playH.Dvec.normalized;
                }//
                //transform.forward = new Vector3(camcontrol.transform.forward.x, 0f, camcontrol.transform.forward.z);
                else if (playH.Dmo > 0.01f && playH.inputEnable)
                {
                    if (battleState)//持有剑\盾下，模型看向父，父在锁定时看向敌人
                    {
                        model.transform.forward = transform.forward;
                        Vector3 targetVec = transform.InverseTransformDirection(playH.Dvec);
                        anim.SetFloat("forward", targetVec.z * playH.Dmo);//不能跑
                        anim.SetFloat("right", targetVec.x * playH.Dmo);
                        //anim.SetFloat("forward", targetVec.z * playH.Dmo * (playH.run ? 2.0f : 1.0f));//
                    }
                    else//和无锁定差不多
                    {
                        Vector3 targetForward = Vector3.Slerp(anim.transform.forward, playH.Dvec, 0.3f);
                        anim.transform.forward = targetForward;
                        anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                        anim.SetFloat("right", 0);
                    }
                }
                else if(playH.inputEnable)
                {
                    anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                    anim.SetFloat("right", 0);
                }
            }
            if (anim.GetFloat("forward") > 1.1f)
            {
                isRun = true;
            }
        }
        //枪态
        else//枪下
        {
            //上子弹，可以添加射击时间间隔
            if (playH.Btn2.OnPressed && am.sm.ammo < 30)
            {
                anim.SetBool("reload", true);
                AudioManager.SetAndPlayVfxSource(1);
            }
            //Vector3 temp;
            model.transform.forward = transform.forward;
            Vector3 targetVec = transform.InverseTransformDirection(playH.Dvec);
            anim.SetFloat("forward", targetVec.z * playH.Dmo);//不能跑
            anim.SetFloat("right", targetVec.x * playH.Dmo);
            camcontrol.transform.parent.localPosition = new Vector3(1,camcontrol.transform.parent.localPosition.y, 1f);
            //聚+子弹时间
            if (playH.KeyD.IsPressing)
            {
                camcontrol.transform.parent.localPosition = new Vector3(1, camcontrol.transform.parent.localPosition.y, 2.7f);
                if (playH.Btn7.OnPressed)//凝神，可以开启子弹时间
                {
                    if (Time.timeScale >= 1)
                    {
                        if (am.sm.canUseStamina)
                        {
                            Time.timeScale = 0.5f;
                            isBulletTime = true;
                        }
                    }
                    else
                    {
                        Time.timeScale = 1f;
                        isBulletTime = false;
                    }
                }
                else if(!am.sm.canUseStamina&&Time.timeScale==0.5f)
                 {
                    Time.timeScale = 1f;
                    isBulletTime = true;
                 }
            }
            else if (playH.KeyD.OnReleased)
            {
                Time.timeScale = 1f;
                isBulletTime = false;
            }

        }
        //特定状态下锁住速度输入
        LockSpeedInput();
        //跳跃，攻击，开火，因为不能同时，所以写在一起
        JumpAndAttackAndFire();
        //持盾逻辑
        DefenceAndCounterBack();
        //玩家盾反需要冷却一下
        CounterBackCD();
        //速度快时翻滚
        QuickToRoll();
        //蹲下，后续还有很多东西可以加,应该可以放在playhandle里判断
        Crounch();
        //
        //复活
        if(camcontrol.uim.resurrection.activeSelf&&playH.Btn6.OnPressed&&am.sm.isDie)
        {
            anim.SetTrigger("resurrection");
            resurrectionObj.SetActive(true);
            Invoke("CloseResume", 3f);
        }
        //启用委托,各类事件，只会放一个事件来启用
        if(playH.action)
        {
            OnAction.Invoke();
            Debug.Log("invoke");
        }
        //
    }

    private void FixedUpdate()
    {
        if (isLock)
        {
            rig.velocity =Vector3.zero;
            return;
        }

        stunnedBackTime.Tick();
        //判断是否可爬坡
        if (!isCanClimb)
            rig.velocity = new Vector3(moveVec.x / (isSlope ? 1.43f : 1.0f), rig.velocity.y, moveVec.z / (isSlope ? 1.43f : 1.0f)) + thrustVec;//y=0,避免重力失效
        else
        {
            rig.velocity = moveVec;
        }
        //若果到顶了，给一定的速度,直接给位移更好
        if (CheckAnimatorState("climbtop"))
        {
            rig.velocity = Vector3.zero;
            if (targetClimbTop.y >= transform.position.y)
            {
                //transform.position += model.transform.forward * 0.01f;
                transform.position += model.transform.up * 0.05f;
            }
            //rig.velocity += new Vector3(0, 0.2f, 0f);
        }
        
        //冲量重置，避免力用两次！！！！！！！！！！！！！
        thrustVec = Vector3.zero;
        AimingCorrection(); //枪的位置视角矫正
    }


    //时间

    //复活特效关闭
    public void CloseResume()
    {
        resurrectionObj.SetActive(false);
    }
    //
    //各个动作以及判断
    //

    //速度足够时，触发翻滚
    public void QuickToRoll()
    {
        if (rig.velocity.magnitude > 10f)
        {
            if (!gunState)
                anim.SetTrigger("roll");
        }
    }

    //锁死速度输入，非逆态非枪态
    public void LockSpeedInput()
    {
        if (playH.inputEnable)//后期可能需要改为别的bool判断是否锁死速度
        {
            if (!gunState && !nlState)
                moveVec = playH.Dmo * playH.Dvec * walkSpeed * (playH.run ? 2.0f : 1.0f);//速度用变量,通过这个将Update 传到fixedUpdata里
            else
            {
                if (gunState)
                    moveVec = moveVec = playH.Dmo * playH.Dvec * walkSpeed * 1.4f;
                else
                    moveVec = playH.Dmo * playH.Dvec * walkSpeed * 2.8f;
            }
        }
    }

    //跳跃，取消枪态，控制攻击条件,耦合过高，有恢复正常时间的操作
    public void JumpAndAttackAndFire()
    {
        if (playH.jump)
        {
            anim.SetTrigger("jump");
            //如果疾风突袭下，可以攻击
            canAttack = false;
            if (gunState)
            {
                Time.timeScale = 1f;
                gunState = false;
                am.wm.ChangeWeapon(gunState);
            }
        }
        else if (playH.attack && canAttack&&!am.sm.isDie)
        {
            if (gunState)
            {//添加非上子弹的情况
                if (am.sm.ammo >= 1 && CheckAnimatorState("gunidle", "Gun"))
                {
                    am.sm.ammo -= 1;
                    anim.SetTrigger("shot");
                    //开火
                    AudioManager.SetAndPlayVfxSource(0);
                    GameObject bullet_obj = bl.UseTimePool();
                    bullet_obj.transform.position = bulletPoint.transform.position;
                    bullet_obj.transform.rotation = bulletPoint.transform.rotation;
                    Rigidbody bullet = bullet_obj.GetComponent<Rigidbody>();
                    if (bullet.transform.parent != null)
                    {
                        bullet.velocity = bulletPoint.transform.up * 50f;
                    }
                    if (am.sm.ammo == 0)
                    {
                        //上子弹
                        AudioManager.SetAndPlayVfxSource(1);
                        anim.SetBool("reload", true);
                    }
                    return;//不进行近战攻击
                }
                else
                {
                    //
                    //上子弹
                    //AudioManager.SetAndPlayVfxSource(1);
                    //anim.SetBool("reload", true);
                    return;
                }
            }
            else
                anim.SetTrigger("attack");
    }
    }


    //检测爬山状态，以及是否到顶
    public void TestClimbing()
    {
        RaycastHit hitF;
        RaycastHit hitB;
        //角度爬
        Vector3 c = model.transform.position;
        Vector3 forward = model.transform.forward;
        foot = new Vector3(c.x, c.y + a, c.z);
        body = new Vector3(c.x, c.y + b, c.z);
        head = new Vector3(c.x, c.y + 2f, c.z);
        Vector3 up = body - foot;
        isSlope = Physics.Raycast(foot, forward, out hitF, 1.3f, LayerMask.GetMask("Ground"));
        isCanClimb = Physics.Raycast(body, forward, out hitB, lonag, LayerMask.GetMask("Ground"));
        Collider[] cols = Physics.OverlapBox(head + model.transform.forward, new Vector3(1, 0.1f, 2), model.transform.rotation, LayerMask.GetMask("Ground"));
        if (cols.Length == 0)
            isCanTop = true;//判断是否到顶
        else
            isCanTop = false;
        if (isSlope && isCanClimb && isCanTop && CheckAnimatorTag("climb"))//后面取消标签
            anim.SetTrigger("climbtop");
    }
    //攀爬，有return;
    public void Climb()
    {
        if (!nlState)
        {
            if (playH.run && !am.sm.canUseStamina)
                playH.run = false;

            //BUG多发地
            anim.SetBool("climb", isCanClimb && isSlope);
            if (isCanClimb && isSlope)
            {
                if (battleState)
                {
                    battleState = false;
                    OnExitBattle();
                }
                isRun = false;
                anim.SetBool("battle", battleState);
                    //playH.Dup = -playH.Dup;
                anim.SetFloat("up", playH.Dup);
                anim.SetFloat("right", playH.Dright);
                if (Mathf.Abs(transform.eulerAngles.y - model.transform.eulerAngles.y) > 90)
                moveVec = model.transform.up * -1f * playH.Dup + model.transform.forward * -0.4f * playH.Dup
                    + model.transform.right * 1f * playH.Dright;
                else
                    moveVec = model.transform.up * 1f * playH.Dup + model.transform.forward * 0.4f * playH.Dup
                    + model.transform.right * 1f * playH.Dright;
                return;
            }
        }
    }
    //蹲下
    public void Crounch()
    {
        if (playH.Btn8.OnPressed)
        {
            if (!nlState && !gunState)
            {
                SetTrigger("crouch");
                OnExitBattle();
            }
        }
        if (anim.GetFloat("forward") < 1.1f || nlState)
        {
            isRun = false;
        }//修正bool
    }

    //防御和盾反,耦合太高了
    public void DefenceAndCounterBack()
    {
        if (am.wm.wcL.wd != null && !nlState && (CheckAnimatorState("Move") || CheckAnimatorState("jump")))//只有在地上或者跳跃可以防御
        {
            anim.SetBool("defense", playH.defence);
            if (playH.defence && !CheckAnimatorState("blocked") && !CheckAnimatorState("stunned"))
            {
                if (anim.GetLayerWeight(anim.GetLayerIndex("defense")) < 1.0f)
                    SetLayerWeight("defense", (anim.GetLayerWeight(anim.GetLayerIndex("defense")) + 0.2f));
            }
            else
                SetLayerWeight("defense", 0f);
        }
        else if (CheckAnimatorTag("counterback"))
        {
            anim.SetBool("defense", false);
            SetLayerWeight("defense", 0f);
        }
        else
        {
            playH.defence = false;//避免无法取消举盾
            if (!gunState && !nlState)
            {
                anim.SetBool("defense", playH.defence);
                SetLayerWeight("defense", 0f);
            }
        }
    }

    //盾反CD
    public void CounterBackCD()
    {
        if (CheckAnimatorState("defence", "defense") && playH.counterBack && !CheckAnimatorTag("counterback") && !(stunnedBackTime.state == MyTimer.State.RUN))
        {
            if (stunnedBackTime.state == MyTimer.State.IDLE)
            {
                stunnedBackTime.StartTimer(0.2f);//开始计时
                anim.SetTrigger("stunned");
            }
            if (stunnedBackTime.state == MyTimer.State.FINISHED)
            {
                stunnedBackTime.state = MyTimer.State.IDLE;
                anim.SetTrigger("stunned");
                stunnedBackTime.StartTimer(0.2f);
            }
        }
    }

    //分身也要
    public void AimingCorrection()
    {
        if (gameObject.tag == "Separation")
            return;
        //枪的位置视角矫正,如果在枪态，并且枪矫正的物体不为空
        if (gunLook != null && gunState)
        {
            RaycastHit rayH;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out rayH, 500))
            {
                if (rayH.distance > blong+5)
                    gunLook.transform.LookAt(rayH.point);
                else
                    gunLook.transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50)));
            }
            else
                gunLook.transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50)));
            gunFixPos =gunLook.transform.eulerAngles;

        }
    }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <returns></returns>

    //视野与相机相关
    public bool GetLookForwardBool()
    {
        return canLookForward;
    }
 

    public void ReloadEnd()
    {
        //上完子弹
        am.sm.ammo = 30;
        anim.SetBool("reload",false);
    }
    public void OnExitReload()
    {
        if(gunState)
            ReloadEnd();
        else
            AudioManager.StopVfxAudioSource();
    }
    public void OnEnterShot()
    {
        rig.AddForce(model.transform.forward * 300f, ForceMode.Force);
    }
    public void OnJumpEnter()
    {
        canActorLock = false;
        playH.inputEnable = false;
        thrustVec = new Vector3(0,5f, 0);//后期写活
        //还需要锁死速度（上面调变）
    }
    public void OnJumpExit()
    {

    }
    public void OnFallEnter()
    {

        playH.inputEnable = false;
        //thrustVec = new Vector3(0, 5f, 0);//后期写活
        //还需要锁死速度（上面调变）
    }
    public void OnEnterGround()
    {
        playH.inputEnable = true;
        isLock = false;
        canAttack = true;
        canActorLock = true;
    }
    public void IsGround()
    {
        playH.ground = true;
        anim.SetBool("isground", true);
    }
    public void IsNotGround()
    {
        playH.ground = false;
        anim.SetBool("isground", false);
    }

    public void OnEnterRoll()
    {
        canActorLock = false;
        //thrustVec = model.transform.forward *144.4f;//瞬移预定
        thrustVec += new Vector3(0, rollSpeed, 0);
        playH.inputEnable = false;
    }

    public void OnEnterJab()
    {
        thrustVec = model.transform.forward * -jabSpeed;
        thrustVec += new Vector3(0, 2f, 0);
        playH.inputEnable = false;
    }
    public void OnStayJab()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabCurve");
    }

    public void OnEnterAttack1hA()
    {
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 1.0f);
        playH.inputEnable = false;
    }
    public void OnStayAttack1hA()
    {
        thrustVec= model.transform.forward *0.8f* anim.GetFloat("attack1hA");//可以写活
        moveVec = moveVec * 0.9f;//攻击既有原来移动的速度，又不会停不下来；
    }
    public void OnEnterWindy()
    {
        model.transform.forward = transform.forward;
        //canAttack = true;
    }
    public void OnEnterFlip()
    {
        //继续调节
        playH.inputEnable = false;
        thrustVec = new Vector3(0, 0, model.transform.forward.z * -15);
    }
    public void OnStayWindy()
    {
        thrustVec = new Vector3(0, 0, model.transform.forward.z * 5f);
    }


    public void OnEnterHit()
    {
        playH.inputEnable = false;
    }

    public void OnEnterDie()
    {
        playH.inputEnable = false;
        isLock = true;
        //碰撞体去掉
        if(gameObject.tag!="Player"&&gameObject.tag!="Separation")
        {
            CapsuleCollider cp = GetComponent<CapsuleCollider>();
            cp.enabled = false;
        }
    }
    public void OnEnterBattle()
    {
        //武器盾牌等设置为装备
        //am检查装备的物品，如果没有盾牌的话不启用盾牌
        battleState = true;
        ChangeBool("battle",true);
        am.SetOrCollectWeapon(true);
    }
    public void OnExitBattle()
    {
        //收起武器盾牌，关闭攻击
        if (!am.sm.isAttack)
        {
            ChangeBool("battle",false);
            battleState = false;
            am.SetOrCollectWeapon(false);
        }
        else
        {
            ChangeBool("battle", true);
            battleState = true;
            am.SetOrCollectWeapon(true);
            Debug.Log("失败了"+battleState+"aa"+anim.GetBool("battle"));
        }
    }
    //切换武器
    public void OnEnterGunWeapon()
    {
        //如果是没有枪才做，分身做

        gunState = true;
        anim.SetBool("isgun", gunState);
        am.wm.ChangeWeapon(true);

    }
    public void OnExitGunWeapon()
    {
        gunState = false;
        anim.SetBool("isgun", gunState);
        am.wm.ChangeWeapon(false);
        if (rightPos.transform.localEulerAngles != Vector3.zero)
        {
            rightPos.transform.localEulerAngles = Vector3.zero;
        }
    }
    public void OnEnterBlocked()
    {
        playH.inputEnable = false;
    }
    public void OnExitAttack()
    {
        //被打时关闭自己的武器攻击，或者 改到攻击动画跳出关闭武器碰撞体
        am.HitCloseWeapon(); 
    }
    public void OnEnterLock()
    {
        playH.inputEnable = false;
        isLock = true;
    }
    public void OnExitLock()
    {
        playH.inputEnable = true;
        isLock = false;
        battleState = true;
    }

    public void OnEnterClimb()
    {
        playH.inputEnable = true;
    }
    public void OnEnterTop()
    {
        playH.inputEnable = false;
        targetClimbTop=transform.position +model.transform.forward * 0.2f + model.transform.up * 2f;
        transform.position += model.transform.up * 0.5f;
    }



    //设置防御层级的权重
    public void SetLayerWeight(string layerName,float value)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("defense"), value);
    }
    //anim的操作
    public void SetTrigger(string trigerName)
    {
        anim.SetTrigger(trigerName);
    }
    public void ChangeBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }
    public void SetFloat(string floatName,float value)
    {
        anim.SetFloat(floatName,value);
    }



    //mori
    public void LongSword()
    {
        if(nlState&&!gunState)
        anim.Play("doomoath");
    }

    //时光倒流相关
    public void ChangeActAndPlayBack(string _actName,int _layerNum)
    {
        Debug.Log(_actName);
        bool isS =(gameObject.tag == "Sepataration");
        anim.SetFloat("back",isS?-2.6f:-2f);
        if (actPlaying != _actName)
        {
            if (gameObject.tag == "Separation")
            {
                anim.Play(_actName, _layerNum, 0.7f);
            }
            else if (gameObject.tag != "Player")
            {
                anim.Play(_actName, _layerNum, 0.7f);
                if (_layerNum == 1)
                    anim.Play("Move",0,0.7f);
            }
        }
    }
    public void StopPlayBack()
    {
        anim.SetFloat("back", 1f);
    }
    public void StopPlayBackS(string _actName, int _layerNum)
    {
        if(anim.GetFloat("back")!=1f)
            anim.SetFloat("back", 1f);
        if (actPlaying!= _actName)
        {
            anim.Play(_actName, _layerNum,0.1f);
            if(_actName=="shot")
            {
                GameObject bullet_obj = bl.UseTimePool();
                bullet_obj.transform.position = bulletPoint.transform.position;
                bullet_obj.transform.rotation = bulletPoint.transform.rotation;
                Rigidbody bullet = bullet_obj.GetComponent<Rigidbody>();
                if (bullet.transform.parent != null)
                {
                    bullet.velocity = bulletPoint.transform.up * 50f;
                    Debug.Log("子弹生成，并且优速贷");
                }
            }
            Debug.Log(actPlaying+"   "+_actName);
        }
    }

    public bool CheckAnimatorState(string stateName,string layerName="Base Layer")
    {
        int index=anim.GetLayerIndex(layerName);
        if (index == -1)
            return false;
        bool stateValue=anim.GetCurrentAnimatorStateInfo(index).IsName(stateName);
        if (stateValue)
        {
            //如果是玩家，需要判断第二层的状态来传参数
            //逻辑太慌乱了
            actLayer = index;
            if (actLayer == 0)
                actPlaying = stateName;
            else if(actLayer==1)
                gunActPlaying= stateName;
            if (gameObject.tag == "Player"||gameObject.tag=="Separation")
            {
                if (gunActPlaying == "empty")
                {
                    actLayer = 0;
                   // Debug.Log(actPlaying+actLayer);
                }
                else
                    actPlaying = gunActPlaying;
            }
            else
                actPlaying = stateName;
        }
        return stateValue;
    }

    public bool CheckAnimatorTag(string tagName, string layerName = "Base Layer")//看动画里的标签
    {
        int index = anim.GetLayerIndex(layerName);
        if (index == -1)
            return false;
        bool stateValue = anim.GetCurrentAnimatorStateInfo(index).IsTag(tagName);
        return stateValue;
    }
    

    //动画自身的移动量，无需调用，放入函数里使用
    public void OnUpdateRootMotion(object _deltaPos)
    {
        //设置条件，加上动画本身速度
        if (CheckAnimatorState("attack1hC","Base Layer")||CheckAnimatorState("climbtop"))
        {
            transform.position = transform.position + (Vector3)_deltaPos;
        }
    }
}
