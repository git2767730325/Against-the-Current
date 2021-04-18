using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject enemy;
    public int passLevel = 1;
    void Update()
    {
        if(enemy!=null)
            if(!enemy.activeSelf)
            {
                enemy = null;
                Invoke("PassLevel",5f);
                Debug.Log("5s后传送");
            }
    }
    public void PassLevel()
    {
        if (passLevel == 1)
            GameManager.GM.passLevel1 = true;
        else if (passLevel == 2)
            GameManager.GM.passLevel2 = true;
        GameManager.GM.ChangeScene(1);
    }
}
