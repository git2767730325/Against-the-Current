using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItem : MonoBehaviour
{
    public int itemID;//奖励的物品ID
    public int itemNum = 1;//剩余多少个奖励物品，以防背包满了
    public bool canReward = false;//完成任务可获取奖励，领取成功就false,避免重复领取
    public void Finshed()
    {
        canReward = true;
    }
}

