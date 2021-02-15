using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePoint
{
    public int pointIndex = 0;
    public bool isHaveTimeBlock=false;
    public TimeInformation timeInformation = new TimeInformation(
        Vector3.zero,new Quaternion(),new Rigidbody(),0);
    public int startPoint=0;
    public int stopPoint=0;
    public TimePoint(int _pointIndex)
    {
        pointIndex = _pointIndex;
    }
    public void Copy(TimePoint tp)
    {
        isHaveTimeBlock = tp.isHaveTimeBlock;
        timeInformation = tp.timeInformation;
        startPoint = tp.startPoint;
        stopPoint = tp.stopPoint;
    }
}
