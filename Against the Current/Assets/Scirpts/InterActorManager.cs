using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActorManager : IActorManagerInterface
{
    private CapsuleCollider capCol;
    //ECM包括 指向对应物体的AM属性
    public List<EventCasterManager> ecmList=new List<EventCasterManager>();
    private void Awake()
    {
        capCol = GetComponent<CapsuleCollider>();
    }
    public void ListUpdate()
    {
        ecmList.Remove(ecmList[0]);
    }
    private void OnTriggerEnter(Collider col)
    { 
        EventCasterManager[] ecms=col.gameObject.GetComponents<EventCasterManager>();//抓取事件管理
        foreach (var ecm  in ecms)
        {
            if (!ecmList.Contains(ecm))
            {
                ecmList.Add(ecm);
                if(ecm.eventName!="pickup")
                {
                    ecm.active = true;
                }
            }
            //如果对话，UIM的物体设置为此
            if(ecm.eventName=="talk")
            {
                ecm.GetComponentInChildren<Dialogue>().SetDia();//应该不是很好的做法
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        EventCasterManager[] ecms = col.gameObject.GetComponents<EventCasterManager>();//抓取事件管理
        foreach (var ecm in ecms)
        {
            if (ecmList.Contains(ecm))
                ecmList.Remove(ecm);
            if (ecm.eventName == "talk")
            {
                ecm.GetComponentInChildren<Dialogue>().CancelDia();//应该不是很好的做法
            }
        }
    }
}
