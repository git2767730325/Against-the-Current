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
    public PlayerHandle playH;
    public StateManager sm;
    [Header("======   时间状态=========")]
    public bool isRewind = false;
    //
    void Awake()
    {

        rig = this.GetComponent<Rigidbody>();
    }
    void Start()
    {
        currentHp = 50f;
        //设置逆流技能
        if(gameObject.name=="Player")
        {
            timeLength = 2f;//回到两秒前的状态
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHp = sm.Hp;
        if (gameObject.name == "Player")
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
                sm.Hp = currentHp;
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
            if(gameObject.name!="Player")
            Rewind();
        }
        else
        {
            Record();
        }
    }
    //各方法
    public void StartRewinding()
    {
        rig.isKinematic = true;//不受力
        Time.timeScale = 3f;//加速倒流
        isRewind = true;
            //UI设置
    }

    public void StopRewinding()
    {
        rig.isKinematic = false;//恢复受力
        Time.timeScale = 1f;//恢复时间流速
        isRewind = false;
        cdTimer.StartTimer(backInTimeCD);
    }

    public void Record()//录制
    {
        //最大录制数取整数，检查决定是否删掉，覆盖录制
        if (timeList.Count > Mathf.Round(canBackTime / Time.fixedDeltaTime))
        {
            timeList.RemoveAt(timeList.Count - 1);
        }
        timeList.Insert(0,new TimeInformation(transform.position,Quaternion.identity,rig,
            currentHp));//将新录制的插入列表头
        canBackTime += Time.fixedDeltaTime;
        
    }

    public void Rewind()
    {
        if(timeList.Count<1||canBackTime<Time.fixedDeltaTime)
        {
            StopRewinding();
            return;
        }
        Debug.Log("back"+timeList.Count);
        TimeInformation t_information =timeList[0];//取出信息
        transform.position = t_information.t_postion;
        transform.rotation = t_information.t_rotion;//欧拉角，不是旋转量
        rig = t_information.t_rig;
        currentHp = t_information.Hp;
        timeList.RemoveAt(0);//使用后移除
        canBackTime -= Time.fixedDeltaTime;
    }

}
