using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviour : PlayableBehaviour
{
    public ActorManager amPlayable;
    public float myFloat;
    PlayableDirector pd;
    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnGraphStart(Playable playable)
    {
        //
        pd = (PlayableDirector)playable.GetGraph().GetResolver();
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "My Playable Track")
        //    {
        //    }
        //    else if (track.streamName == "My Playable Track2")
        //    {
        //    }
        //}
    }
    public override void OnGraphStop(Playable playable)
    {
        if (amPlayable != null)
            amPlayable.UnLockAC();
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();//获得导演,错误已经有了
        if (pd!= null)
        {
            pd.playableAsset = null;//清空剧本
            pd = null;//清空控制
        }
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (amPlayable!= null)
        {
            amPlayable.LockAC();//
        }
    }
}
