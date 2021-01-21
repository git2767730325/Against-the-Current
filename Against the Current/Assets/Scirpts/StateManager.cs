using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float Hp=15.0f;
    public float HpMax = 300f;
    public float ATK = 3f;
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
    [Header("Second state flag")]
    public bool isRun;
    public bool isAllowDefence;
    public bool isImmortal;
    public bool isStunned;
    public bool isCounterBack;
    private void Start()
    {
        Hp=Mathf.Clamp(Hp, 0, HpMax);
        
    }

    private void Update()
    {
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
        isCounterBack = am.ac.CheckAnimatorState("counterback");
}

    public void AddHP(float value)
    {
        {
            Hp += value;
            Hp = Mathf.Clamp(Hp, 0, HpMax);
        }
    }
    
}
