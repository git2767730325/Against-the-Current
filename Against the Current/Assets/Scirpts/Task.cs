using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public int taskID;
    public string taskName;
    public bool isFinished = false;
    public bool haveFinished = false;

    public Task CloneTask()
    {
        return MemberwiseClone() as Task;
    }
}
