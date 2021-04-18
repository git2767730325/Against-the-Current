using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHandle : IUserInput
{
    Rigidbody rig;
    private void Awake()
    {
        rig = this.GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
            //yield return new WaitForSeconds(3.0f);
        //attack = false;//{
            yield return new WaitForSeconds(1.0f);
        StartCoroutine("Start");
        // }
    }

}
