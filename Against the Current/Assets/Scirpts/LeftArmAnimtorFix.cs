using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimtorFix : MonoBehaviour
{
    ActorController ac;
    //修改左手的位置
    public Vector3 vecFix;
    private Animator anim;
    public Vector3 vecFixpos;
    public GameObject gbjP;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ac = transform.parent.GetComponent<ActorController>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Transform body = anim.GetBoneTransform(HumanBodyBones.Chest);
        //Transform s = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        if (ac != null && ac.CheckAnimatorTag("climb"))
        {
            body.eulerAngles += vecFix;
            anim.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(body.localEulerAngles));//赋值回去
        }
        else if(ac != null&&ac.CheckAnimatorTag("gun","Gun"))
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, gbjP.transform.position);
            //anim.SetLookAtWeight(1, 0.3f, 1, 1,1);
            //RaycastHit rayH;
            //if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out rayH, 200))
            //{
            //    //anim.SetLookAtPosition(rayH.point);

            //    Debug.Log(rayH.point);
            //}
            //Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out rayH,200);
            //Plane plane = new Plane(Vector3.forward, transform.position);
            //float rayDistance = 0f;
            //if(plane.Raycast(ray,out rayDistance))
            //{
            //}
        }
            //调手脚有空再做
            //anim.SetIKPosition(AvatarIKGoal.LeftHand, vecFixpos);
            //anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,0.8f);
    }
}
