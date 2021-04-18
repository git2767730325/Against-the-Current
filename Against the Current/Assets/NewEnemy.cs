using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public GameObject[] enemys;
    public Vector3[] enemysPos;
    private void Start()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemysPos[i] = enemys[i].transform.position;
        }
    }
    void Update()
    {
        for(int i=0;i<enemys.Length;i++)
        {
            if(!enemys[i].activeSelf)
            {
                //产生新敌人
                enemys[i].SetActive(true);
                enemys[i].transform.position= enemysPos[i];
                StateManager sm=enemys[i].GetComponent<StateManager>();
                sm.Hp = sm.HpMax;
                ActorController ac = enemys[i].GetComponent<ActorController>();
                ac.anim.Play("fall",0);
            }
        }
    }
}
