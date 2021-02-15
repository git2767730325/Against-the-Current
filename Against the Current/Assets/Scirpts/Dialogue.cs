using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    public TextAsset dialogueAsset;
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
    void Awake()
    {
        InitTextList();
        //InitTextList("test");
    }
    // Update is called once per frame
    public void Dialog()
    {
        if (diaList.Count > index)
        {
            dialog.SetActive(true);
            ChangeImg(diaList[index]);
            dialogText.text = diaList[index];
            index++;
        }
        else if (diaList.Count == index)
        {
            dialog.SetActive(false);
            index = 0;
        }
    }

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

    public void InitTextList()
    {
        if(dialogueAsset!=null)
        {
            index = 0;
            diaList.Clear();
            string[] txtStr = dialogueAsset.text.Split('\n');
            foreach (var str in txtStr)
            {
                diaList.Add(str);
            }
        }
    }

    public void ChangeImg(string h)
    {
        switch (h)
        {
            case "A\r"://player
                headImg.sprite = player;
                index++;
                break;
            case "B\r"://npc
                headImg.sprite = npc;
                index++;
                break;
            case "C\r"://Reward Item
                itemImg.gameObject.SetActive(true);
                itemImg.sprite = itemSprite;
                index++;
                break;
        }
    }

    public void SetDia()
    {
        uim.dialogue = this;
    }
    public void CancelDia()
    {
        uim.dialogue = null;
        index = 0;
        if (dialog.activeSelf)
            dialog.SetActive(false);
    }
}
