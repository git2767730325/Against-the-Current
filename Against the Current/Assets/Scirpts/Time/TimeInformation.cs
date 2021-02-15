using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInformation
{
    public Vector3 t_postion;
    public Quaternion t_rotion;
    public Rigidbody t_rig;
    public float Hp;//怪物的血量
    //动画信息，动作帧，
    public TimeInformation(Vector3 _pos,Quaternion _rot,Rigidbody _rig,float _Hp)
    {
        t_postion = _pos;
        t_rotion = _rot;
        t_rig = _rig;
        Hp = _Hp;
    }

}
