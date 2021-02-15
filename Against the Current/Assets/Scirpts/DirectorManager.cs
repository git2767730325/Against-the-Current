using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;
    public TimelineAsset stab;//剧本
    public TimelineAsset seal;
    private int i = 0;

    void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }

    public void PlayTimeline(string TimelineName, ActorManager _attacker, ActorManager _victim)
    {
        pd.Evaluate();
        if(TimelineName=="stab")
        {
            pd.playableAsset = Instantiate(stab);//干净
            pd.playableAsset = Instantiate(pd.playableAsset);
            TimelineAsset tls = (TimelineAsset)pd.playableAsset;

            //pd.Evaluate();
            foreach (var track in tls.GetOutputTracks())
            {
                if (track.name == "Animation Track1")
                {
                    pd.SetGenericBinding(track, _attacker.ac.anim);
                }
                if (track.name == "Animation Track2")
                    pd.SetGenericBinding(track, _victim.ac.anim);
                if (track.name == "My Playable Track")
                {
                    pd.SetGenericBinding(track, am);
                    foreach (var clip in track.GetClips())//取得每个clip
                    {
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        pd.SetReferenceValue(myClip.amPlayable.exposedName,_attacker);
                        myBehaviour.myFloat = 232;
                        //Debug.Log(myBehaviour.myFloat);
                    }
                }
                if (track.name == "My Playable Track2")
                {
                    pd.SetGenericBinding(track,_victim);
                    foreach (var clip in track.GetClips())//取得每个clip
                    {
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        pd.SetReferenceValue(myClip.amPlayable.exposedName,_victim);
                    }
                }
                else if (track.name == "Control Track2")
                {
                    //ControlTrack ctrack=(ControlTrack)track;
                    foreach (var clip in track.GetClips())
                    {
                        ControlPlayableAsset cpa = (ControlPlayableAsset)clip.asset;
                        pd.SetReferenceValue(cpa.sourceGameObject.exposedName, _victim.gameObject);
                    }
                }
            }
        }
        else if(TimelineName == "seal")
        {
            pd.playableAsset = Instantiate(seal);
            pd.playableAsset = Instantiate(pd.playableAsset);
            TimelineAsset tla = (TimelineAsset)pd.playableAsset;
            foreach (var track in tla.GetOutputTracks())
            {
                if (track.name == "Animation Track")
                {
                    pd.SetGenericBinding(track, _attacker.ac.anim);
                    am.ac.OnEnterBattle();
                }
                else if (track.name == "My Playable Track")
                {
                    pd.SetGenericBinding(track, am);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        pd.SetReferenceValue(myClip.amPlayable.exposedName, _attacker);
                        Debug.Log("a"+clip.displayName);
                    }
                }
                else if (track.name == "My Playable Track2")
                {
                    pd.SetGenericBinding(track, _victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        pd.SetReferenceValue(myClip.amPlayable.exposedName, _victim);
                        Debug.Log("b");
                    }
                }
                else if (track.name == "Control Track2")
                {
                    foreach (var clip in track.GetClips())
                    {
                        ControlPlayableAsset cpa = (ControlPlayableAsset)clip.asset;
                        pd.SetReferenceValue(cpa.sourceGameObject.exposedName, _victim.gameObject);
                    }
                }
            }
        }
        else if (TimelineName == "openbox")
            Debug.Log("aa");
            pd.Play();
    }
}
