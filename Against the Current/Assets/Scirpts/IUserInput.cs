using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    public MyButton KeyA = new MyButton();
    public MyButton KeyB = new MyButton();
    public MyButton KeyC = new MyButton();
    public MyButton KeyD = new MyButton();
    public MyButton Btn1 = new MyButton();
    public MyButton Btn2 = new MyButton();
    public MyButton Btn3 = new MyButton();
    public MyButton Btn4 = new MyButton();
    public MyButton Btn5 = new MyButton();
    public MyButton Btn6 = new MyButton();
    public MyButton Btn7 = new MyButton();
    public MyButton Btn8 = new MyButton();
    public MyButton Btn9 = new MyButton();
    public MyButton BtnUI1 = new MyButton();
    public MyButton BtnUI2 = new MyButton();
    public MyButton BtnUI3 = new MyButton();
    public MyButton BtnUI4 = new MyButton();
    public string up;
    public string down;
    public string left;
    public string right;

    public string keyA;//跑步，闪避
    public string keyB;//跳跃
    public string keyC;//攻击，背刺，
    public string keyD;//防御,放大视角
    public string btn1;//盾反
    public string btn2;//上子弹
    public string btn3;//拔枪
    public string btn4;//切换形态
    public string btnA;//蹲下
    public string btnB;//觉醒？
    [Header("===========    MyTime  ===========")]
    public string btnT1;//时光倒流
    public string btnT2;//逆流
    public string btnT3;//子弹时间

    [Header("====== UI面板 =======")]
    public string btnUI1;//物品栏
    public string btnUI2;//人物信息
    public string btnUI3;//小地图
    public string btnUI4;//主菜单

    [Header("====== 视角移动按键 =======")]
    public string keyJup;
    public string keyJright;
    public string keyJdown;
    public string keyJleft;


    [Header("Signal")]
    public float Dup;
    public float Dright;
    public float Dup2;
    public float Dright2;

    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;
    public float Jup;
    public float Jright;

    [Header("Signal2")]
    public float Dmo;
    public Vector3 Dvec;

    [Header("Bool")]
    public bool inputEnable = true;
    public bool run;
    public bool jump;
    public bool ground;
    public bool attack;
    public bool defence;
    public bool counterBack;
    public bool action;
    [Header("====== DoubleTrigger  ======")]
    public bool lockOn = false;

    [Header("Mouse")]
    public bool mouseEnable;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    protected void SquareToCircle(float x, float y)//x,y are input ,Dup2,Dright2 are ouput ,没有规范命名
    {
        float X, Y;
        X = x * Mathf.Sqrt(1 - y * y / 2.0f);
        Y = y * Mathf.Sqrt(1 - x * x / 2.0f);
        Dup2 = Y;
        Dright2 = X;
    }
    protected void GoMove(float Dright,float Dup)
    {
        SquareToCircle(Dright, Dup);
        Dmo = Mathf.Sqrt(Dup2* Dup2 + Dright2* Dright2);
        Dvec = Dup2* transform.forward + Dright2* transform.right;
    }
}
