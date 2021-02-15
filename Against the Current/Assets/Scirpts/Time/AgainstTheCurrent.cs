using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainstTheCurrent : MonoBehaviour
{
    public PlayerHandle playH;
    public Queue<GameObject> separationQue = new Queue<GameObject>();
    public GameObject[] objs = new GameObject[5];
    public List<TimeInformation> tInformation = new List<TimeInformation>();
    public List<TimePoint[]> timeAllList = new List<TimePoint[]>();
    public Rigidbody rig;
    public float Hp;
    public int i = 0;//时间存储列表
    public int timeIndex = 0;//时间运行节点
    public int maxIndex = 1500;//最大运行节点
    public int comeBackTime = 0;//可以倒流的时间点数
    public GameObject separationPrefab;
    public bool canRecord=true;
    public bool isCanBlade = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();
    }
    void Start()
    {
        FullPool();
        InitTimeInformations();
        Debug.Log("init ITI List finished");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIndex >= 1500)
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
        if (timeIndex >= 1500)
            OverTime();
        //返回对象池
        for(int id=0;id<5;id++)
        {
            if(objs[id]!=null)
            {
                ReturnPool(objs[id]);
                objs[id] = null;
            }
        }
        //镜像
        for(int id=0;id<5;id++)
        {
            if(timeAllList[id][timeIndex].isHaveTimeBlock)
            {
                objs[id] = Dequeue();
                Rigidbody _rig= objs[id].GetComponent<Rigidbody>();
                _rig = timeAllList[id][timeIndex].timeInformation.t_rig;
                objs[id].transform.position = timeAllList[id][timeIndex].timeInformation.t_postion;
                objs[id].transform.rotation = timeAllList[id][timeIndex].timeInformation.t_rotion;
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
    //初始化时间集
    public void InitTimeInformations()
    {
        for (int i = 0; i < 5; i++)
        {
            TimePoint[] tps = new TimePoint[1500];
            for (int j = 0; j < 1500; j++)//30S，每秒50帧或者记录17帧
            {
                tps[j] = new TimePoint(j);
            }
            timeAllList.Add(tps);
        }
    }


    public void Type_Void()
    {
        //停止录制,清空录制
        //
        tInformation.Clear();
        canRecord = false;
    }


    //末日誓言
    public void DoomOath()
    {

    }

    //超出录制时间，所有队列信息的开始点，所有信息结束点减少1，移除第一个点
    public void OverTime()
    {
        if(timeIndex>=maxIndex)
        {
            for(int indexa=0;indexa<5;indexa++)
            {
                for(int j=0;j<1499;j++)
                {
                    timeAllList[indexa][j] = timeAllList[indexa][j + 1];
                    if (timeAllList[indexa][j].isHaveTimeBlock)
                    {
                        timeAllList[indexa][j].startPoint -= 1;
                        timeAllList[indexa][j].stopPoint -= 1;
                        if (timeAllList[indexa][j].startPoint <= 0)
                            timeAllList[indexa][j].startPoint = 0;
                        if (timeAllList[indexa][j].stopPoint <= 0)
                            timeAllList[indexa][j].stopPoint = 0;
                    }
                }
                timeAllList[indexa][1499].isHaveTimeBlock = false;
            }
            timeIndex -= 1;
        }
        else
        {
            int remain =1500-timeIndex;
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
        canRecord = false;
        bool haveEmptyPos = false;
        //录制的起始点t,timeIndex是结束点
        int t=timeIndex-tInformation.Count;
        for (i = 0; i < 5; i++)
        {
            if ((!timeAllList[i][t].isHaveTimeBlock )&&( !timeAllList[i][timeIndex].isHaveTimeBlock))
            {
                if (t+125<1500&&!timeAllList[i][t + 125].isHaveTimeBlock)
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

    }
    public void Rewind()
    {
        timeIndex -= 1;
    }

    public void Record()
    {
        if(timeIndex>=1500)
        {
            tInformation.RemoveAt(tInformation.Count-1);
        }
        tInformation.Insert(0,new TimeInformation(transform.position, Quaternion.identity,
            rig, Hp));
        //先存储到快，后将存储的信息放入时间片中
        //timeAllList[i][timeIndex].timeInformation = tInformation[0];

        //if (timeAllList[i][timeIndex].isHaveTimeBlock)
        //    timeAllList[i][timeIndex].startPoint=
        //timeAllList[i][timeIndex].startPoint = timeIndex;
        timeIndex += 1;
        comeBackTime += 1;
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
