using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;
    [Header("===========  音效轨道（音效文件）  ==========")]
    public AudioClip[] backGroundClips;
    public AudioClip audioClip;
    public AudioClip btnClip;
    public AudioClip inputTextClip;
    public AudioClip timeComeBackClip;
    public AudioClip atcDurationClip;
    public AudioClip againstTheCurrentClip;
    public AudioClip useDrugClip;
    public AudioClip gunReloadClip;
    public AudioClip gunShotClip;
    [Header("移动脚步,受伤")]
    public AudioClip moveClip;
    [Header("攻击防御跳跃翻滚等")]
    public AudioClip dunClip;
    public AudioClip hitClip;
    public AudioClip dieClip;
    public AudioClip attackVfxClip;
    public AudioClip[] jumpClips;
    [Header("===========  音效播放源  ==========")]
    public AudioSource backGroundaudioSource;
    public AudioSource playeraudioSource;
    public AudioSource enemyaudioSource;
    public AudioSource interfaceSource;
    public AudioSource vfxSource;
    public AudioSource UISource;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("删除了多余的音效管理者");
            return;
        }
        //添加对应音源
        backGroundaudioSource = gameObject.AddComponent<AudioSource>();
        playeraudioSource = gameObject.AddComponent<AudioSource>();
        enemyaudioSource = gameObject.AddComponent<AudioSource>();
        interfaceSource = gameObject.AddComponent<AudioSource>();
        vfxSource = gameObject.AddComponent<AudioSource>();
        UISource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        backGroundaudioSource.volume=GameManager.GM.voiceValue;
        vfxSource.loop = false;
        UISource.loop = false;
        SetBackGroundClip();
        PlayBackGroundSource();
    }

    public static void SetVoiceValue(float i)
    {
        Instance.backGroundaudioSource.volume = i;
    }


    public static void SetBackGroundClip(int i=0)
    {
        Instance.backGroundaudioSource.clip = Instance.backGroundClips[i];
        Instance.backGroundaudioSource.loop = true;
    }

    public static void SetAndPlayInterfaceAudio(int a)
    {
        if (a == 0)
            Instance.interfaceSource.clip = Instance.timeComeBackClip;
        else if (a == 1)
            Instance.interfaceSource.clip = Instance.againstTheCurrentClip;
        else if (a == 2)
            Instance.interfaceSource.clip = Instance.atcDurationClip;
        if (!Instance.interfaceSource.isPlaying)
            PlayInterfaceSource();
    }


    //设置ui按钮声音,自动开启声音
    public static void SetUIClipsAndPlay(int i = 0)
    {
        if (i == 0)
            Instance.UISource.clip = Instance.btnClip;
        else if (i == 1)
            Instance.UISource.clip = Instance.inputTextClip;
        PlayUISource();
    }
    //
    //播放相关的静态函数，需要记得设置循环,除了背景音乐，暂时自动调用
    //

    //游戏玩家声音，防御攻击等
    public static void SetPlayerSource(int i=0)
    {
        if (i == 0)
        {
            //移动
            Instance.playeraudioSource.clip = Instance.moveClip;
        }
        else if (i == 1)
        {
            //防御
            Instance.playeraudioSource.clip = Instance.dunClip;
        }

        else if (i == 2)
        {
            //受击
            Instance.playeraudioSource.clip = Instance.hitClip;
        }

        else if (i == 3)
        {
            //死亡
            Instance.playeraudioSource.clip = Instance.dieClip;
        }

        else if (i == 4)
        {
            //攻击特效
            Instance.playeraudioSource.clip = Instance.attackVfxClip;
        }
    }

    public static void PlayBackGroundSource()
    {
        Instance.backGroundaudioSource.Play();
    }
    public static void PlayPlayeraudioSource()
    {
        Instance.playeraudioSource.Play();
    }
    public static void PlayEnemyaudioSource()
    {
        Instance.enemyaudioSource.Play();
    }
    public static void PlayInterfaceSource()
    {
        Instance.interfaceSource.Play();
    }
    public static void SetAndPlayVfxSource(int i = 0)
    {
        if (i == 0)
        {
            //子弹发射
            Instance.vfxSource.clip = Instance.gunShotClip;
        
        }
        else if(i==1)
        {
            //上子弹
            Instance.vfxSource.clip = Instance.gunReloadClip;

        }
        else if(i==2)
        {
            //使用药品
            Instance.vfxSource.clip = Instance.useDrugClip;
        }
        else if(i==3)
        {
            //Instance.vfxSource.clip=Instance.
        }
        Instance.vfxSource.Play();
    }
    public static void PlayUISource()
    {
        Instance.UISource.Play();
    }

    public static void StopInterfaceSource()
    {
        if (Instance.interfaceSource.isPlaying)
            Instance.interfaceSource.Stop();
    }
    public static void StopPlayerAudioSource()
    {
        Instance.playeraudioSource.Stop();
    }

    public static void StopVfxAudioSource()
    {
        Instance.vfxSource.Stop();
    }

}
