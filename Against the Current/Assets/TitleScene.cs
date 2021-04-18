using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //只负责开启线程，连接
        Client.StartThread();
        //同步装备数据
        JsonData jd = new JsonData();
        jd["function"] = 6;//协议：重登陆更新数据
        GameManager.SendMessages(jd);
    }

    void OnDestroy()
    {
        JsonData jd = new JsonData();
        jd["function"] = 9;
        jd["account"] = GameManager.apJD["account"];
        jd["passwords"] = GameManager.apJD["passwords"];
        GameManager.SendMessages(jd);
        Client.CloseThread();
    }
}
