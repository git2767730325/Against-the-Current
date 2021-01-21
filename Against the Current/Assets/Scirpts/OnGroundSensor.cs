using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;


    private Vector3 pointBottom;
    private Vector3 pointUpper;
    private float radius;
    Collider[] cols;
    // Start is called before the first frame update
    void Awake()
    {
        radius = capcol.radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pointBottom = transform.position + Vector3.up * (radius-0.2f);
        pointUpper = transform.position + Vector3.up * (-radius+capcol.height);
        cols = Physics.OverlapCapsule(pointBottom, pointUpper, radius, LayerMask.GetMask("Ground"));
        if (cols.Length > 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
