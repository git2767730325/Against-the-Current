using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
public class ChangeLevelLayer : MonoBehaviour
{
    public bool canChange = false;

    //
    public DieOnLow dol;
    public GameObject oldObj;
    public GameObject newObj;
    public TimeComeBack atc;
    [Header("最终关卡/二阶段取消时光倒流")]
    public bool isTheLastLevel=false;
    public ShaderControl[] sc;
    public StateManager enemyState;
    public AgainstTheCurrent against;
    public TimeComeBack timeComeb;
    [Header("=============触发相关===========")]
    public Collider layerChangeTriger;
    //Scene scene;
    //public enum Level
    //{
    //    Level1,
    //    Level2,
    //    Level3,
    //    LevelA,
    //}
    //
    //public enum ChangeCondition
    //{
     //   DefeatALLEnemy,//构思过关条件
    //}

    public enum ChangeLayerCondition
    {
        TriggerSth,
        EnemyHPLess50p,
    }
    //public Level level = new Level();
    public ChangeLayerCondition condition = new ChangeLayerCondition();

    void Start()
    {
        //scene = SceneManager.GetActiveScene();
        //Debug.Log(scene.name);
    }


    // Update is called once per frame
    void Update()
    {
        if(condition==ChangeLayerCondition.EnemyHPLess50p)
            if(enemyState.Hp/enemyState.HpMax<0.5f)
            {
                //改变判断条件
                condition = ChangeLayerCondition.TriggerSth;
                //显示文本,枪消失,特效
                dol.DisplayText();
                //更换贴图
                foreach (var item in sc)
                {
                    item.ChangeSceneMat();
                }
                against.Disable();
                against.enabled = false;
                dol.Saving();
            }
        else
            if(enemyState.Hp / enemyState.HpMax<=0)
            {
                //通关~~~~~~
            }
        if(canChange)
        {
            newObj.SetActive(true);
            Destroy(this.gameObject, 3f);
            atc.enabled = true;
            timeComeb.enabled = false;
            oldObj.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!canChange && condition == ChangeLayerCondition.TriggerSth)
        {
            atc.enabled= false;
            canChange = true;
        }
    }
}
