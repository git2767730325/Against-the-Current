using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemTipText : MonoBehaviour
{
    public TextAsset itemTip;
    public List<string> itemTipList = new List<string>();
    public Text tip;
    private void Awake()
    {
        //初始化
        itemTip = Resources.Load<TextAsset>("itemtip");
        if (itemTip == null)
            Debug.Log("noting");
        string[] tips = itemTip.text.Split('\n');
        foreach (var tip in tips)
        {
            itemTipList.Add(tip);
        }
        this.gameObject.SetActive(false);
    }

    public void DisplayTip(int index)
    {
        if (index == -1)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        if (index > 10)
            index = 10;
        tip.text = itemTipList[index];

    }
    public string GetItemTip(int index)
    {
        return itemTipList[index];
    }
}
