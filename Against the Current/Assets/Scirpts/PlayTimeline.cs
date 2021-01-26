using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class PlayTimeline : MonoBehaviour
{
    public PlayableDirector pd;
    public PlayableAsset pa;
    public Animator am1;
    public Animator am2;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        { 
            foreach(var track in pa.outputs)
            {
                if(track.streamName== "Animation Track1")
                {
                    pd.SetGenericBinding(track.sourceObject, am1);
                }
                else if(track.streamName == "Animation Track2")
                { 
                    pd.SetGenericBinding(track.sourceObject, am2);
                }
            }
            pd.Play();
            
        }
    }

    // Update is called once per frame
}
