using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave
{
    //背包不需要存储，存储在网络上了，意味着盾牌的装备也要联网，耐久
    //时光倒流，逆流的信息不存，因为每次存档刷新纪录点
    //玩家输入playhandle脚本不存，减少数据读取复原
    //玩家状态nl,gun,battle等不存

    //普通敌人位置等信息不存，没有敌人才能存档
    //玩家的主物体位置,旋转量,设置特定存档点，那么模型就没必要存储了
    public string playerPositionX;
    public string playerPositionY;
    public string playerPositionZ;
    public string playerRotionX;
    public string playerRotionY;
    public string playerRotionZ;
    //|||ATK(由装备决定)体力条,BP是否可恢复不做（对游戏基本无影响）
    //玩家的基本状态信息，HP,WP,BP（使用过饰品会不一样,但是由于会添加物品，防御不用存）,
    //----------------------不能用float，哪怕存的时候没用到也不行
    public string hp;
    public string wp;
    public int ammo;
    //盾牌的装备与否（暂时不做）
    //提前读取

    //鼠标灵敏度,音量
    public string mouseSensitivity;
    public string voiceValue;
    //玩家处于第几个场景
    public int sceneNum=1;
    
    //任务完成情况参数，奖励情况等，要存储
    public List<Task> taskList=new List<Task>();
    //
    public int sealEnemyNum = 0;
    public int dunEnemyNum = 0;
    public int backStabEnemyNum = 0;
    public bool passLevel1 = false;
    public bool passLevel2 = false;

    //克隆
    public GameSave CloneSave()
    {
        return MemberwiseClone() as GameSave;
    }

    //
}
