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

    [Header("=======   ========")]
    public ActorManager attacker;
    public ActorManager victim;
    void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
        pd.playableAsset = Instantiate(stab);//干净
    }

    public void PlayTimeline(string TimelineName, ActorManager _attacker, ActorManager _victim)
    {
        if(TimelineName=="stab")
        {
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
                        Debug.Log("aaaa");
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        Debug.Log(myClip.myCamera.exposedName);
                        pd.SetReferenceValue(myClip.myCamera.exposedName, attacker);
                        Debug.Log(myClip.myCamera.exposedName);
                        myBehaviour.myFloat = 232;
                        //Debug.Log(myBehaviour.myFloat);
                    }
                }
                if (track.name == "My Playable Track2")
                {
                    pd.SetGenericBinding(track, am);
                    foreach (var clip in track.GetClips())//取得每个clip
                    {
                        Debug.Log("aaaa");
                        MyPlayableClip myClip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour myBehaviour = myClip.template;
                        Debug.Log(myClip.myCamera.exposedName);
                        pd.SetReferenceValue(myClip.myCamera.exposedName, victim);
                        Debug.Log(myClip.myCamera.exposedName);
                    }
                }

            }
        }
        if (TimelineName == "openbox")
            Debug.Log("aa");
            pd.Play();
    }
    /*
    public void PlayTimeline(string eventName,ActorManager _attacker,ActorManager _victim)
    {
        foreach (var track in pd.playableAsset.outputs)
        {
            if(track.streamName== "Animation Track1")
            {
                pd.SetGenericBinding(track.sourceObject, _attacker.ac.anim);
            }
            if (track.streamName == "Animation Track2")
            {
                pd.SetGenericBinding(track.sourceObject, _victim.ac.anim);
            }
            if (track.streamName == "My Playable Track")
            {
                pd.SetGenericBinding(track.sourceObject, am);
            }
        }
        pd.Play(); 
    }   
    public void stPlayTimeline(string eventName,ActorManager _attacker,ActorManager _victim)
    {
        foreach (var track in pd.playableAsset.outputs)
        {
            if(track.streamName== "Animation Track1")
            {
                pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
            }
            if (track.streamName == "Animation Track2")
            {
                pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
            }
            if (track.streamName == "My Playable Track")
            {
                pd.SetGenericBinding(track.sourceObject, am);
            }
            pd.Play();

        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            pd.Play();
        }
    }
}
