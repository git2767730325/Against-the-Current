using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandle : MonoBehaviour
{
    public string up;
    public string down;
    public string left;
    public string right;

    [Header("Signal")]
    public float Dup;
    public float Dright;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    [Header("Signal2")]
    public float Dmo;
    public Vector3 Dvec;

    [Header("Bool")]
    public bool inputEnable = true;




    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //目标速度值
        targetDup = (Input.GetKey(up) ? 1.0f : 0f) + (Input.GetKey(down) ? -1.0f : 0f);//记得括号，3元运算先后顺序不同
        targetDright = (Input.GetKey(right) ? 1.0f : 0f) + (Input.GetKey(left) ? -1.0f : 0f);
        //软控制，避免数值大乱
        if(!inputEnable)
        {
            targetDright = targetDup = 0.0f;//清空输入
        }

        //使用平滑达到目标速度，时间0.2s可变
        Dup = Mathf.SmoothDamp(Dup, targetDup,ref velocityDup, 0.2f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.2f);
        //方向和速度向量需要纠正
        Dmo = Mathf.Sqrt(Dup*Dup + Dright*Dright);
        Dvec = Dup * Vector3.forward + Dright * Vector3.right;
        //
    }
}
