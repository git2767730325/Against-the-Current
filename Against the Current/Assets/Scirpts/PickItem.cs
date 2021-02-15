using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    public UIManager uim;
    public int itemID;
    
    private void OnEnable()
    {
        itemID = Random.Range(39, 50);
    }
    public void SetFalse()
    {
        uim.pickItem = null;
        this.transform.parent.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer== LayerMask.NameToLayer("sensor"))
        {
            uim.pickItem=this.GetComponent<PickItem>();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer==LayerMask.NameToLayer("sensor"))
        {
            uim.pickItem = null;
        }
    }
}
