using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHandle : IUserInput
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        attack = true;
            yield return new WaitForSeconds(3.0f);
        attack = false;//{
            yield return new WaitForSeconds(1.0f);
        StartCoroutine("Start");
        // }
    }

    private void Update()
    {
        //GoMove(Dright,Dup);
    }

}
