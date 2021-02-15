using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandle : IUserInput
{
    //public string up;
    //public string down;
    //public string left;
    //public string right;

    //public string keyA;
    //public string keyB;
    //public string keyC;
    //public string keyD;

    //public string keyJup;
    //public string keyJright;
    //public string keyJdown;
    //public string keyJleft;


    //[Header("Signal")]
    //public float Dup;
    //public float Dright;
    //public float Dup2;
    //public float Dright2;

    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;
    //public float Jup;
    //public float Jright;

    //[Header("Signal2")]
    //public float Dmo;
    //public Vector3 Dvec;

    //[Header("Bool")]
    //public bool inputEnable = true;
    //public bool run;
    //public bool jump;
    //public bool ground;
    //public bool attack;

    //private bool lastjump;
    //private bool lastattack;
    // Update is called once per frame

    private void Awake()
    {
        
    }
    void Update()
    { 
        KeyA.Tick(Input.GetKey(keyA));
        KeyB.Tick(Input.GetKey(keyB));
        KeyC.Tick(Input.GetKey(keyC));
        KeyD.Tick(Input.GetKey(keyD));
        Btn1.Tick(Input.GetKey(btn1));
        Btn2.Tick(Input.GetKey(btn2));//上子弹
        Btn3.Tick(Input.GetKey(btn3));//枪态
        Btn4.Tick(Input.GetKey(btn4));//切换模式
        Btn5.Tick(Input.GetKey(btnT1));//时光
        Btn6.Tick(Input.GetKey(btnT2));//逆流
        Btn7.Tick(Input.GetKey(btnT3));//子弹时间
        Btn8.Tick(Input.GetKey(btnA));//蹲下
        Btn9.Tick(Input.GetKey(btnB));//
        BtnUI1.Tick(Input.GetKey(btnUI1));//物品栏
        BtnUI2.Tick(Input.GetKey(btnUI2));//人物信息
        BtnUI3.Tick(Input.GetKey(btnUI3));//小地图
        BtnUI4.Tick(Input.GetKey(btnUI4));//菜单
        if (mouseEnable)
        {
            Jup = Input.GetAxis("Mouse Y")*-mouseSensitivityY;
            Jright = Input.GetAxis("Mouse X")*mouseSensitivityX;
        }
        else
        {
            //使用平滑，视角移动
            Jup = (Input.GetKey(keyJup) ? 1.0f : 0) - (Input.GetKey(keyJdown) ? 1.0f : 0);
            Jright = (Input.GetKey(keyJright) ? 1.0f : 0) - (Input.GetKey(keyJleft) ? 1.0f : 0);
        }
        //目标速度值
        targetDup = (Input.GetKey(up) ? 1.0f : 0f) + (Input.GetKey(down) ? -1.0f : 0f);//记得括号，3元运算先后顺序不同
        targetDright = (Input.GetKey(right) ? 1.0f : 0f) + (Input.GetKey(left) ? -1.0f : 0f);
        //软控制，避免数值大乱
        if(!inputEnable)
        {
            targetDright = targetDup = 0.0f;//清空输入
        }
        //使用平滑达到目标速度，时间0.2s可变
        Dup = Mathf.SmoothDamp(Dup, targetDup,ref velocityDup, 0.2f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.2f);
        SquareToCircle(Dright, Dup);
        //方向和速度向量需要纠正
        Dmo = Mathf.Sqrt(Dup2*Dup2 + Dright2*Dright2);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
        //改用类,一行代码,虽然down up 也可以
        run = KeyA.IsPressing;
        jump = KeyB.OnPressed;
        attack = KeyC.OnPressed;
        defence = KeyD.IsPressing;
        counterBack = Btn1.OnPressed;
        action = Btn1.OnPressed;
        //
    }
}
