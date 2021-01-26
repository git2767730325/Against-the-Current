using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActorManager : IActorManagerInterface
{
    private CapsuleCollider capCol;
    public List<EventCasterManager> ecmList=new List<EventCasterManager>();
    // Start is called before the first frame update
    private void Awake()
    {
        capCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider col)
    {
        
        EventCasterManager[] ecms=col.gameObject.GetComponents<EventCasterManager>();//抓取事件管理
        foreach (var ecm  in ecms)
        {
            if (!ecmList.Contains(ecm))
                ecmList.Add(ecm);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        EventCasterManager[] ecms = col.gameObject.GetComponents<EventCasterManager>();//抓取事件管理
        foreach (var ecm in ecms)
        {
            if (ecmList.Contains(ecm))
                ecmList.Remove(ecm);
        }
    }
}
