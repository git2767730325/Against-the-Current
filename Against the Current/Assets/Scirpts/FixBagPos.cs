using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FixBagPos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 mousePos;
    Vector3 target;
    RectTransform rt;
    bool canMoveBagPanel = false;


    //
    //用于拖动UI
    //
    //


    void Start()
    {
        rt=transform.GetComponent<RectTransform>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveBagPanel)
        {
            target = (Input.mousePosition - mousePos);
            rt.position += target;
            mousePos = Input.mousePosition;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        rt.position += (-mousePos + Input.mousePosition);
        canMoveBagPanel = false;
        //mousePos = Input.mousePosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        canMoveBagPanel = true;
    }
}
