using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainstTheCurrent : MonoBehaviour
{
    public PlayerHandle playH;
    public ActorController ac;
    public Queue<GameObject> separationQue = new Queue<GameObject>();
    public GameObject[] objs = new GameObject[5];
    public List<TimeInformation> tInformation = new List<TimeInformation>();
    public List<TimePoint[]> timeAllList = new List<TimePoint[]>();
    public Rigidbody rig;
    public float Hp;//非必要？
    public int recordI=0;
    public int i = 0;//时间存储列表
    public int timeIndex = 0;//时间运行节点
    public int maxIndex = 500;//最大运行节点
    public int comeBackTime = 0;//可以倒流的时间点数
    public bool canRecord=true;
    public bool isCanBlade = false;
    [Header("=====  对象池镜像分身，预制体，拖动 ====")]
    public GameObject separationPrefab;
    [Header("==============    后处理   ==============")]
    public PostProcessing postProcessing;
    [Header("==============   末日誓言   =============")]
    public MyTimer canDoomOathTimer=new MyTimer();
    // Start is called before the first frame update
    private void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();
        ac = this.GetComponent<ActorController>();
    }
    void Start()
    {  

        FullPool();
        InitTimeInformations();
        Debug.Log("init ITI List finished");
        //postProcessing = GameObject.FindWithTag();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(separationQue.Count);
        if (timeIndex >= 500)
            OverTime();
        if (playH.Btn5.OnPressed)
        {
            StartRewind();
            canRecord = false;
        }
        else if(playH.Btn5.OnReleased)
        {
            StopRewind();
            canRecord = true;
        }
    }
    void FixedUpdate()
    {
        if (canDoomOathTimer.state == MyTimer.State.RUN)
        {
            canDoomOathTimer.Tick();
            if (playH.KeyD.LongPressing)
            {
                //长按
                DoomOath();
            }

        }
        if (timeIndex >= 500)
            OverTime();
        //返回对象池,每帧都返回，当时这么写怕不是傻了吧,疯了吧
        for(int id=0;id<5;id++)
        {
            if(objs[id]!=null)
            {
                if (!timeAllList[id][timeIndex].isHaveTimeBlock)
                {
                    ReturnPool(objs[id]);
                    objs[id] = null;
                }
            }
        }
        //镜像
        for(int id=0;id<5;id++)
        {
            //连续三帧播放同一个动画，明显不合理,所以多出判断
            if (recordI <= 1&&canRecord)
            {
                break;
            }
            if (timeAllList[id][timeIndex].isHaveTimeBlock)
            {
                if(objs[id]==null)
                objs[id] = Dequeue();
                Rigidbody _rig= objs[id].GetComponent<Rigidbody>();
                _rig = timeAllList[id][timeIndex].timeInformation.t_rig;
                objs[id].transform.position = timeAllList[id][timeIndex].timeInformation.t_postion;
                ActorController s_ac = objs[id].GetComponent<ActorController>();
                s_ac.transform.rotation = timeAllList[id][timeIndex].timeInformation.t_rotion;
                s_ac.model.transform.rotation = timeAllList[id][timeIndex].timeInformation.t_modelRotion;
                s_ac.SetFloat("forward",timeAllList[id][timeIndex].timeInformation.forwardValue);
                s_ac.SetFloat("right",timeAllList[id][timeIndex].timeInformation.rightValue);
                s_ac.gunLook.transform.rotation = timeAllList[id][timeIndex].timeInformation.gunFixRot;
                if (!canRecord)//回放的时候
                {
                    if (timeAllList[id][timeIndex].timeInformation.layerNum == 1)
                    {
                        s_ac.OnEnterGunWeapon();
                        if(!s_ac.am.sm.isGround)
                        s_ac.ChangeActAndPlayBack("Move", 0);

                    }
                    else
                    {
                        s_ac.OnExitGunWeapon();
                    }
                    s_ac.ChangeActAndPlayBack(timeAllList[id]
                        [timeIndex].timeInformation.actName, timeAllList[id][timeIndex].timeInformation.layerNum);
                }
                else
                {
                    if (timeAllList[id][timeIndex].timeInformation.layerNum == 1)
                    {
                        s_ac.OnEnterGunWeapon();
                        if (!s_ac.am.sm.isGround)
                        s_ac.StopPlayBackS("Move", 0);
                        Debug.Log("在层级1射击");
                    }
                    else
                    {
                        s_ac.OnExitGunWeapon();
                    }
                    s_ac.StopPlayBackS(timeAllList[id]
                       [timeIndex].timeInformation.actName, timeAllList[id][timeIndex].timeInformation.layerNum);
                }
            }
        }
        if(separationQue.Count>0)
        {
            if(isCanBlade)
            {
                isCanBlade = false;
                OverTime();
                canRecord = true;
            }
        }
        else if(separationQue.Count==0&&(isCanBlade||canRecord))
        {
            Type_Void();
            OverTime();
            isCanBlade = true;
        }
        if (canRecord)
        {
            Record();
        }
        else if(!isCanBlade)
        {
            if(timeIndex<=0)
            {
                StopRewind();
                return;
            }
            Rewind();
        }
    }

    private void LateUpdate()
    {
        //后处理效果恢复
        if (separationQue.Count < 5)
        {
            postProcessing.StartVague();
            postProcessing.StartRedCurtain();
            postProcessing.LensDistortionAdd(true);
            if (canRecord)
                AudioManager.SetAndPlayInterfaceAudio(2);
        }
        else
        {
            postProcessing.StopRedCurtain();
            postProcessing.StopVague();
            postProcessing.LensDistortionAdd(false);
        }
    }

    //
    public void Disable()
    {
        for(int isa=0;isa<5;isa++)
        {
            if(this.enabled==true)
                if(objs[isa]!=null)
                    if(objs[isa].activeSelf)
                        ReturnPool(objs[isa]);
        }
    }
    //初始化时间集
    public void InitTimeInformations()
    {
        for (int i = 0; i < 5; i++)
        {
            TimePoint[] tps = new TimePoint[500];
            for (int j = 0; j < 500; j++)//30S，每秒50帧或者记录17帧
            {
                tps[j] = new TimePoint(j);
            }
            timeAllList.Add(tps);
        }
    }


    public void Type_Void()
    {
        //在5个分身时
        //停止录制,清空录制
        //
        tInformation.Clear();
        canRecord = false;
        //开启技能计时
        canDoomOathTimer.StartTimer(8f);
    }


    //末日誓言,
    public void DoomOath()
    {
        if(canDoomOathTimer.state==MyTimer.State.RUN)
        {
            //如果可以就调用检查是否可以
            Debug.Log("末日誓言");
            canDoomOathTimer.state = MyTimer.State.FINISHED;
            ac.LongSword();
        }
    }

    //超出录制时间，所有队列信息的开始点，所有信息结束点减少1，移除第一个点
    public void OverTime()
    {
        if(timeIndex>=maxIndex)
        {
            for(int indexa=0;indexa<5;indexa++)
            {
                for(int j=0;j<499;j++)
                {

                    //BUG使我当场去世，写得我二次去世

                    timeAllList[indexa][j].isHaveTimeBlock= timeAllList[indexa][j + 1].isHaveTimeBlock;
                    //timeAllList[indexa][j] = timeAllList[indexa][j + 1];
                    //timeAllList[indexa][j].timeInformation.actName= timeAllList[indexa][j + 1].timeInformation.actName;
                    //timeAllList[indexa][j].timeInformation.layerNum= timeAllList[indexa][j + 1].timeInformation.layerNum;
                    //timeAllList[indexa][j].timeInformation.t_postion= timeAllList[indexa][j + 1].timeInformation.t_postion;
                    //timeAllList[indexa][j].timeInformation.t_rotion= timeAllList[indexa][j + 1].timeInformation.t_rotion;
                    //timeAllList[indexa][j].timeInformation.t_modelRotion= timeAllList[indexa][j + 1].timeInformation.t_modelRotion;
                    //timeAllList[indexa][j].timeInformation.forwardValue= timeAllList[indexa][j + 1].timeInformation.forwardValue;
                    timeAllList[indexa][j].timeInformation = timeAllList[indexa][j + 1].timeInformation.CloneInfo();

                    //应该全部写下来
                    if (timeAllList[indexa][j].isHaveTimeBlock)
                    {
                        timeAllList[indexa][j].startPoint -= 1;
                        timeAllList[indexa][j].stopPoint -= 1;
                        if (timeAllList[indexa][j].startPoint <= 0)
                            timeAllList[indexa][j].startPoint = 0;
                        if (timeAllList[indexa][j].stopPoint <= 0)
                            timeAllList[indexa][j].stopPoint = 0;
                    }
                    else
                    {
                        timeAllList[indexa][j].startPoint =0;
                        timeAllList[indexa][j].stopPoint =0;
                    }
                }
                    timeAllList[indexa][499].isHaveTimeBlock = false;
            }
            timeIndex -= 1;
        }
        else
        {
            int remain =500-timeIndex;
            if(isCanBlade)
            {
                timeIndex += 1;
                Debug.Log("aa"+timeIndex);
                return;
            }
            for (int indexa = 0; indexa < 5; indexa++)
            {
                for (int j = 0; j < remain; j++)
                {
                    //timeAllList[indexa][j] = timeAllList[indexa][j+timeIndex];
                    timeAllList[indexa][j].Copy(timeAllList[indexa][j + timeIndex]);
                    timeAllList[indexa][j + timeIndex].isHaveTimeBlock = false;
                    if (timeAllList[indexa][j].isHaveTimeBlock)
                    {
                        timeAllList[indexa][j].startPoint -= timeIndex;
                        timeAllList[indexa][j].stopPoint -= timeIndex;
                        if (timeAllList[indexa][j].startPoint <= 0)
                            timeAllList[indexa][j].startPoint = 0;
                        if (timeAllList[indexa][j].stopPoint <= 0)
                            timeAllList[indexa][j].stopPoint = 0;
                    }
                }
            }
            Debug.Log("guozai");
            timeIndex = 0;
        }
    }
    public void StartRewind()
    {
        AudioManager.SetAndPlayInterfaceAudio(0);
        canRecord = false;
        bool haveEmptyPos = false;
        //录制的起始点t,timeIndex是结束点
        int t=timeIndex-tInformation.Count;
        Debug.Log("开始帧"+t+"  录制了:"+tInformation.Count+"帧");
        //Debug.Log(t+"aa"+tInformation.Count);
        for (i = 0; i < 5; i++)
        {
            if ((!timeAllList[i][t].isHaveTimeBlock )&&( !timeAllList[i][timeIndex].isHaveTimeBlock))
            {
                if (t+45<500&&!timeAllList[i][t + 45].isHaveTimeBlock)
                {
                    haveEmptyPos = true;
                    break;
                }
            }
        }
        //如果没有空位，先覆盖
        if(haveEmptyPos==false)
        {
            i = 1;
            Debug.Log("覆盖1的录制条");
        }
        if (timeAllList[i][t].isHaveTimeBlock)
            {
                //如果点的前方有录制过,删掉之前录制的所有信息
                int j = timeAllList[i][t].startPoint;
                int k = timeAllList[i][t].stopPoint;
                for (int l = 0; l < (k - j); l++)
                {
                    timeAllList[i][j + l].isHaveTimeBlock = false;
                }
            }
        if (timeAllList[i][timeIndex].isHaveTimeBlock)
            {

            //如果点的后有录制过,删掉之后连续录制的所有信息
            int j = timeAllList[i][timeIndex].startPoint;
                int k = timeAllList[i][timeIndex].stopPoint;
                for (int l = 0; l < (k - j); l++)
                {
                    timeAllList[i][j + l].isHaveTimeBlock = false;
                }
            }
        //将信息保存
        for(int l=0;l<tInformation.Count;l++)
        {
            timeAllList[i][timeIndex- l].timeInformation = tInformation[l];
            timeAllList[i][timeIndex-l].isHaveTimeBlock = true;
            timeAllList[i][timeIndex-l].stopPoint = timeIndex;
            timeAllList[i][timeIndex-l].startPoint = t;
        }
        tInformation.Clear();
            //isAgainstTheCurrent = true;
            //将当前记录时间信息的列表复制到序号列表中

        //停止移动
    }
    public void StopRewind()
    {
        canRecord = true;
        //isAgainstTheCurrent = false;
        //恢复移动
        AudioManager.StopInterfaceSource();
    }
    public void Rewind()
    {
        timeIndex -= 1;
    }

    public void Record()
    {
        recordI++;
        if(recordI<=2)
        {
            return;
        }
        recordI = 0;
        //if(timeIndex>=499),错误
        if(tInformation.Count>=499)
        {
            Debug.Log("清除了超时的信息");
            tInformation.RemoveAt(498);
        }
        tInformation.Insert(0,new TimeInformation(transform.position, Quaternion.identity,
            Quaternion.Euler(ac.modelRot),rig, 999,ac.actPlaying,ac.actLayer,ac.forwardValue,ac.rightValue, Quaternion.Euler(ac.gunFixPos)));
        timeIndex += 1;
        comeBackTime += 1;
        //先存储到快，后将存储的信息放入时间片中
        //timeAllList[i][timeIndex].timeInformation = tInformation[0];

        //if (timeAllList[i][timeIndex].isHaveTimeBlock)
        //    timeAllList[i][timeIndex].startPoint=
        //timeAllList[i][timeIndex].startPoint = timeIndex;
    }

    //填充对象池，最大5个分身
    public void FullPool()
    {
        for (int i = 0; separationQue.Count < 5; i++)
        {
           // GameObject prefab = Instantiate(separationPrefab);
            //prefab.transform.position = transform.position;
            //prefab.transform.rotation = Quaternion.identity;
            ReturnPool(Instantiate(separationPrefab));
        }
    }
    //返回对象池
    public void ReturnPool(GameObject g)
    {
        g.SetActive(false);
        separationQue.Enqueue(g);
    }
    //调用对象池
    public GameObject Dequeue()
    {
        if(separationQue.Count<1)
        {
            Debug.Log("none");
            return null;
        }
        GameObject g= separationQue.Dequeue();
        g.SetActive(true);
        return g;
    }
}
