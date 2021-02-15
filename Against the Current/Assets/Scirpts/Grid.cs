using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Grid : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Image image;
    private Text textCount;//文本显示数目
    private int count;//格子物品数目
    private Item item;
    private int gType;
    private int gridID;
    private void Awake()
    {
        gridID = transform.GetSiblingIndex();
        count = 0;
        image = this.gameObject.transform.Find("Image").GetComponent<Image>();
        textCount = this.gameObject.transform.Find("Text").GetComponent<Text>();
        //开始时格子没有物品
        image.gameObject.SetActive(false);
        textCount.gameObject.SetActive(false);

    }

    //外部获取格子数目
    public int GetCount() 
    {
        if(item!=null)
        {
            return count;
        }
        return 0;
    }
    public void SetDurable(int t)
    {
        count = t;//耐久和数量
    }
    public void SetGrid(Item _item, int _count=1)//_count可以是多个，做到同时拾取多个物品
    {
        if (count <= 0)
        {
            //image.gameObject.SetActive(true);//自己无法做到
            item = _item;//将物品属性返回格子中
            image.sprite = _item.sprite;
            if (_item.itemType != Item.ItemType.equipment)
            {
                count = _count;
                //textCount.gameObject.SetActive(true);自己无法做到
                textCount.text = count.ToString();
            }
            else
            {
                count = _count;
            }
        }
        else
        {
            item = _item;
            //数目大于等于一不用激活
            Debug.Log("物品数目大于0的格子，");
            count += _count;
            textCount.text = count.ToString();
        }
    }
    public void SetImgActive(bool value=true)
    {
        image.gameObject.SetActive(value);
    }
    public void SetTextActive(bool value = true)
    {
        textCount.gameObject.SetActive(value);
        textCount.text = count.ToString();
    }
    public void InitItemType(int type)
    {
        gType =type;
    }
    public int GetItemID()
    {
        return item.itemId;
    }
    public void SetImageSprite()
    {
        image.sprite=item.sprite;
    }

    public Item GetItem()
    {
        if (item != null)
            return item;
        return null;
    }

    public int GetGridID()
    {
        return gridID;
    }
    public int GetGridType()
    {
        return gType;
    }
    public bool RemoveItem()
    {
        if (count <= 0)
            return false;
        if(count>=1)
        {
            if (item.itemType == Item.ItemType.equipment)
            {
                count = 0;
                image.sprite = null;
                item = null;
                image.gameObject.SetActive(false);
                textCount.gameObject.SetActive(false);
                return false;
            }
            count -= 1;
            if(count<=0)
            {
                image.sprite = null;
                item = null;
                image.gameObject.SetActive(false);
                textCount.gameObject.SetActive(false);
                return false;
            }
        }
        //bool表示可以继续删除
        return true;
    }

    public void ChangeTo(Grid _grid)
    {
        count=_grid.GetCount();
        item=_grid.GetItem();
        image.gameObject.SetActive(true);
        SetImageSprite();//更换图片
    }
    public void ChangeToEmpty()
    {
        count = 0;
        item = null;
        image.gameObject.SetActive(false);
    }


    public void SetColor()
    {
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (count <= 0)
                return;
            Debug.Log(count);
            ItemPanel.itemP.gameObject.SetActive(true);
            ItemPanel.itemP.grid = this;
            ItemPanel.itemP.transform.position=new Vector3(Input.mousePosition.x+80,Input.mousePosition.y,0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.image.color= new Color(255, 20, 0, 1);
        if (count >= 1 && item != null)
            transform.parent.parent.SendMessage("DisplayTip",item.itemId);
        else
            transform.parent.parent.SendMessage("DisplayTip",-1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.image.color = new Color(255,255,255, 1);    
    }
}
