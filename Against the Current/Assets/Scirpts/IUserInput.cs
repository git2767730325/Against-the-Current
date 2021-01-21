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
    public string up;
    public string down;
    public string left;
    public string right;

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string btn1;
    public string btn2;
    public string btn3;


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
