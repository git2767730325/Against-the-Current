using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    [Header("========= base ==========")]
    public float Hp=15.0f;//不规范命名
    public float HpMax = 300f;
    public float ATK = 3f;
    public float DEF = 0f;
    public float stamina = 200f;//体力
    public float weaponPower = 100f;
    public float weaponMaxPower = 100f;//与主武器配套
    public int ammo = 30;
    public int ammoMax = 30;
    public float durable = 0f;//与盾牌配套
    public bool canUseStamina = true;
    [Header("First state flag")]
    public bool isGround;
    public bool isRoll;
    public bool isJump;
    public bool isHit;
    public bool isDie;
    public bool isFall;
    public bool isJab;
    public bool isAttack;
    public bool isDefence;
    public bool isBlocked;
    public bool isInGun;
    public bool isFlip;
    [Header("Second state flag")]
    public bool isRun;
    public bool isAllowDefence;
    public bool isImmortal;
    public bool isStunned;
    public bool isCounterBack;
    public bool isWindy;
    [Header("======  actorBool  =======")]
    public bool canCounterBack = true;
    public bool canWindy = true;
    public bool canReloadWP = false;
    [Header("======  Timer  =========")]
    MyTimer wpReloadTimer = new MyTimer();
    private void Start()
    {
        Hp=Mathf.Clamp(Hp, 0, HpMax);
    }

    private void Update()
    {
        //各个状态
        isGround=am.ac.CheckAnimatorState("Move");
        isRoll=am.ac.CheckAnimatorState("roll");
        isJump=am.ac.CheckAnimatorState("jump");
        isHit=am.ac.CheckAnimatorState("hit");
        isDie=am.ac.CheckAnimatorState("die");
        isFall = am.ac.CheckAnimatorState("fall");
        isJab =am.ac.CheckAnimatorState("jab");
        isBlocked =am.ac.CheckAnimatorState("blocked");
        isAttack = am.ac.CheckAnimatorTag("attack");
        isRun=am.ac.CheckAnimatorState("Move")&&am.ac.playH.run;
        isAllowDefence = isBlocked || isGround;
        isDefence=am.ac.CheckAnimatorState("defence","defense")&&isAllowDefence;
        isImmortal = isRoll || isJab;//无敌状态
        isStunned = am.ac.CheckAnimatorState("stunned");
        isCounterBack = am.ac.CheckAnimatorState("counterback")&&canCounterBack;
        isFlip = am.ac.CheckAnimatorState("flip");
        //在持枪下
        isInGun = am.ac.CheckAnimatorTag("gun","Gun");

        //疾风突袭
        isWindy = am.ac.CheckAnimatorTag("windy");

        //体力相关
        //判断体力红条
        if ((am.ac.isRun||am.ac.isBulletTime)&&canUseStamina)
        {
            stamina -= 0.9f;
            if (stamina <= 0)
                canUseStamina = false;
        }
        else if (stamina < 200)
        {
            stamina += 0.5f;
        }
        else if (!canUseStamina && stamina >= 200)
            canUseStamina = true;
        if (weaponPower <= 0)
        {
            canReloadWP = true;
        }
        else if(weaponPower>=weaponMaxPower)
        {
            weaponPower = weaponMaxPower;
            canReloadWP = false;
        }
    }

    private void FixedUpdate()
    {
        if(canReloadWP)
        {
            weaponPower += 0.05f;
        }
    }

    public void AddHP(float value)
    {
        {
            Hp += value;
            Hp = Mathf.Clamp(Hp, 0, HpMax);
        }
    }
    
}
