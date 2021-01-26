using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class EquipScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //只负责开启线程，连接
        Client.StartThread();
        //同步装备数据
        JsonData jd = new JsonData();
        jd["function"] = 8;//协议：重登陆更新数据
        GameManager.SendMessages(jd);
    }

    //暂时弃用了
    public void AddItem()
    {

    }

    public void DelItem()
    {

    }

    public void SellItem()
    {

    }

    //如果收到消息调用此函数
    public void ItemUpdate(JsonData _jd)
    {
        //物品类型，ID，数量，位置等

        //金币多少

    }

    void OnDestroy()
    {
        JsonData jd = new JsonData();
        jd["function"] = 9;
        jd["account"] = GameManager.apJD["account"];
        GameManager.SendMessages(jd);
        Client.CloseThread();
    }
}
