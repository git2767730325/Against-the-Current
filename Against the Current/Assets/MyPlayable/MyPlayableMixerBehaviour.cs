using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MyPlayableMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //取AM？
        ActorManager trackBinding = playerData as ActorManager;
        if (!trackBinding)//track没有放入am脚本的话不运行后面
            return;
        int inputCount = playable.GetInputCount ();//取得轨道内behavior数目
        for (int i = 0; i < inputCount; i++)
        {
            
            float inputWeight = playable.GetInputWeight(i);
            //ScriptPlayable是Base class for all user-defined playables
            ScriptPlayable<MyPlayableBehaviour> inputPlayable = (ScriptPlayable<MyPlayableBehaviour>)playable.GetInput(i);
            //???
            MyPlayableBehaviour input = inputPlayable.GetBehaviour ();//一个Clip里的一或多个behaviord
            //input.myCamera.transform.Translate(new Vector3(0, 0, 0.01f));
        }
    }
}
