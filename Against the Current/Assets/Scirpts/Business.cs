using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Business : MonoBehaviour
{
    public BagPanel bp;
    //是脚本
    public ItemTipText itt;
    public GameObject buyTip;
    public Text buyTipText;
    public Button buy;
    public Item item_s;
    [Header("======     装备和药品面板       ==========")]
    public GameObject equipmentPanel;
    public GameObject consumablesPanel;
    [Header("====  如果需要选中效果时再写代码   ===")]
    public Grid tempGrid;
    private void Start()
    {
        buy.onClick.AddListener(TryBuy);
        InitEquip();
        InitConsum();
        equipmentPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void InitEquip()
    {
        int i = 0;
        Grid[] grids=equipmentPanel.GetComponentsInChildren<Grid>();
        foreach (var grid in grids)
        {
            grid.SetGrid(bp.GetItem(i));
            grid.SetImgActive();
            i++;
        }
    }
    public void InitConsum()
    {
        int i = 6;
        Grid[] grids = consumablesPanel.GetComponentsInChildren<Grid>();
        foreach (var grid in grids)
        {
            grid.SetGrid(bp.GetItem(i));
            grid.SetImgActive();
            i++;
        }
    }
    public void SellEquipment()
    {
        equipmentPanel.SetActive(true);
        consumablesPanel.SetActive(false);
    }
    public void SellConsum()
    {
        consumablesPanel.SetActive(true);
        equipmentPanel.SetActive(false);
    }
    //获得物品信息
    public void GetItem(Item _item)
    {
        item_s = _item;
        if (!buyTip.activeSelf)
            buyTip.SetActive(true);
        //获得物品描述信息；
        buyTipText.text =itt.GetItemTip(_item.itemId);
    }
    void TryBuy()
    {
        if(item_s!=null)
        TryBuySth(item_s.itemId);
    }
    void TryBuySth(int itemid)
    {
        //如果价格对不上
        //以及背包满了由bp判断？
        if(item_s!=null)
        {
            //如果不够钱
            if(item_s.itemPrice>bp.coin)
            {
                buyTipText.text = "快去赚钱吧";
                return;
            }
            if(!bp.BuyItem(item_s.itemId))
            {
                buyTipText.text = "背包满了";
            }
        }
    }
}
