using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    public UIManager uim;
    public int itemID;
    public int minID;
    public int maxID;
    private void Awake()
    {
        uim = GameObject.FindWithTag("uim").GetComponent<UIManager>();
    }
    private void OnEnable()
    {
        itemID = Random.Range(minID, maxID);
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
