using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2dTri : MonoBehaviour
{

    public ActorManager am;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ff");
        if (collision.tag == "bullet")
        {
            am.TryDoDamage();
            Bullets bl = collision.GetComponentInParent<Bullets>();
            if(bl!=null)
            {
                bl.ReturnPool(collision.gameObject);
                Debug.Log("ok");
            }
            Debug.Log("aaa"+collision.gameObject.activeSelf);
            //碰撞点特效
        }
    }
}
