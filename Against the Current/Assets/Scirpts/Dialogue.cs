using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    public TextAsset dialogueAsset;
    public TextAsset dialogueAsset_1;
    public List<string> diaList=new List<string>();
    private int index;
    public GameObject dialog;
    public Text dialogText;
    public Image headImg;
    public Image itemImg;
    public Sprite player;
    public Sprite npc;
    public Sprite itemSprite;
    public UIManager uim;
    public int taskTargetnum = 1;
    void Awake()
    {
        uim = GameObject.FindWithTag("uim").GetComponent<UIManager>();
        InitTextList(dialogueAsset);
        //InitTextList("test");
    }
    // Update is called once per frame
    public void Dialog()
    {
        if (diaList.Count > index)
        {
            if(!dialog.activeSelf)
            dialog.SetActive(true);
            SwitchText(diaList[index]);
            dialogText.text = diaList[index];
            if (dialog.activeSelf)
            index++;
        }
        else if (diaList.Count <= index)
        {
            dialog.SetActive(false);
            index = 0;
        }
    }

    //初始化，更换一号脚本的资源
    public void InitTextList(string txt)
    {
        diaList.Clear();
        index = 0;
        dialogueAsset = (TextAsset)Resources.Load(txt);
        string[] txtStr = dialogueAsset.text.Split('\n');
        foreach (var str in txtStr)
        {
            diaList.Add(str);
        }
    }
    //初始化和更换脚本的使用
    public void InitTextList(TextAsset diatext)
    {
        if(diatext!=null)
        {
            index = 0;
            diaList.Clear();
            string[] txtStr = diatext.text.Split('\n');
            foreach (var str in txtStr)
            {
                diaList.Add(str);
            }
        }
    }

    //使用1号脚本通道
    public void UseAssetOne()
    {
        InitTextList(dialogueAsset_1);
        dialog.SetActive(false);
    }

    //根据任务选择脚本，包含了更换对话文本了
    public void ChangeAsset(string name)
    {
        if(name==null)
        {
            //如果没有完成的任务，使用初始的对话脚本
            InitTextList(dialogueAsset);
            return;
        }
        //有完成的任务
        dialogueAsset_1 = Resources.Load<TextAsset>("dialogue/"+name.Trim());
        InitTextList(dialogueAsset_1);
    }

    //根据文本判断触发
    public void SwitchText(string h)
    {
        switch (h)
        {
            case "A\r"://player
                headImg.gameObject.SetActive(true);
                headImg.sprite = player;
                index++;
                break;
            case "B\r"://npc
                headImg.gameObject.SetActive(true);
                headImg.sprite = npc;
                index++;
                break;
            case "C\r"://Reward Item
                itemImg.gameObject.SetActive(true);
                itemImg.sprite = itemSprite;
                index++;
                break;
            case "R1\r":
                string a = GameManager.GM.CheckTask();
                if (a != null)
                {
                    ChangeAsset(a);
                }
                else
                {
                    index++;
                    SwitchText(diaList[index]);
                }
                break;
            case "S1":
                index = diaList.Count - 1;
                dialog.SetActive(false);
                //商店UI；耦合高做法
                uim.OpenBusinessPanel(true);
                break;
            case "S2":
                index = diaList.Count - 1;
                dialog.SetActive(false);
                //商店UI；耦合高做法
                uim.OpenBusinessPanel(false);
                break;
            //case "E\r":
                //rewardItem.Finshed();
            //    break;
            case "G1":
                uim.TryGoHome();
                dialog.SetActive(false);
                break;
            case "L1":
                uim.sceneNum = 2;
                uim.TryGoHome();
                dialog.SetActive(false);
                InitTextList(dialogueAsset);
                break;
            case "L2":
                uim.sceneNum = 3;
                uim.TryGoHome();
                dialog.SetActive(false);
                InitTextList(dialogueAsset);
                break;
            case "L3":
                uim.sceneNum = 4;
                uim.TryGoHome();
                dialog.SetActive(false);
                break;
            case "LA":
                uim.sceneNum = 5;
                uim.TryGoHome();
                dialog.SetActive(false);
                break;
            case "LB":
                uim.sceneNum = 6;
                uim.TryGoHome();
                dialog.SetActive(false);
                InitTextList(dialogueAsset_1);
                break;
        }
    }

    //绑定和解绑 
    public void SetDia()
    {
        uim.dialogue = this;
    }
    public void CancelDia()
    {
        if(headImg!=null)
        headImg.gameObject.SetActive(false);
        uim.dialogue = null;
        index = 0;
        if (dialog.activeSelf)
            dialog.SetActive(false);
    }
}
