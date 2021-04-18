using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControl : MonoBehaviour
{
    public Material material;
    public Texture texture;
    private void Awake()
    {
        material = this.gameObject.GetComponent<MeshRenderer>().material;
    }
    private IEnumerator Start()
    {
        float indensty = material.GetFloat("_RangeS");
        /*if(indensty<5)
        {
            indensty += 0.1f;
        }
        else
        {
            indensty = 1f;
        }
        */
        //在脚本中计算消耗的是CPU
        indensty =5* Mathf.Abs( Mathf.Sin(Time.time/3));
        material.SetFloat("_RangeS",indensty);
        //Debug.Log(material.GetFloat("_RangeS"));
        //Debug.Log(material.GetFloat("_Rangea"));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("Start");
    }

        /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            material.SetTexture("_MainTex", texture);
            material.SetFloat("_rotateDir",-1);
        }
    }
        */
    public void ChangeSceneMat()
    {
        material.SetTexture("_MainTex", texture);
        material.SetFloat("_rotateDir", -1);
    }
}
