using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion",(object)anim.deltaPosition);//(object)转换多余
    }

}
