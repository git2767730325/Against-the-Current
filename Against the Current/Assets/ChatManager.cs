using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class ChatManager : MonoBehaviour
{
    public RectTransform disRectTrans;
    public RectTransform textRectTrans;
    public Text chatText;
    public InputField inputText;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GM.cm = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (disRectTrans.rect.height < textRectTrans.rect.height)
        {
            Debug.Log("aaaaasa");
            disRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,0, textRectTrans.rect.height);
            //disRectTrans.rect.Set(disRectTrans.rect.x, disRectTrans.rect.y, disRectTrans.rect.width, textRectTrans.rect.height);
        }
        textRectTrans.localPosition = new Vector3(textRectTrans.localPosition.x, textRectTrans.rect.height / 2 * -1 - 3, 0);
        Canvas.ForceUpdateCanvases();
    }
    public void OnClickBtn()
    {
        if (inputText.text != "")
            chatText.text += "\n" + "<color=red>我</color>:"+inputText.text;
        JsonData jd = new JsonData();
        jd["function"] = 5;//发送消息
        jd["account"] = GameManager.apJD["account"];//便于服务端管理
        jd["msg"]=inputText.text;
        GameManager.SendMessages(jd);
        inputText.text = "";
    }
    public void ReciveChat(JsonData _jd)
    {
        string userName =_jd["account"].ToString();
        string Text = _jd["msg"].ToString();
        if (chatText == null)
            return;
         chatText.text += "\n" + "<color=green>"+"user_"+userName+"</color>:" + Text;
    }
}
