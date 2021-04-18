using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeComeBack : MonoBehaviour
{
    //列表存储过去信息
    public List<TimeInformation> timeList=new List<TimeInformation>();
    public float timeLength=30f;//倒流时间的最大长度
    public float canBackTime=0f;//现在可倒流时间长度
    public float usedTime;//已经使用的时间
    public float backInTimeCD = 2.5f;//冷却时间
    public MyTimer cdTimer=new MyTimer();//计时器
    public float currentHp;
    private Rigidbody rig;
    private int recordtimeI=0;
    [Header("== 由玩家控制是否时光倒流，固定是玩家（拖动） ==")]
    public PlayerHandle playH;
    public ActorController ac;
    public AI_Enemy ai;
    [Header("对应物体的状态，拖动")]
    public StateManager sm;
    [Header("======   时间状态=========")]
    public bool isRewind = false;
    //
    void Awake()
    {
        rig = this.GetComponent<Rigidbody>();
        ac = this.GetComponent<ActorController>();
        TryGetComponent<AI_Enemy>(out ai);
    }
    void Start()
    {
        currentHp = 50f;
        //设置逆流技能
        if(gameObject.tag=="Player")
        {
            timeLength = 2f;//回到两秒前的状态
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRewind)
            currentHp = sm.Hp;
        else if(tag!="Player")
            sm.Hp = currentHp;
        if (gameObject.tag == "Player")
        {
            if(playH.Btn6.OnPressed&& (cdTimer.state != MyTimer.State.RUN))
            {
                //如果不是死亡状态等
                cdTimer.StartTimer(backInTimeCD);
                TimeInformation t_information = timeList[timeList.Count-1];//取出信息
                transform.position = t_information.t_postion;
                transform.rotation = t_information.t_rotion;//欧拉角，不是旋转量
                rig = t_information.t_rig;
                currentHp = t_information.Hp;
                if(sm.Hp<=currentHp)
                sm.Hp = currentHp;
                AudioManager.SetAndPlayInterfaceAudio(1);
                timeList.Clear();//使用后移除
                canBackTime =0f;
            }
            if (canBackTime >= timeLength)
            {
                canBackTime = timeLength;
            }
            return;
        }
        if (playH.Btn5.OnPressed && (cdTimer.state != MyTimer.State.RUN))
        {
            StartRewinding();
        }
        else if(playH.Btn5.OnReleased&&isRewind)
        {
            StopRewinding();
        }
        //最大录制长度
        if(canBackTime>=timeLength)
        {
            canBackTime = timeLength;
        }
    }

    void FixedUpdate()
    {
        
        cdTimer.Tick();
        if (isRewind)
        {
            if(gameObject.tag!="Player")
            Rewind();
        }
        else
        {
            Record();
        }
    }
    //各方法

    private void OnEnable()
    {
        timeList.Clear();
        canBackTime = 0f;
        cdTimer.StartTimer(backInTimeCD);
    }

    public void StartRewinding()
    {
        if (ai != null)
            ai.agent.isStopped = true;
        rig.isKinematic = true;//不受力
        //Time.timeScale = 1f;//加速倒流
        isRewind = true;
            //UI设置
    }

    public void StopRewinding()
    {
        if (ai != null)
            ai.agent.isStopped = false;
        rig.isKinematic = false;//恢复受力
        //Time.timeScale = 1f;//恢复时间流速
        isRewind = false;
        cdTimer.StartTimer(backInTimeCD);
        ac.StopPlayBack();
    }

    public void Record()//录制
    {
        recordtimeI += 1;
        if(recordtimeI<=2)
        {
            return;
        }
        recordtimeI = 0;
        //最大录制数取整数，检查决定是否删掉，覆盖录制
        if (timeList.Count > Mathf.Round(canBackTime /(3*Time.fixedDeltaTime)))
        {
            timeList.RemoveAt(timeList.Count - 1);
        }
        timeList.Insert(0,new TimeInformation(transform.position,Quaternion.identity,Quaternion.Euler(ac.modelRot),rig,
            currentHp,ac.actPlaying,ac.actLayer,ac.forwardValue,ac.rightValue,Quaternion.identity));//将新录制的插入列表头
        canBackTime +=3*Time.fixedDeltaTime;
        
    }

    public void Rewind()
    {
        //不能完全保证停止，也不能恢复原来是否停止的状态
        if(ai!=null)
        Debug.Log(ai.agent.isStopped);
        if(timeList.Count<1||canBackTime<3*Time.fixedDeltaTime)
        {
            StopRewinding();
            return;
        }
        //Debug.Log("back"+timeList.Count);
        TimeInformation t_information =timeList[0];//取出信息
        transform.position = t_information.t_postion;
        transform.rotation = t_information.t_rotion;//欧拉角，不是旋转量
        ac.model.transform.rotation = t_information.t_modelRotion;
        ac.SetFloat("forward",t_information.forwardValue);
        rig = t_information.t_rig;
        currentHp = t_information.Hp;
        if(gameObject.tag != "Player" && gameObject.tag != "Separation")
        {
            ac.ChangeActAndPlayBack(t_information.actName,t_information.layerNum);
        }
        timeList.RemoveAt(0);//使用后移除
        canBackTime -= 3*Time.fixedDeltaTime;
    }
}
