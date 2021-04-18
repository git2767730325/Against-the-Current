using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterParticle : MonoBehaviour
{
    private void OnParticleCollision(GameObject col)
    {
        //Debug.Log("sadasfasfasf");
        if(col.transform.tag!="Player")
        {
            Debug.Log("有碰撞u+" + col.name);
            ActorManager am = col.transform.GetComponent<ActorManager>();
            am.TryDoDamage(-7);
        }
    }
    private void OnParticleTrigger()
    {
        
    }
}
