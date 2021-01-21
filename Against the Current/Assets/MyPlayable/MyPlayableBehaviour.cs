using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviour : PlayableBehaviour
{
    public ActorManager myCamera;
    public float myFloat;

    PlayableDirector pd;
    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnGraphStart(Playable playable)
    {
        pd = (PlayableDirector)playable.GetGraph().GetResolver();
        foreach (var track in pd.playableAsset.outputs)
        {
            if (track.streamName == "My Playable Track")
            {
              // ActorManager am=(ActorManager) pd.GetGenericBinding(track.sourceObject);
                //am.LockAC();
            }
        }
    }
    public override void OnGraphStop(Playable playable)
    {
        pd = (PlayableDirector)playable.GetGraph().GetResolver();//��õ���
        if(pd!=null)
        pd.playableAsset = null;//��վ籾
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        myCamera.LockAC();//myCamera����am���Ժ��
    }



}
