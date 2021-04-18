using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class LoginManager : MonoBehaviour
{
    public GameObject login;
    public GameObject register;
    public GameObject warning;


    [Header("=========== Login============")]
    public InputField accountL;
    public InputField passwordsL;
    public Button loginBtn;
    public Button registerBtn;

    [Header("=========== Register============")]
    public InputField accountR;
    public InputField passwordsR;
    public InputField registerR;
    public Button cancelBtn;
    public Button confirmBtn;

    [Header("=========== Warning============")]
    public Button closeBtn;
    public Text warningText;
    [Header("--------- 登陆的账号密码--------------")]
    public string account;
    public string passwords;
    private void Awake()
    {
        registerBtn.onClick.AddListener(OnEnterRegister);
        cancelBtn.onClick.AddListener(OnEnterLogin);//回到登陆界面事件；
        //登陆,注册
        loginBtn.onClick.AddListener(Login);
        confirmBtn.onClick.AddListener(Register);
        //warning
        closeBtn.onClick.AddListener(CloseWarning);
        //passwordsL.onValueChanged.AddListener(delegate { InputText(); });
        passwordsL.onValueChanged.AddListener(InputText);
    }
    private void Start()
    {
        Client.StartThread();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("要在游戏中才退出");
    }

    public void Login()
    {
        AudioManager.SetUIClipsAndPlay();
        if (accountL.text.Length > 0 && passwordsL.text.Length > 0)
        {
            //联网判断账号密码
            JsonData jd = new JsonData();
            //联网功能分为登陆界面1，装备管理2、、
            jd["function"] = 1;
            //登陆模式为1，注册模式为2、、
            jd["mode"] = 1;
            jd["account"] = accountL.text;
            jd["passwords"] = passwordsL.text;
            Client.Send(jd);
            account = accountL.text;
            passwords = passwordsL.text;
        }
        else
            Warning("a");
    }
    public void Register()
    {
        AudioManager.SetUIClipsAndPlay();
        //注册账号密码的长度要大于0
        if (accountR.text.Length > 0 && registerR.text.Length > 0)
        {
            if (registerR.text == passwordsR.text)
            {
                //联网下判断账号是否存在
                JsonData jd = new JsonData();
                //联网功能分为登陆界面1，装备管理2、、
                jd["function"] = 1;
                //登陆模式为1，注册模式为2、、
                jd["mode"] = 2;
                jd["account"] = accountR.text;
                jd["passwords"] = passwordsR.text;


                GameManager.SendMessages(jd);

                Warning("wait");
                //
            }
            else
                Warning("password error");
        }
        else//输入不全
            Warning("a");
    }

    public void Warning(string str)
    {
        warning.SetActive(true);
        if (str == "wait")
        {
            warningText.text = str;
            return;
        }
        if (str == "a")
            warningText.text = "请确认输入是否完整？";
        if (str == "password error")
            warningText.text = str;
        else if (str == "register fail")
            warningText.text = str;
        else if (str == "register success")
        {
            warningText.text = str;
            //上传网络判定存储账号
            OnEnterLogin();
        }
        else if (str == "Repeat login")
        {
            warningText.text = str;
        }
        else if (str == "Login success")
        {
            warningText.text = str;
            GameManager.GM.ChangeSceneLoad(7);
        }
        else if (str == "account error")
        {
            warningText.text = str;
        }
    }
    //GM判断返回是否注册成功
    public void LinkState(int value)
    {
        if (value == 1)
            Warning("register success");
        else if (value == 0)
            Warning("register fail");
        else if (value == 2)
            Warning("Repeat login");
        else if (value == 3)
        {
            Warning("Login success");
        }
        else if (value == 4)
            Warning("password error");
        else if (value == 5)
            Warning("account error");
        Debug.Log("LinkState");
    }
    //进入注册界面
    public void OnEnterRegister()
    {
        accountL.text = "";
        passwordsL.text = "";
        login.SetActive(false);
        register.SetActive(true);
        AudioManager.SetUIClipsAndPlay();
    }

    //回到登陆界面
    public void OnEnterLogin()
    {
        accountR.text = "";
        passwordsR.text = "";
        registerR.text = "";
        login.SetActive(true);
        register.SetActive(false);
        AudioManager.SetUIClipsAndPlay();
    }


    public void CloseWarning()
    {
        warning.SetActive(false);
    }
    public void InputText(string i)//string i不用委托
    {
        AudioManager.SetUIClipsAndPlay(1);
    }


    //切换到下个场景会破坏连接
    private void OnDestroy()
    {
        JsonData jd = new JsonData();
        jd["function"] = 9;
        jd["account"] = account;
        jd["passwords"] = passwords;
        GameManager.SendMessages(jd);
        Client.CloseThread();
    }
}
