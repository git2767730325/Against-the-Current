using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer 
{
    public enum State{ 
    IDLE,
    RUN,
    FINISHED,
    }
    public State state;
    public float duration=1.0f;
    public float elapsedTime = 0.0f;

    public void Tick()
    {
        if(state==State.IDLE)
        {
            //Go(targetTime);
        }
        else if(state==State.RUN)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
                state = State.FINISHED;
        }
        else
        {
        }

    }

    public void StartTimer(float targetTime)
    {
        duration = targetTime;
        elapsedTime = 0.0f;
        state = State.RUN;
    }
}
