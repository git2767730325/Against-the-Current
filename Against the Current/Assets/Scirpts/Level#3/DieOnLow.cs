using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DieOnLow : MonoBehaviour
{
    public ActorController playerAc;
    public StateManager sm;
    public bool canRe = false;
    public bool isLoading = false;
    public Image loadingImg;
    public float colorAlpha = 0;
    //public GameObject sp;
    public GameObject textDia;
    public GameObject gun;
    public GameObject gunback;
    public GameObject gunVfx;
    public GameObject blackSky;
    private void Start()
    {
        Saving();
        StartCoroutine("WhenDie");
    }
    private void Update()
    {
        if(Time.timeScale==0.25f)
        {
            blackSky.SetActive(true);
        }
        else
        {
            if (blackSky.activeSelf)
                blackSky.SetActive(false);
        }
        if(sm.isDie)
        {
            if (!loadingImg.enabled)
            {
                loadingImg.enabled = true;
            }
            if (colorAlpha<0.95f)
                colorAlpha += 0.04f;
            loadingImg.color = new Color(1,1,1,colorAlpha);
        }
        else
        {
            if(loadingImg.enabled)
            {
                loadingImg.enabled = false;
            }
            if (colorAlpha > 0)
                colorAlpha -= 0.04f;
            loadingImg.color = new Color(1,1,1,colorAlpha);
        }
        if(canRe)
        {
            LoadLastSave();
            canRe = false;
        }
    }
    public void LoadLastSave()
    {
        if (isLoading)
            return;
        else
            isLoading = true;
        //读取存档
        Loading();
        Debug.Log("读取存档了");
    }
    public void Saving()
    {
        GameManager.GM.SaveByJson();
    }
    public void Loading()
    {
        GameManager.GM.LoadByJson(1);
        isLoading = false;
        playerAc.anim.Play("Move");
    }
    public void DisplayText()
    {
        StartCoroutine("display");
        //GameObject.Destroy(gun,0.1f);
        playerAc.canUseGun = false;
        //sp.SetActive(false);
        gunback.SetActive(false);
        //枪碎
        if(playerAc.anim.GetBool("isgun"))
        {
            playerAc.anim.SetBool("isgun",false);
            playerAc.am.wm.ChangeWeapon(false);
        }
        gunVfx.SetActive(true);
    }
    IEnumerator display()
    {
        textDia.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        gunVfx.SetActive(false);
        yield return new WaitForSeconds(3.2f);
        textDia.SetActive(false);
    }
    IEnumerator WhenDie()
    {
        yield return new WaitForSeconds(5f);
        if(sm.isDie)
        {
            canRe = true;
        }
        StartCoroutine("WhenDie");
    }
    private void OnTriggerEnter(Collider other)
    {
        playerAc.anim.Play("die");
    }
}
