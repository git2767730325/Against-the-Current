using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_Enemy : MonoBehaviour
{
    public TimeComeBack tcb;
    public AgainstTheCurrent atc_Player;
    //NavmeshAgent本身有很多的东西，不用自己写
    public ActorController ac;
    public NavMeshAgent agent;
    public float walkSpeed;
    public Transform[] targetPoint;
    public int i;
    MyTimer stayTimer = new MyTimer();
    //视野玩家位置
    public Enemy_SIght enemy_Sight;
    public GameObject player;
    public GameObject separation;
    private StateManager playerS;
    MyTimer lookTimer = new MyTimer();
    [Header("死亡后可封印，不可背刺")]
    public GameObject stab;
    public GameObject seal;
    [Header("协程相关")]
    //是否可以开启
    public bool isCanOpenIE = true;
    [Header("停顿计时")]
    public MyTimer idleTimer = new MyTimer();
    public enum EnemyKind
    {
        Enemy1,
        Enemy2,
        Boss1,
        Boss2,
        Boss3,
    }
    public EnemyKind enemyActiveMode;

    private void Awake()
    {
        tcb = GetComponent<TimeComeBack>();
        //如果是小怪才要巡逻
        if(enemyActiveMode==EnemyKind.Enemy1|| enemyActiveMode == EnemyKind.Enemy2)
        if(targetPoint.Length==0)
            targetPoint = GameObject.FindWithTag("partol").transform.GetComponentsInChildren<Transform>();
        //要锁定的玩家
        player = GameObject.FindWithTag("Player");
        i = targetPoint.Length;
        agent.isStopped = false;
        agent.speed = walkSpeed;
        playerS = player.GetComponent<StateManager>();
        atc_Player = player.GetComponent<AgainstTheCurrent>();
    }
    void Update()
    {
        if (tcb != null && tcb.isRewind)
            return;
        if (ac.CheckAnimatorState("die"))
        {
            if (seal != null && stab != null)
                if (!seal.activeSelf)
                {
                    seal.SetActive(true);
                    stab.SetActive(false);
                }
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                agent.speed = walkSpeed;
            }
            return;
        }//倒地后返回
        else
        {
            if(seal!=null)
            if (seal.activeSelf)
            {
                seal.SetActive(false);
                    if(stab!=null)
                        stab.SetActive(true);
            }
        }
        if(ac.CheckAnimatorState("hit"))
        {
            lookTimer.StartTimer(3f);
        }
        if(enemy_Sight.isInSight)
        {
            if(playerS.isDie)
            {
                agent.isStopped = false;
                Patrol();//巡逻
                return;
            }
            //看向玩家或者分身
            LookTarget();
            //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(Vector3.Normalize(player.transform.position-transform.position)),1f);
            //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(player.transform.position),30f);
            if (enemyActiveMode== EnemyKind.Enemy1)
            {
                AIAttack();
                AIJumb();
                AIWalk(2.01f,90f);
                AIDefence();
            }
            else if(enemyActiveMode == EnemyKind.Enemy2)
            {
                AIAttack();
                AIWalk();
            }
            else if (enemyActiveMode == EnemyKind.Boss1)
            {
                //如果距离过大，看血量决定是否追
                if (Vector3.Distance(this.transform.position, player.transform.position) > 50)
                {
                    if (ac.am.sm.Hp < ac.am.sm.HpMax * 0.95f)
                    {
                        AIWalk(2f, 200f);
                        AIDefence(10f, 300f);
                    }
                    //什么都不做
                }
                else
                {
                    AIAttack();
                    AIWalk(2.5f, 20f);
                    AIDefence(6f, 18f);
                    //协程随机动作
                    if (isCanOpenIE)
                    {
                        StartCoroutine("RandowBehavior");
                        isCanOpenIE = false;
                    }
                }
            }
            else if (enemyActiveMode == EnemyKind.Boss2)
            {
                //如果距离过大，看血量决定是否追
                if (Vector3.Distance(this.transform.position, player.transform.position) > 50)
                {
                    if (ac.am.sm.Hp < ac.am.sm.HpMax * 0.95f)
                    {
                        AIWalk(2f, 200f);
                        AIDefence(10f, 300f);
                    }
                    //什么都不做
                }
                else
                {
                    idleTimer.Tick();
                    if (idleTimer.state != MyTimer.State.RUN)
                    {
                        AIAttack();
                        AIWalk(2.5f, 20f);
                    if (isCanOpenIE)
                    {
                        StartCoroutine("RandowBehavior");
                        isCanOpenIE = false;
                    }
                    }
                    AIDefence(6f, 18f);
                    //协程随机动作
                }
            }
            lookTimer.StartTimer(5f);
            agent.speed = 1.3f;
        }
        //超出距离，看着玩家
        else if(lookTimer.state==MyTimer.State.RUN)
        {
            //Vector3.Slerp(transform.forward, Vector3.Normalize(player.transform.position - transform.position), 0.03f);
            //ac.model.transform.forward = transform.forward;
            LookTarget();
            //设置目标点
            SetDestination();
            agent.speed = 0.2f;
            ac.SetFloat("forward", 0.4f);
            lookTimer.Tick();
        }
        else
        {
            stayTimer.Tick();
            Patrol();//巡逻
        }
    }


    //设置移动目标点,玩家或者分身
    void SetDestination()
    {
        if(IsTargetToSeparation())
        {
            agent.destination = separation.transform.position;
        }
        else
        {
            agent.destination = player.transform.position;
        }
    }

    //看向玩家或者分身
    void LookTarget()
    {
        if (IsTargetToSeparation())
        {
            Vector3.Slerp(transform.forward, Vector3.Normalize(separation.transform.position - transform.position), 0.03f);
            ac.model.transform.forward = transform.forward;
        }
        else
        {
            Vector3.Slerp(transform.forward, Vector3.Normalize(player.transform.position - transform.position), 0.03f);
            ac.model.transform.forward = transform.forward;
        }
    }


    //追玩家还是分身
    bool IsTargetToSeparation()
    {
        if (atc_Player.separationQue.Count < 5)
        {
            if (separation == null)
            {
                separation = GameObject.FindWithTag("Separation");
                return true;
            }
            else if (!separation.activeSelf)
            {
                separation = GameObject.FindWithTag("Separation");
                return true;
            }
            else
            {
                return true;
            }
        }
        else if(separation!=null)
                separation = null;
            return false;
    }

    //巡逻,如果是BOSS则返回
    void Patrol()
    {
        if (enemyActiveMode == EnemyKind.Enemy1 || enemyActiveMode == EnemyKind.Enemy2)
        {
            //agent.isStopped = false;
            agent.speed = walkSpeed;
            //判断距离是否接近了巡逻点，若果接近
            if (agent.remainingDistance <= (agent.stoppingDistance + 0.5f) && agent.isStopped == false)
            {
                stayTimer.StartTimer(2);
                ac.SetFloat("forward", 0f);
                agent.isStopped = true;
                i++;
            }
            else
            {
                if (stayTimer.state != MyTimer.State.RUN)
                    ac.SetFloat("forward", 1f);
            }
            if (stayTimer.state == MyTimer.State.FINISHED)
            {
                agent.isStopped = false;
            }
            if (i >= targetPoint.Length)
                i = 0;
            agent.destination = targetPoint[i].position;//移动的目标点设置
        }
    }
    void AIDefence(float min = 3f, float max = 6f)
    {
        if(DistanceIn(min,max))
        {
            ac.SetLayerWeight("defence", 1.0f);
            ac.anim.SetBool("defense",true);
        }
        else
        {
            ac.SetLayerWeight("defence", 0f);
            ac.anim.SetBool("defense", false);
        }
    }
    //特别一点，攻击,模型全程面向目标
    void AIWalk(float min = 2.01f, float max = 6f)
    {
        if (DistanceIn(min, max))
        {
            SetDestination();
            agent.isStopped = false;
            //agent.speed = walkSpeed;
            ac.SetFloat("forward", 1f);
        }
    }
    void AIAttack(float min = 0f, float max = 2f)
    {
        if (DistanceIn(min,max))
        {
            ac.SetTrigger("attack");
            agent.isStopped = true;
            if(IsTargetToSeparation())
            ac.transform.LookAt(separation.transform.position);
            else
            ac.transform.LookAt(player.transform.position);
        }
    }
    void AIJumb(float min=2.5f,float max=3f)
    {
        if (DistanceIn(min,max))
            ac.SetTrigger("jump");
    }
    //判断距离是否在一定范围内
    bool DistanceIn(float min,float max)
    {
        //如果分身不存在
        if (separation == null)
        {
            if ((Vector3.Distance(player.transform.position, transform.position) < max) && (Vector3.Distance(player.transform.position, transform.position) > min))
                return true;
        }
        else
        {
            if ((Vector3.Distance(separation.transform.position, transform.position) < max) && (Vector3.Distance(separation.transform.position, transform.position) > min))
                return true;
        }
        return false;
    }

    //随机行为，协程
    IEnumerator RandowBehavior()
    {
        int i = Random.Range(0, 100);
        if (i > 50)
        {
            AIJumb(0f, 10f);
            
        }
        else if (i > 20)
            AIDefence();
        else if (i<=20)
        {
            StartCoroutine("AddHpToBoss");
            idleTimer.StartTimer((float)(i)/5f+2f);
            Debug.Log("协程开启了"+ ((float)(i) / 5f + 2f));
        }
        yield return new WaitForSeconds(3f);
        isCanOpenIE = true;
    }
    IEnumerator AddHpToBoss()
    {
        ac.SetTrigger("flip");
        yield return null;
    }

}
