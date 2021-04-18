using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testlookcamera : MonoBehaviour
{
    public GameObject enemy;
    public float lookHeight=3f;
    private Image npcHP;
    private Slider slider;
    private StateManager sm;
    public bool isTextLook = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isTextLook)
            return;
        sm = enemy.GetComponent<StateManager>();
        slider = GetComponent<Slider>();
        npcHP = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (isTextLook)
            return;
        if (slider.value!=sm.Hp/sm.HpMax)
            slider.value = sm.Hp / sm.HpMax;
        transform.position = enemy.transform.position+new Vector3(0,lookHeight,0);
        if (slider.value == 0)
        {
            npcHP.enabled = false;
        }
        else
            npcHP.enabled = true;
    }
}
