using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public MyPlayableBehaviour template = new MyPlayableBehaviour ();
    public ExposedReference<ActorManager> myCamera;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MyPlayableBehaviour>.Create (graph, template);
        MyPlayableBehaviour clone = playable.GetBehaviour ();
        //≥ı ºªØ
       // myCamera.exposedName = new PropertyName(GetInstanceID().ToString());
        clone.myCamera = myCamera.Resolve (graph.GetResolver ());
        //myCamera=new Vector3(0,0,1);
        return playable;
    }
}
