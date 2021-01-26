using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class BagPanel : MonoBehaviour
{
    public int coin = 0;
    public BagPanel bp;

    public Button equipmentBtn;
    public Button consumablesBtn;
    public Button materialBtn;

    public GameObject equipPanel;
    public GameObject consumPanel;
    public GameObject materPanel;
    //按钮
    private Sprite lLight;
    private Sprite hLight;

    //金币
    public Text coinCount;

    //实例化到字典
    Dictionary<int, Item> itemDic = new Dictionary<int, Item>(); 
    private void Awake()
    {
        bp = this;
        equipmentBtn = bp.transform.Find("equipmentBtn").GetComponent<Button>();
        consumablesBtn = bp.transform.Find("consumablesBtn").GetComponent<Button>();
        materialBtn = bp.transform.Find("materialBtn").GetComponent<Button>();
        //添加切换物品栏事件，可以放到start
        equipmentBtn.onClick.AddListener(EquipBtnCall);
        consumablesBtn.onClick.AddListener(ConsumBtnCall);
        materialBtn.onClick.AddListener(MaterBtnCall);
        lLight=Resources.Load<Sprite>("UI/button");
        hLight=Resources.Load<Sprite>("UI/lightbutton");
        AddCoin(0);
        GameManager.BindBP(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        InitItemDic();
        EquipBtnCall();
        InitGrid();
    }
    void EquipBtnCall()
    {
        equipPanel.SetActive(true);
        consumPanel.SetActive(false);
        materPanel.SetActive(false);
        ButtonLight(equipmentBtn, true);
        ButtonLight(consumablesBtn);
        ButtonLight(materialBtn);
    }
    void ConsumBtnCall()
    {
        equipPanel.SetActive(false);
        consumPanel.SetActive(true);
        materPanel.SetActive(false);
        ButtonLight(equipmentBtn);
        ButtonLight(consumablesBtn,true);
        ButtonLight(materialBtn);
    }
    void MaterBtnCall()
    {
        equipPanel.SetActive(false);
        consumPanel.SetActive(false);
        materPanel.SetActive(true);
        ButtonLight(equipmentBtn);
        ButtonLight(consumablesBtn);
        ButtonLight(materialBtn,true);
    }
    void ButtonLight(Button btn, bool value=false)
    {
        if (value)
        {
            btn.image.sprite = hLight;
        }
        else
            btn.image.sprite = lLight;
    }//高亮背包栏

    void InitItemDic()//读取资源
    {
        int id=0;
        Sprite[] sprites = Resources.LoadAll<Sprite>("UI/Equipment");//读取文件资源
        id=InitItemDic(Item.ItemType.equipment, sprites, id);//初始化字典
        Debug.Log("初始化字典完成id数:"+id);
        sprites = Resources.LoadAll<Sprite>("UI/Consumable");
        id=InitItemDic(Item.ItemType.comsumables, sprites, id);//初始化字典
        Debug.Log("初始化字典完成id数:"+id);
        sprites = Resources.LoadAll<Sprite>("UI/Material");
        id=InitItemDic(Item.ItemType.material, sprites, id);//初始化字典
        Debug.Log("初始化字典完成id数:"+id);
    }
    int InitItemDic(Item.ItemType _itemType,Sprite[] sprites,int _id)
    {
        int id = _id;
        //将sprite赋值到字典里
        for(int i=0;i<sprites.Length; i++)
        {
            Item item =new Item();
            item.itemId =_id +i;
            item.itemType= _itemType;
            item.sprite = sprites[i];
            if (!itemDic.ContainsKey(id))
            {
                itemDic.Add(id, item);
            }
            id++;
        }
        return id;
    }//初始化字典
    void InitGrid()//初始化格子类型int
    {
        Grid[] grids = transform.Find("EquipPanel").GetComponentsInChildren<Grid>();
        foreach (var grid in grids)
        {
            grid.InitItemType(0);
        }
        grids = transform.Find("ConsumPanel").GetComponentsInChildren<Grid>();
        foreach (var grid in grids)
        {
            grid.InitItemType(1);
        }
        grids = transform.Find("MaterPanel").GetComponentsInChildren<Grid>();
        foreach (var grid in grids)
        {
            grid.InitItemType(2);
        }

    }
    public void GridItemUpdate(JsonData _jd)//接收到更新值并更新
    {
        //如果是金币
        Debug.Log(_jd.ToJson());
        int gridIndex = (int)_jd["gridID"];
        int itemNum = (int)_jd["count"];
        int itemID = (int)_jd["itemID"];
        Item item=itemDic[itemID];
        Grid grid = null;
        if((int)itemDic[itemID].itemType ==1)//消耗品
        {
            grid=consumPanel.transform.GetChild(gridIndex).GetComponent<Grid>();
            grid.SetTextActive();
        }
        else if((int)itemDic[itemID].itemType == 2)//材料
        {
            grid=materPanel.transform.GetChild(gridIndex).GetComponent<Grid>();
            grid.SetTextActive();
        }
        else
        {
            grid=equipPanel.transform.GetChild(gridIndex).GetComponent<Grid>();
        }//装备
        grid.SetGrid(item,itemNum);
        if(itemNum<=0)
        {
            //并不可能是0
            //不显示物品
            //grid.SetTextActive(false);
        }//暂时的逻辑是有物品才网络传输并不可能是0
        else
            grid.SetImgActive();
    }
    public void AddItem()
    {
        int i = UnityEngine.Random.Range(0, itemDic.Count);//[0,count);
        Debug.Log("ID"+i);
        Item tempItem;
        Grid toGrid = null;
        tempItem = itemDic[i];
        toGrid=CheckEmptyGrid(tempItem);//检查对应栏位是否有空格,返回对应格子
        if (toGrid == null)
        {
            Debug.Log("满了，找不到格子存放");
        }
        else
        {
            toGrid.SetImgActive();
            if (!tempItem.itemType.Equals(Item.ItemType.equipment))
                toGrid.SetTextActive();
            toGrid.SetGrid(tempItem);
            JsonData jd = new JsonData();
            int iID = tempItem.itemId;
            jd["function"] = 2;
            jd["mode"] = 1;//添加物品
            jd["gridID"] = toGrid.GetGridID();
            jd["count"] = toGrid.GetCount();
            jd["itemID"] = iID;
            jd["account"] = GameManager.apJD["account"];//便于服务端管理
            GameManager.SendMessages(jd);
        }
    }
    public void DelItem(Grid _grid)
    {
        JsonData jd = new JsonData();
        _grid.RemoveItem();//可以bool判断
        if (_grid.GetCount() <= 0)
            ItemPanel.itemP.gameObject.SetActive(false);
        jd["function"] = 2;
        jd["mode"] = 2;//删除物品
        jd["gridID"] = _grid.GetGridID();
        jd["count"] = _grid.GetCount();
        jd["itemID"] = _grid.GetItemID();
        jd["account"] = GameManager.apJD["account"];//便于服务端管理
        GameManager.SendMessages(jd);
    }  
    public void SellItem(Grid _grid)
    {
        JsonData jd = new JsonData();
        jd["function"] = 2;
        jd["mode"] = 3;//删除物品
        DelItem(_grid);
        AddCoin(5);
        jd["gridID"] = _grid.GetGridID();
        jd["count"] = _grid.GetCount();
        jd["itemID"] = _grid.GetItemID();
        jd["account"] = GameManager.apJD["account"];//便于服务端管理
        
        //jd["coin"] = coin;
        GameManager.SendMessages(jd);
    }
    public void AddCoin(int a)
    {
        coin += a;
        coinCount.text = coin.ToString();
       
    }
    Grid CheckEmptyGrid(Item _item)
    {
        Grid[] grids = null;
        Grid grid = null;
        switch (_item.itemType)
        {
            case Item.ItemType.equipment:
                grids = transform.Find("EquipPanel").GetComponentsInChildren<Grid>();
                foreach (Grid g in grids)
                {
                    if (g.GetCount() == 0)
                    {
                        grid = g;
                        break;
                    }
                }
                break;
                //消耗品和材料可叠加
            case Item.ItemType.comsumables:
                grids = transform.Find("ConsumPanel").GetComponentsInChildren<Grid>();
                break;
            case Item.ItemType.material:
                grids = transform.Find("MaterPanel").GetComponentsInChildren<Grid>();
                break;
        }
        if(_item.itemType.Equals(Item.ItemType.equipment)&&grid!=null)
        {
            return grid;
           // grid.SetGrid(_item);//修改空白格子上的物品信息
        }
        else if((!_item.itemType.Equals(Item.ItemType.equipment)))
        {
            foreach (var g in grids)
            {
                if(g.GetCount()>0&&(_item.itemId==g.GetItemID()))//如果格子内有数目，可以判断下是否为对应的物品
                {
                    return g;
                }
            }
            //要是格子物品不连续则需要再遍历一次？是否产生效能问题？
            foreach (var g in grids)
            {
                if(g.GetCount()<=0)
                {
                    return g;
                }
            }
        }
        return grid;
    }
}