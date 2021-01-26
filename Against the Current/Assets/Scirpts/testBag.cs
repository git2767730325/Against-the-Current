using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testBag : MonoBehaviour
{
    public BagPanel bp;
    public Button addItemBtn;
    private void Awake()
    {
        addItemBtn.onClick.AddListener(AddItemBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddItemBtn()
    {
        bp.AddItem();
    }
}
