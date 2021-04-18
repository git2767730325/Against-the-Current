using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
public class SaveManager : MonoBehaviour
{
    public GameObject selectSavePoint;
    public GameSave gs=null;
    public bool canOverride = false;

    public Text s1;
    public Text s2;
    public Text s3;

    private void Start()
    {
        UpdateSaveText();
    }

    public void UpdateSaveText()
    {
        if (File.Exists(@"E:\" + s1.transform.parent.name + ".txt"))
        {
            FileInfo fi = new FileInfo(@"E:\" + s1.transform.parent.name + ".txt");
            Debug.Log(fi.CreationTime);
            s1.text = fi.LastWriteTime.ToString();
        }
        if (File.Exists(@"E:\" + s2.transform.parent.name + ".txt"))
        {
            FileInfo fi2 = new FileInfo(@"E:\" + s2.transform.parent.name + ".txt");
            Debug.Log(fi2.CreationTime);
            s2.text = fi2.LastWriteTime.ToString();
        }
        if (File.Exists(@"E:\" + s3.transform.parent.name + ".txt"))
        {
            FileInfo fi3 = new FileInfo(@"E:\" + s3.transform.parent.name + ".txt");
            Debug.Log(fi3.CreationTime);
            s3.text = fi3.LastWriteTime.ToString();
        }
    }
    //点击按钮设置存档点
    public void SettingSavePoint(GameObject obj)
    {
        if(selectSavePoint!=null)
        {
            selectSavePoint.GetComponent<Image>().color=new Color(1,1,1,1);
        }
        selectSavePoint = obj;
        selectSavePoint.GetComponent<Image>().color = new Color(1, 0, 0, 1);
    }
    //复制存档
    public void CopySave()
    {
        if (selectSavePoint!=null)
        {
            //GameSave gameSave = null;
            if (File.Exists(@"E:\" + selectSavePoint.name + ".txt"))
            {
                using (StreamReader sr = new StreamReader(@"E:\" + selectSavePoint.name + ".txt"))
                {
                    string s=sr.ReadToEnd();
                    gs = JsonMapper.ToObject<GameSave>(s);
                    canOverride = true;
                }
            }
            
        }
    }
    //确定，存档复制和删除存档
    public void OverrideSave()
    {
        if(canOverride)
        {
            if (gs == null)
            {
                canOverride = false;
                return;
            }
            using (StreamWriter sr = new StreamWriter(@"E:\" + selectSavePoint.name + ".txt"))
            {
                string s = JsonMapper.ToJson(gs);
                sr.Write(s);
                canOverride = false;
                gs = null;
                selectSavePoint.GetComponentInChildren<Text>().text=Time.time.ToString("f3");
                Debug.Log("覆盖存档了");
            }
        }
        
    }
    public void DeleteSave()
    {
        if(selectSavePoint!=null)
        if (File.Exists(@"E:\" + selectSavePoint.name + ".txt"))
        {
            File.Delete(@"E:\" + selectSavePoint.name + ".txt");
            selectSavePoint.GetComponentInChildren<Text>().text =("空白存档");
            Debug.Log("删除存档了");
        }
        gs = null;
    }

    public void LoadSave()
    {
        if (selectSavePoint == null)
            return;
        if (File.Exists(@"E:\" + selectSavePoint.name+ ".txt"))
            selectSavePoint.GetComponentInChildren<Text>().text="加载中";
        else
            selectSavePoint.GetComponentInChildren<Text>().text="新存档";
        GameManager.GM.saveName = selectSavePoint.name;
        GameManager.GM.LoadByJson(0);
    }


    public void ExitAccount()
    {
        //断开连接
        GameManager.GM.ChangeScene(0);
    }

}
