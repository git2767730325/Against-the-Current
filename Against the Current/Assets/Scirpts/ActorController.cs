using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public ActorManager am;
    public CameraController camcontrol;
    public GameObject model;
    public IUserInput playH;
    public float walkSpeed;
    public float rollSpeed;
    public float jabSpeed;

    [Header("======== friction =======")]
    public PhysicMaterial f0;
    public PhysicMaterial f1;


    [SerializeField]
    public Animator anim;
    private Rigidbody rig;
    private Vector3 moveVec;
    private Vector3 thrustVec;
    private bool canAttack = false;
    private bool canActorLock = false;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;
    //private Vector3 rollVec;
    // Start is called before the first frame update
    private void Awake()
    {
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
    }
    // Update is called once per frame
    void Update()
    {
        //暂时使用
        if (rig.velocity.magnitude>5f)
        {
            anim.SetTrigger("roll");
        }
        //走路跑步过渡平滑,两种移动，锁死视角和解锁
        {
            //转向平滑;相机锁定否
            if (!camcontrol.lockState)
            {
                anim.SetFloat("forward",playH.Dmo*Mathf.Lerp(anim.GetFloat("forward"),(playH.run?2.0f:1.0f),0.5f));//lerp第一个参数不用playH.Dmo,不然会突变
                anim.SetFloat("right",0);
                if (playH.Dmo > 0.01f && playH.inputEnable)
                { 
                Vector3 targetForward = Vector3.Slerp(anim.transform.forward, playH.Dvec, 0.3f);
                anim.transform.forward = targetForward;
                }

            }
            else
            {
                if (!canActorLock&&playH.Dvec.normalized!=Vector3.zero)
                    model.transform.forward = playH.Dvec.normalized;
                //transform.forward = new Vector3(camcontrol.transform.forward.x, 0f, camcontrol.transform.forward.z);
                else if (playH.Dmo > 0.01f && playH.inputEnable)
                {
                    model.transform.forward = transform.forward;
                    Vector3 targetVec = transform.InverseTransformDirection(playH.Dvec);
                    anim.SetFloat("forward", targetVec.z * playH.Dmo * (playH.run ? 2.0f : 1.0f));//
                    anim.SetFloat("right", targetVec.x * playH.Dmo * (playH.run ? 2.0f : 1.0f));
                }
            }
        }
        if(playH.inputEnable)//后期可能需要改为别的bool判断是否锁死速度
        moveVec = playH.Dmo * playH.Dvec*walkSpeed*(playH.run?2.0f:1.0f);//速度用变量,通过这个将Update 传到fixedUpdata里

        if (playH.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        else if (playH.attack &&canAttack)
            anim.SetTrigger("attack");

        SetLayerWeight("defense", 0f);
        if (!CheckAnimatorState("roll") && !CheckAnimatorTag("attack")&& !CheckAnimatorTag("stunback"))//翻滚和攻击时不能进入防御状态，提前防御另算
        {
            anim.SetBool("defense", playH.defence);
            if(playH.defence&&!CheckAnimatorState("blocked")&&!CheckAnimatorState("stunned"))
                SetLayerWeight("defense", 1f);
            else
                SetLayerWeight("defense", 0f);
        }
        if(CheckAnimatorState("defence","defense")&&playH.counterBack)
        {
            anim.SetTrigger("stunned");
        }

        if(playH.action)
        {
            OnAction.Invoke();
        }
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector3(moveVec.x,rig.velocity.y,moveVec.z)+thrustVec;//y=0,避免重力失效
        thrustVec = Vector3.zero;
    }


    //
    //
    //
    //
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

    public void OnEnterAttack1hC()
    {
    }

    public void OnExitAttack1hC()
    {

    }

    public void OnEnterHit()
    {
        playH.inputEnable = false;
    }

    public void OnEnterDie()
    {
        playH.inputEnable = false;
        //跳跃死亡，人物一直滑
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
    public void SetTrigger(string trigerName)
    {
        anim.SetTrigger(trigerName);
    }
    public void ChangeBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }
    public void SetLayerWeight(string layerName,float value)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("defense"), value);
    }

    

    public bool CheckAnimatorState(string stateName,string layerName="Base Layer")
    {
        int index=anim.GetLayerIndex(layerName);
        bool stateValue=anim.GetCurrentAnimatorStateInfo(index).IsName(stateName);
        return stateValue;
    }

    public bool CheckAnimatorTag(string tagName, string layerName = "Base Layer")//看动画里的标签
    {
        int index = anim.GetLayerIndex(layerName);
        bool stateValue = anim.GetCurrentAnimatorStateInfo(index).IsTag(tagName);
        return stateValue;
    }

    public void OnUpdateRootMotion(object _deltaPos)
    {
        //设置条件，加上动画本身速度
        if (CheckAnimatorState("attack1hC","Base Layer"))
        {
            transform.position = transform.position + (Vector3)_deltaPos;
        }
    }

    //public void WaitAttackEnd()
    //{
    //    anim.SetLayerWeight(anim.GetLayerIndex("attack"),0f);
    //    playH.inputEnable = true;
    //}
}
