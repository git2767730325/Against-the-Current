using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimtorFix : MonoBehaviour
{   
    //修改左手的位置
    public Vector3 vecFix;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Transform leftArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftArm.eulerAngles += vecFix;
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftArm.localEulerAngles));//赋值回去
    }
}
