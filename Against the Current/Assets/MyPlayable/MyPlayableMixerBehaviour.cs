using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MyPlayableMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //ȡAM��
        ActorManager trackBinding = playerData as ActorManager;
        if (!trackBinding)//trackû�з���am�ű��Ļ������к���
            return;
        int inputCount = playable.GetInputCount ();//ȡ�ù����behavior��Ŀ
        for (int i = 0; i < inputCount; i++)
        {
            
            float inputWeight = playable.GetInputWeight(i);
            //ScriptPlayable��Base class for all user-defined playables
            ScriptPlayable<MyPlayableBehaviour> inputPlayable = (ScriptPlayable<MyPlayableBehaviour>)playable.GetInput(i);
            //???
            MyPlayableBehaviour input = inputPlayable.GetBehaviour ();//һ��Clip���һ����behaviord
            //input.myCamera.transform.Translate(new Vector3(0, 0, 0.01f));
        }
    }
}
