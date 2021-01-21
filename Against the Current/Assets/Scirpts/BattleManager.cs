using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defcol;
    private void Awake()
    { 
    }
    private void Start()
    {
        //设置防御碰撞体的基本属性
        defcol = transform.GetComponent<CapsuleCollider>();
        defcol.height = 2f;
        defcol.radius = 0.3f;
        //Vector3 tran = transform.position;
        defcol.center = new Vector3(0, 1, 0);
        defcol.isTrigger = true;
    }
    public float CheckAngle(GameObject launch,GameObject receive,Vector3 modelForward)
    {
        Vector3 ab = Vector3.Normalize(receive.transform.position -launch.transform.position);//终点减去起点等于AB向量,归一化为方向向量
        Vector3 model_forward = modelForward;//launch的模型正前方
        float angle = Vector3.Angle(model_forward, ab);//夹角
        angle = Mathf.Abs(angle);
        return angle;
    }
    private void OnTriggerEnter(Collider col)
    {
        WeaponControl wc;
        GameObject launch;//发出攻击检测的人
        //被武器打到
        if (col.tag == "weapon")
        {
            
            wc = col.transform.gameObject.GetComponentInParent<WeaponControl>();//未检查
            float angle=CheckAngle(wc.wm.gameObject,transform.gameObject, wc.wm.am.ac.model.transform.forward);
            if(angle<80)
            am.TryDoDamage(wc);
        }
    }
}
