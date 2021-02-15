using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public float durationLongTime;
    public float durationDoubleTime;

    public bool IsPressing=false;
    public bool OnPressed=false;
    public bool OnReleased=false;
    public bool IsExtending = false;
    public bool LongPressing = false;
    public bool ShortPressed = false;
    public bool DoublePressed= false;


    public bool curState=false;
    public bool lastState=false;
    MyTimer myTimer=new MyTimer();
    MyTimer myTimer2=new MyTimer();//延时时间判断长按
    public void Tick(bool input)//外部调用
    {
        myTimer.Tick();
        myTimer2.Tick();
        IsPressing = input;
        curState = IsPressing;
        if (input)
        {
            if (curState != lastState)
            {
                OnPressed = true;
                myTimer2.StartTimer(0.4f);
            }
            else
                OnPressed = false;

            OnReleased= false;
            //lastState = true;
        }
        else
        {
            if (lastState)
            {
                //lastState = false;
                OnReleased = true;
                myTimer.StartTimer(1.0f);
            }

            else
            {
                OnReleased = false;
            }
            OnPressed = false;
        }
            IsExtending = (myTimer.state == MyTimer.State.RUN);//通过计时来判断是否延时按键
            lastState =curState;
            DoublePressed = (IsExtending && OnPressed);//规定时间内按第二下，感觉判断条件不算完善
            LongPressing = (myTimer2.state == MyTimer.State.FINISHED&&IsPressing);
            ShortPressed = (OnReleased && myTimer2.state == MyTimer.State.RUN);//快速按下松开
    }

}
