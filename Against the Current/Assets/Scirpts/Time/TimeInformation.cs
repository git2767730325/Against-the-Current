using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInformation
{
    public Vector3 t_postion;
    public Quaternion t_rotion;
    public Quaternion t_modelRotion;
    public Rigidbody t_rig;
    public float Hp;//的血量
    public string actName;
    public int layerNum;
    public float forwardValue;
    public float rightValue;
    public Quaternion gunFixRot;
    //动画信息，动作帧，
    public TimeInformation(Vector3 _pos,Quaternion _rot, Quaternion _modelRot, Rigidbody _rig,float _Hp,string _actName,int _layer,float _forward,float _right,Quaternion _gunFixRot)
    {
        t_postion = _pos;
        t_rotion = _rot;
        t_modelRotion = _modelRot;
        t_rig = _rig;
        Hp = _Hp;
        actName = _actName;
        layerNum = _layer;
        forwardValue = _forward;
        rightValue = _right;
        gunFixRot = _gunFixRot;
    }
    public TimeInformation CloneInfo()
    {
        return MemberwiseClone() as TimeInformation;
    }
}
