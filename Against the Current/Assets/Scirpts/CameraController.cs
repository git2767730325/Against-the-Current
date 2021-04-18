﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public bool isAI;
    [Header("========  ui自动获取UIM的第一个子物体=======")]
    public UIManager uim;
    public Image lockImg;
    public float sensitivity=10f;//灵敏度
    public IUserInput ph;

    public bool lockState=false;

    private float eulerAnglesX;
    private GameObject playHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private Vector3 a;//缓存值
    //private Vector3 b;
    private GameObject camera;

    public Vector3 halfBoxCol;
    //private GameObject targetObject;
    [SerializeField]
    private LockTarget lockTarget;

    MyTimer yourtime=new MyTimer();
    //private Collider[] cols; 
    //private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        //恢复灵敏度
        uim = GameObject.FindWithTag("uim").GetComponent<UIManager>();
        if (GameManager.GM.mouseSensitivity > 5)
        {
            sensitivity = GameManager.GM.mouseSensitivity;
            if(uim!=null)
            if (uim.sensitivity != null)
                uim.sensitivity.value = sensitivity;
        }
        lockImg = GameObject.FindWithTag("uim").transform.GetChild(0).
            GetComponent<Image>();
        
        cameraHandle = transform.parent.gameObject;
        playHandle = cameraHandle.transform.parent.gameObject;
        ActorController ac = playHandle.GetComponent<ActorController>();
        model = ac.model;
        eulerAnglesX = 20;
        if (!isAI)
        {
            camera = Camera.main.transform.gameObject;//主相机,注意标签对
            Cursor.lockState = CursorLockMode.Locked;//锁定模式
            lockImg.enabled = false;
        }
        lockTarget = new LockTarget(null);
    }

    private void Update()
    {
        if(uim.sensitivity!=null)
        if ((uim.sensitivity.value) != sensitivity)
            sensitivity = uim.sensitivity.value;
        if (lockTarget.obj!= null)
        {
            if (!isAI)
            {
                lockImg.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position
                                + new Vector3(0, lockTarget.halfHeight, 0));
            }
            yourtime.Tick();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //lockState用于判断是否 锁定相机视角
        Vector3 modelEuler = model.transform.eulerAngles;
        //cameraHandle.transform.Rotate(Vector3.right,-ph.Jup*sensitivity);//负值造成同位角问题
        //if else判断解决旋转角度360问题,未改为localeulerAngles
        #region second project
        //if (cameraHandle.transform.eulerAngles.x<=70)
        //cameraHandle.transform.eulerAngles = new Vector3(Mathf.Clamp(cameraHandle.transform.eulerAngles.x,0, 40),0,0);//yz本来就是0,直接用取不到负数角度，由于同位角？
        //else
        //   cameraHandle.transform.eulerAngles = new Vector3(Mathf.Clamp(cameraHandle.transform.eulerAngles.x, 330, 360), 0, 0);
        #endregion
        //用加法解决问题,不需要获得caremehandle的角度
        //eulerAnglesX = cameraHandle.transform.eulerAngles.x;//错误

        if (lockTarget.obj != null)
        {//锁定下~~~~~~~~~~~~~~~~~~~~~~~~~
        if(Vector3.Distance(lockTarget.obj.transform.position,model.transform.position)>12.0f)
        {
                lockState = false;
                lockTarget.obj = null;
                if(!isAI)
                lockImg.enabled = false;
                return;
        }
            eulerAnglesX = Vector3.Distance(cameraHandle.transform.position, lockTarget.obj.transform.position);
            cameraHandle.transform.localEulerAngles = new Vector3(eulerAnglesX, 0, 0);//上下移动视角
            //看向
            if (!isAI)
            {
                camera.transform.LookAt(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight / 1.2f, 0));//距离修正视野高低
                playHandle.transform.forward = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z);
            }
            else//让AI锁定后能面向玩家
            {
                playHandle.transform.LookAt(new Vector3 (lockTarget.obj.transform.position.x,playHandle.transform.position.y,
                    lockTarget.obj.transform.position.z));
            }
            lockState = true;

            if(yourtime.state==MyTimer.State.IDLE)
                yourtime.StartTimer(0.3f);
            if (yourtime.state == MyTimer.State.FINISHED)
            {
                yourtime.state = MyTimer.State.IDLE;
                float alp = Random.Range(200, 255)-Vector3.Distance(lockTarget.obj.transform.position, model.transform.position)*15;//可以实现远离变浅色
                lockImg.color = new Color(1, 1, 1, alp / 255f);
            }
        }
        //非锁定
        else
            lockState=false;    
        if (!lockState)
        {
            if (!isAI)
            {
                playHandle.transform.Rotate(Vector3.up, ph.Jright * sensitivity * Time.fixedDeltaTime);//只有这个的话，物体也会跟着旋转
                eulerAnglesX -= -ph.Jup * sensitivity * Time.fixedDeltaTime;
                eulerAnglesX = Mathf.Clamp(eulerAnglesX, -16, 50);
                cameraHandle.transform.localEulerAngles = new Vector3(eulerAnglesX, 0, 0);//上下移动视角
                model.transform.eulerAngles = modelEuler;
                camera.transform.LookAt(cameraHandle.transform);
            }
        }
            if(!isAI)
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position,transform.position,ref a,0.14f);
        //camera.transform.eulerAngles = cameraHandle.transform.eulerAngles;
        //camera.transform.rotation = cameraHandle.transform.rotation;
        //camera.transform.position = transform.position;
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, transform.position, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log(hit.point);
                transform.position = hit.point;
                return;
            }
        }
        else
            transform.localPosition = new Vector3(0, 0, -2.7f);
    }
    public void LockUnLock()
    {

        //判断敌人是否在模型父物体的正前方（类似屏幕前方的效果）特定范围
        Collider[] cols=Physics.OverlapBox(model.transform.position+(model.transform.parent.forward*5f), halfBoxCol,model.transform.parent.rotation, isAI?LayerMask.GetMask("player"):LayerMask.GetMask("enemy"));
        //模型面向发出箱子判断是否有物体
        //Collider[] cols=Physics.OverlapBox(model.transform.position+(model.transform.forward*5f), halfBoxCol,model.transform.rotation, isAI?LayerMask.GetMask("player"):LayerMask.GetMask("enemy"));
        if (cols.Length > 0)
        {
            foreach (Collider col in cols)
            {
                if (lockTarget.obj == null || lockTarget.obj != col.transform.gameObject)//后面判断多余了
                {
                    lockTarget =new LockTarget(col.transform.gameObject);
                    if(!isAI)
                    lockImg.enabled = true;
                    lockState =true;
                    lockTarget.halfHeight = col.bounds.extents.y/2;
                    break;
                }
                //持续锁定，后面这个用不上了
                else if (col.transform.gameObject == lockTarget.obj)
                {
                    lockTarget.obj = null;
                    if(!isAI)
                    lockImg.enabled = false;
                    lockState = false;
                }
            }
        }
        else //没有敌人时解除锁定，并且面向模型前方
        {
            lockTarget.obj = null;
                    if(!isAI)
            lockImg.enabled = false;
            lockState = false;
            //没有目标就面向模型前方，如果模型在与父类对齐视角会发生不可预料的视角偏移(向视角前方)
            if (playHandle.GetComponent<ActorController>().GetLookForwardBool())
            {
                model.transform.parent.forward = model.transform.forward;
                model.transform.forward = model.transform.parent.forward;
            }
        }
    }

    public void CancelLock()
    {
        lockState = false;
        lockTarget.obj = null;
        if (!isAI)
            lockImg.enabled = false;
        lockState = false;
    }//取消锁定
    private class LockTarget//初始化类
    {
        public float halfHeight;
        public GameObject obj;

        public LockTarget(GameObject _obj)
        {
            obj = _obj;
        }

    }
}
