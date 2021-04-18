using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHp : MonoBehaviour
{
    public StateManager sm;
    void Start()
    {
        sm = this.GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
