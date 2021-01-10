using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerHandle playH;
    public float walkSpeed;
    [SerializeField]
    private Animator anim;
    private Rigidbody rig;
    private Vector3 moveVec;
    // Start is called before the first frame update
    private void Awake()
    {
        playH = GetComponent<PlayerHandle>();
        anim = model.GetComponent<Animator>();
        rig = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playH.Dmo > 0.01f) 
        anim.transform.forward = playH.Dvec;
        anim.SetFloat("forward", playH.Dmo);
        moveVec = playH.Dmo * playH.Dvec*walkSpeed;
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector3(moveVec.x,rig.velocity.y,moveVec.z);//y=0,避免重力失效
    }
}
