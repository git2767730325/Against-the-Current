using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public MyPlayableBehaviour template = new MyPlayableBehaviour ();
    //ExposedReference泛型，创建对场景对象的引用。
    public ExposedReference<ActorManager> amPlayable;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MyPlayableBehaviour>.Create (graph, template);
        //不可缺少
        MyPlayableBehaviour clone = playable.GetBehaviour ();
        amPlayable.exposedName =GetInstanceID().ToString();
        clone.amPlayable = amPlayable.Resolve (graph.GetResolver ());
        return playable;
    }
}
