using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ActorControl : MonoBehaviour
{
    public ActorController nlAC;//更换状态控制器
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
    private bool canLookForward = false;
    //子弹时间，攀爬
    public bool isRun = false;
    public GameObject gunPoint;

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
        anim.SetFloat("forward", playH.Dmo * Mathf.Lerp(anim.GetFloat("forward"), (playH.run ? 2.0f : 1.0f), 0.5f));
    }

    private void FixedUpdate()
    {

    }


    //
    public bool GetLookForwardBool()
    {
        return canLookForward;
    }
    //
    //
    //


    public void ReloadEnd()
    {
        am.sm.ammo = 30;
        anim.SetBool("reload", false);
    }

    public void OnExitReload()
    {
        ReloadEnd();
    }
    public void OnEnterShot()
    {
        rig.AddForce(model.transform.forward * 300f, ForceMode.Force);
    }
    public void OnJumpEnter()
    {
        canActorLock = false;
        playH.inputEnable = false;
        thrustVec = new Vector3(0, 5f, 0);//后期写活
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
        thrustVec = model.transform.forward * 0.8f * anim.GetFloat("attack1hA");//可以写活
        moveVec = moveVec * 0.9f;//攻击既有原来移动的速度，又不会停不下来；
    }
    public void OnEnterWindy()
    {
        canAttack = true;
    }
    public void OnEnterFlip()
    {
        playH.inputEnable = false;
        thrustVec = new Vector3(0, 0, model.transform.forward.z * -20);
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
    public void OnEnterBattle()
    {
        //武器盾牌等设置为装备
        //am检查装备的物品，如果没有盾牌的话不启用盾牌
        am.SetOrCollectWeapon(true);
    }
    public void OnExitBattle()
    {
        //收起武器盾牌，关闭攻击
        am.SetOrCollectWeapon(false);
    }
    //切换武器
    public void OnEnterGunWeapon()
    {
        //如果是没有枪才做
    }

    public void OnExitGunWeapon()
    {

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
    public void OnEnterClimb()
    {
        playH.inputEnable = true;
    }
    public void SetTrigger(string trigerName)
    {
        anim.SetTrigger(trigerName);
    }
    public void ChangeBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }
    public void SetLayerWeight(string layerName, float value)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("defense"), value);
    }

    public void SetFloat(string floatName, float value)
    {
        anim.SetFloat(floatName, value);
    }

    public bool CheckAnimatorState(string stateName, string layerName = "Base Layer")
    {
        int index = anim.GetLayerIndex(layerName);
        bool stateValue = anim.GetCurrentAnimatorStateInfo(index).IsName(stateName);
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
        if (CheckAnimatorState("attack1hC", "Base Layer") || CheckAnimatorState("climbtop"))
        {
            transform.position = transform.position + (Vector3)_deltaPos;
        }
    }
}
