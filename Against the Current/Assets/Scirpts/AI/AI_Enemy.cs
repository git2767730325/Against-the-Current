using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_Enemy : MonoBehaviour
{
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
    MyTimer lookTimer = new MyTimer();

    private void Awake()
    {
        targetPoint = GameObject.FindWithTag("partol").transform.GetComponentsInChildren<Transform>();
        player = GameObject.FindWithTag("Player");
        i = targetPoint.Length;
        agent.isStopped = false;
        agent.speed = walkSpeed;
    }
    void Update()
    {
        if(ac.CheckAnimatorState("die"))
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                agent.speed = walkSpeed;
            }
            return;
        }//倒地后返回
        if(enemy_Sight.isInSight)
        {
            Vector3.Slerp(transform.forward,Vector3.Normalize(player.transform.position-transform.position),0.03f);
            ac.model.transform.forward = transform.forward;
            //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(Vector3.Normalize(player.transform.position-transform.position)),1f);
            //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(player.transform.position),30f);
            AIAttack();
            AIJumb();
            AIWalk();
            AIDefence();
            lookTimer.StartTimer(5f);
            agent.speed = 1.3f;
        }
        //超出距离，看着玩家
        else if(lookTimer.state==MyTimer.State.RUN)
        {
            Vector3.Slerp(transform.forward, Vector3.Normalize(player.transform.position - transform.position), 0.03f);
            ac.model.transform.forward = transform.forward;
            agent.destination = player.transform.position;
            agent.speed = 0.1f;
            ac.SetFloat("forward", 0.4f);
            lookTimer.Tick();
            
        }
        else
        {
            stayTimer.Tick();
            Patrol();//巡逻
        }
    }
    //巡逻
    void Patrol()
    {
        //agent.isStopped = false;
        agent.speed = walkSpeed;
        //判断距离是否接近了巡逻点，若果接近
        if(agent.remainingDistance<=(agent.stoppingDistance+0.5f)&&agent.isStopped==false)
        {
            stayTimer.StartTimer(2);
            ac.SetFloat("forward", 0f);
            agent.isStopped=true;
            i++;
        }
        else
        {
            if(stayTimer.state!=MyTimer.State.RUN)
            ac.SetFloat("forward", 1f);
        }
        if(stayTimer.state==MyTimer.State.FINISHED)
        {
            agent.isStopped = false;
        }
        if (i >=targetPoint.Length)
            i = 0;
        agent.destination = targetPoint[i].position;//移动的目标点设置
    }
    void AIDefence()
    {
        if(DistanceIn(3,6))
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
    void AIWalk()
    {
        if (DistanceIn(2.01f, 6))
        {
            agent.destination = player.transform.position;
            agent.isStopped = false;
            //agent.speed = walkSpeed;
            ac.SetFloat("forward", 1f);
        }
    }
    void AIAttack()
    {
        if (DistanceIn(0, 2))
        {
            ac.SetTrigger("attack");
            agent.isStopped = true;
            ac.transform.LookAt(player.transform.position);
        }
    }
    void AIJumb()
    {
        if (DistanceIn(2.5f,3))
            ac.SetTrigger("jump");
    }
    //判断距离是否在一定范围内
    bool DistanceIn(float min,float max)
    {
        if ((Vector3.Distance(player.transform.position, transform.position) < max)&&( Vector3.Distance(player.transform.position, transform.position)>min))
            return true;
        return false;
    }
}
