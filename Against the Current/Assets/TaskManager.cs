using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Task> taskList = new List<Task>();
    public TextAsset tAsset;
    public Dialogue dialogue;
    public RewardItem rewardItem;
    private void Awake()
    {
        InitTaskList();
        //加载
    }

    private void Start()
    {
        if (GameManager.GM.tm == null)
            GameManager.GM.tm = this;
        UpdateTaskFinshState();
    }

    //初始化任务列表,加载时就好
    void InitTaskList()
    {
        string a = tAsset.text;
        string[] fs = a.Split('\n');
        for (int s = 0; s < fs.Length; s++)
        {
            Task task = new Task();
            task.taskID = s;
            task.taskName = fs[s];
            taskList.Add(task);
        }
    }
    //更新完成任务状况,在切换场景的时候更新一次就行
    void UpdateTaskFinshState()
    {
        foreach(var task in taskList)
        {
            if (task.haveFinished || task.isFinished)
            {
                Debug.Log("has");
                continue;
            }
            else
            {
                switch (task.taskID)
                {
                    case 0:
                        if (GameManager.GM.sealEnemyNum >= 5)
                        {
                            task.isFinished = true;
                        }
                        break;
                    case 1:
                        if (GameManager.GM.backStabEnemyNum >= 1)
                        { 
                        task.isFinished = true;
                        }
                        break;
                    case 2:
                        if (GameManager.GM.dunEnemyNum>= 1)
                        {
                            task.isFinished = true;
                        }
                        break;
                    case 3:
                        if (GameManager.GM.passLevel1)
                        {
                            task.isFinished = true;
                        }
                        break;
                    case 4:
                        if (GameManager.GM.passLevel2)
                            task.isFinished = true;
                        break;
                }
            }
        }
    }


    //如果满足任务条件，返回对应任务的ID，每次提交任务都要执行
    public string CheckTask()
    {
        foreach (var task in taskList)
        {
            if (!task.haveFinished)
                if (task.isFinished)
                {
                    rewardItem.itemNum = 1;
                    if (task.taskID < 2)
                    {
                        rewardItem.itemID = task.taskID + 2;
                        rewardItem.coinNum = 2000;
                    }
                    else if (task.taskID < 4)
                        rewardItem.itemID = task.taskID + 2;
                    else
                    {
                        rewardItem.itemID = -1;
                        rewardItem.coinNum = 20000;
                    }
                    rewardItem.canReward = true;
                    return task.taskName;
                }
        }
        return null;
    }
    //更新提交的任务，更改奖励情况
    public void UpdateTaskState(string s)
    {
        foreach (var task in taskList)
        {
            Debug.Log(s);
            if (task.taskName.Trim()==s.Trim())
            {
                Debug.Log(task.haveFinished);
                task.isFinished = false;
                task.haveFinished = true;
                rewardItem.canReward = false;
            }
        }
    }
}
