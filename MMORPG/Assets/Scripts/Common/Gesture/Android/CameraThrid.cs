using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraThrid : MonoBehaviour
{
    public static CameraThrid _instance;
    public static CameraThrid Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (CameraThrid)FindObjectOfType(typeof(CameraThrid));
            }
            return _instance;
        }
    }



    [Header("玩家手指滑动屏幕，相机的缩放速度")]
    public float Touch_ScaleSpeed = 0.5f; //玩家手指滑动屏幕，相机的缩放速度

    // 缩放系数
    public float distance = 6.0f;

    //双指触控，只进行旋转
    public bool isOnlyRotation = false;

    //限制镜头的缩放系数
    public float scale_min = 15.0f;//允许最大缩小倍数  9  ,6.0 
    public float scale_max = 4.4f;//允许最大放大倍数  3  ,1.0 
    public float scale_final = 2f;

    [Header("限制垂直轴，镜头旋转角度")]
    public float yMinLimit = 0.0f;
    public float yMaxLimit = 60.0f;

    // 记录上一次手机触摸位置判断用户是在做放大还是缩小手势
    private Vector2 oldPosition1 = new Vector2(0, 0);
    private Vector2 oldPosition2 = new Vector2(0, 0);

    [Header("切换相机，相机的自动跟随速度")]
    public float followSpeed = 2;

    private bool isUseScale = true;                                             //是否使用放大缩小功能
    public bool isFinalScale = false;                                          //是否,进入最终放大缩小功能.
    public bool isFinalScale_plus = false;

    [Header("锁定后，镜头旋转到目标的速度 ")]
    public float camLockToEnemyRotSpeed = 5;        //锁定到目标的旋转速度 


    #region 触摸-监测

    public GameObject cameraHandle;
    public GameObject _camera;
    [Header("手指触摸屏幕，镜头旋转的速度")]
    public float horizontalSpeed = 20.0f;
    public float verticalSpeed = 80.0f;
    public float camDampValue = 0.05f;

    private float temEularX;

    private Vector3 cameraDampVelocity;

    private GameObject CamFollowPos;
    private Transform BackViewPos;
    private Transform CenterPos;

    //摄像机挂载...
    [Header("摄像机(视野注视)挂载...")]
    public Transform BackViewPos_cam_end;

    GameObject tempRot;

    #endregion

    #region 属性

    // 主角
    [HideInInspector]
    public Transform _hero;
    private Transform _Hero
    {
        get
        {
            if (_hero == null)
            {
                if (GameObject.FindWithTag(Tag.Player) != null)
                {
                    _hero = GameObject.FindWithTag(Tag.Player).transform;
                }
            }
            return _hero;
        }
    }

    private Transform cameraFollow;
    public Transform CameraFollow
    {
        get
        {
            if (cameraFollow == null)
            {
                cameraFollow = GameObject.FindWithTag(Tag.Player).transform.Find("CamFollowPos").transform;
            }
            return cameraFollow;
        }
    }

    private Transform centerPlayer;
    public Transform CenterPlayer
    {
        get
        {
            if (centerPlayer == null)
            {
                centerPlayer = GameObject.FindWithTag(Tag.Player).transform.Find("CenterPlayer").transform;
            }
            return centerPlayer;
        }
    }


    #endregion

   
    void Awake()
    {

    }

    void Start()
    { 

    }
    
    void Update()
    {
        //控制相机与主角之间的距离
        HandleCamDistance();

        HandleLockToUnLockTime();

        if (Input.touchCount > 1)
        {
            //处理双指旋转
            HandleTouchForTowHandRot();
            //处理双指缩放
            HandleTouchForTwoHandScale();
        }
        else if (Input.touchCount == 1)
        {
            //处理单指旋转
            HandleTouchForOneHandRot();
        }
    }

    void FixedUpdate()
    {
        //处理-锁定或者解锁.
        LockOrUnlockCam();
    }



    #region 控制摄像机-距离

    public Transform camPos;
    /// <summary>
    /// 控制相机与主角之间的距离
    /// </summary>
    void HandleCamDistance()
    {
        //正常状态
        if (CamFollowPos != null && isFinalScale == false)
        {
            cameraHandle.transform.position = CamFollowPos.transform.position;
            _camera.transform.position = CamFollowPos.transform.position + (camPos.transform.position - CamFollowPos.transform.position).normalized * distance;

            Debug.DrawLine(CamFollowPos.transform.position, _camera.transform.position, Color.red);
        }
        //最终状态-放大
        if (isFinalScale)
        {
            //在放大过程中,相机也是需要跟随的.
            cameraHandle.transform.position = CamFollowPos.transform.position;
            if (Input.touchCount > 1)
            {
                Vector3 targetDir = (_camera.transform.position - BackViewPos_cam_end.position).normalized;
                _camera.transform.position = BackViewPos_cam_end.position + targetDir * distance;
            }

            Debug.DrawLine(BackViewPos_cam_end.transform.position, _camera.transform.position, Color.green);
        }
    }

    #endregion

    #region 设置非锁定状态，相机是否面向（有点类似于锁定状态）目标

    private bool isLocked = false;
    private bool isToLock = true;               //是否要锁定敌人（进行相机跟随）
    private bool isRotCamWhenLock = false;      //是否在锁定过程中旋转镜头
    private bool isLookAtNearestEnemy = false;  //是否在非锁定过程中旋转镜头
    bool isAutoLookAtEnemy = false;

    //设置相机看向目标
    public void SetLookAtNearestEnemyToTrue()
    {
        isLookAtNearestEnemy = true;
        isAutoLookAtEnemy = true;
    }

    //设置相机不看相目标
    public void SetLookAtNearestEnemyToFalse()
    {
        isAutoLookAtEnemy = false;
    }
    //解锁-锁定
    void LockOrUnlockCam()
    {
        if (isLocked)
        {
            if (isRotCamWhenLock == false)
            {
                //CameraLockToTarget();
                //CheckEnemyState();
            }
            else
            {
                //CameraUnLockToTarget();
            }
        }
        else
        {
            if (isOnlyRotation == true)
            {
                if (isLookAtNearestEnemy)
                {
                    isLookAtNearestEnemy = false;
                }
            }

            if (isAutoLookAtEnemy == true)
            {
                if (isLookAtNearestEnemy == false)
                {
                    isLookAtNearestEnemy = true;
                }
            }
        }
    }

    #endregion

    #region 设置相机解除锁定 缓动效果

    float time_LockToUnLock = 0;
    //设置无法旋转镜头的时间，由于镜头从锁定到非锁定这段过程中，不能限制镜头的旋转，
    //当玩家在这段期间旋转屏幕，镜头可能会偏移到地面下面，所以在这段期间应当无法旋转
    float maxTime_LockToUnLock = 0.5f;
    //用于控制解除相机锁定的镜头缓动效果
    void HandleLockToUnLockTime()
    {
        if (isStartLockToUnLock)
        {
            if (time_LockToUnLock <= maxTime_LockToUnLock)
            {
                time_LockToUnLock += Time.deltaTime;
                if (time_LockToUnLock >= maxTime_LockToUnLock)
                {
                    isStartLockToUnLock = false;
                    time_LockToUnLock = 0;

                    isFirstUnlock = false;
                }
            }
        }
    }

    //需要添加一个判断（即是否是刚刚解除锁定，如果是刚刚解除锁定，那么在镜头移动到目标点前，无法旋转镜头）
    private bool isStartLockToUnLock = false;
    //设置相机缓动效果
    public void SetToStartLockToUnLock()
    {
        time_LockToUnLock = 0;
        isStartLockToUnLock = true;
    }


    #endregion
    
    #region 处理触控 旋转和缩放
    bool isFirstUnlock = false;
    /// <summary>
    /// 处理单指旋转
    /// </summary>
    void HandleTouchForOneHandRot()
    {
        #region xxxx

        //是否,锁定.
        if (isStartLockToUnLock == true)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //过滤掉,是否点击在UI上.
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
            }

            Vector2 touchDeltaPosition = Vector2.zero;
            float rotX = 0;
            float rotY = 0;
            if (Input.GetTouch(0).phase == TouchPhase.Moved && isOnlyRotation == false)
            {
                //EnableRotWhenLock();
                //EnableRotWhenUnLock();


                touchDeltaPosition += Input.GetTouch(0).deltaPosition;
                rotY = touchDeltaPosition.x;
                rotX = touchDeltaPosition.y;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (isLocked)
                {
                    //1 秒后，将相机旋转到锁定⽬目标敌⼈人
                    //isHandleRecoverLock = true;
                }
                else
                {

                    //isHandleLockNearestEnemy = true;
                }
            }

            touchDeltaPosition = Vector2.zero;
            cameraHandle.transform.Rotate(Vector3.up, rotY * horizontalSpeed * Time.fixedDeltaTime);
            //物体x,对应镜头的上下.
            temEularX = cameraHandle.transform.localEulerAngles.x;
            temEularX -= rotX * verticalSpeed * Time.fixedDeltaTime;
            temEularX = Mathf.Clamp(temEularX, yMinLimit, yMaxLimit);
            cameraHandle.transform.eulerAngles = new Vector3(temEularX, cameraHandle.transform.eulerAngles.y, 0);
            if (distance <= scale_max)
            {
                Vector3 targetDir = (CenterPos.transform.position - _camera.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDir, CenterPos.transform.up);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, 0.1f);
                //_camera.transform.LookAt(cameraHandle.transform);
            }
            else
            {
                Vector3 targetDir = (cameraHandle.transform.position - _camera.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDir, cameraHandle.transform.up);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, 0.1f);
                //_camera.transform.LookAt(CenterPos.transform);
            }

        }
        #endregion
    }

    bool isRot_00 = false;
    bool isRot_01 = false;
    /// <summary>
    /// 处理双指旋转
    /// </summary>
    void HandleTouchForTowHandRot()
    {
        if (isOnlyRotation)
        {
            if ((Input.GetTouch(0).phase == TouchPhase.Began))
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    isRot_00 = false;
                }
                else
                {
                    isRot_00 = true;
                }
            }

            if ((Input.GetTouch(1).phase == TouchPhase.Began))
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                {
                    isRot_01 = false;
                }
                else
                {
                    isRot_01 = true;
                }
            }

            //=== 处理两个手指点击UI，移动一个手指，镜头会旋转问题
            if ((Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (isRot_01 == false)
                {
                    //1 秒后，将相机旋转到锁定目标敌人
                    //isHandleRecoverLock = true;
                    if (isLocked)
                    {
                        //1 秒后，将相机旋转到锁定目标敌人
                        //isHandleRecoverLock = true;
                    }
                    else
                    {
                        //1 秒后，将相机旋转到最近的目标敌人
                        //isHandleLockNearestEnemy = true;
                    }

                }
                isRot_00 = false;
            }

            if ((Input.GetTouch(1).phase == TouchPhase.Ended))
            {
                if (isRot_00 == false)
                {
                    //1 秒后，将相机旋转到锁定目标敌人
                    if (isLocked)
                    {
                        //1 秒后，将相机旋转到锁定目标敌人
                        //isHandleRecoverLock = true;
                    }
                    else
                    {
                        //1 秒后，将相机旋转到最近的目标敌人
                        //isHandleLockNearestEnemy = true;
                    }
                }

                isRot_01 = false;
            }

            if (isRot_00 == false && isRot_01 == false)
            {
                return;
            }

            //=====end===

            Vector2 touchDeltaPosition = Vector2.zero;
            float rotX = 0;
            float rotY = 0;

            //[这段代码用来处理双指旋转]
            //==== 处理旋转
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (isRot_00)
                {
                    if (isStartLockToUnLock == true)
                    {
                        return;
                    }

                    //EnableRotWhenLock();
                    //EnableRotWhenUnLock();

                    touchDeltaPosition += Input.GetTouch(0).deltaPosition;
                    rotY = touchDeltaPosition.x;
                    rotX = touchDeltaPosition.y;
                }
            }

            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (isRot_01)
                {
                    if (isStartLockToUnLock == true)
                    {
                        return;
                    }

                    //EnableRotWhenLock();
                    //EnableRotWhenUnLock();

                    touchDeltaPosition += Input.GetTouch(1).deltaPosition;
                    rotY = touchDeltaPosition.x;
                    rotX = touchDeltaPosition.y;
                }
            }

            touchDeltaPosition = Vector2.zero;

            //playerHandle.transform.Rotate(Vector3.up, rotY * horizontalSpeed * Time.fixedDeltaTime); 原代码
            cameraHandle.transform.Rotate(Vector3.up, rotY * horizontalSpeed * Time.fixedDeltaTime);

            //temEularX = cameraHandle.transform.eulerAngles.x;
            temEularX = cameraHandle.transform.localEulerAngles.x;
            temEularX -= rotX * verticalSpeed * Time.fixedDeltaTime;
            temEularX = Mathf.Clamp(temEularX, yMinLimit, yMaxLimit);
            cameraHandle.transform.eulerAngles = new Vector3(temEularX, cameraHandle.transform.eulerAngles.y, 0);
            if (distance <= scale_max)
            {
                Vector3 targetDir = (CenterPos.transform.position - _camera.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDir, CenterPos.transform.up);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, 0.1f);
                //_camera.transform.LookAt(CenterPos.transform);
            }
            else
            {
                Vector3 targetDir = (cameraHandle.transform.position - _camera.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDir, cameraHandle.transform.up);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, 0.1f);
                //_camera.transform.LookAt(cameraHandle.transform);
            }
        }
    }


    bool isScale_00 = false;
    bool isScale_01 = false;
    /// <summary>
    /// 处理双指缩放
    /// </summary>
    void HandleTouchForTwoHandScale()
    {
        if (isUseScale && isOnlyRotation == false)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    isScale_00 = false;
                    return;
                }
                isScale_00 = true;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                {
                    isScale_01 = false;
                    return;
                }
                isScale_01 = true;
            }

            //======处理一个手指点击不点击UI，一个手指点击UI 会放大问题======
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isScale_00 = false;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                isScale_01 = false;
            }

            if (isScale_00 == false && isScale_01 == false)
            {
                //1 秒后，将相机旋转到锁定目标敌人
                //isHandleRecoverLock = true;
                if (isLocked)
                {
                    //1 秒后，将相机旋转到锁定目标敌人
                    //isHandleRecoverLock = true;
                }
                else
                {
                    //1 秒后，将相机旋转到最近的目标敌人
                    //isHandleLockNearestEnemy = true;
                }

                return;
            }

            // 前两只手指触摸类型都为移动触摸
            if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                //当我们的distance处于,最终阶段.
                if ((distance > scale_final && distance < scale_max) || distance <= scale_final)
                {
                    isFinalScale = true;
                    // 计算出当前两点触摸点的位置
                    var newPos1 = Input.GetTouch(0).position;
                    var newPos2 = Input.GetTouch(1).position;
                    // 函数返回真为放大，返回假为缩小
                    if (isEnlarge(oldPosition1, oldPosition2, newPos1, newPos2))
                    {
                        if (isStartLockToUnLock == true)
                        {
                            return;
                        }

                        if (distance >= scale_final)
                        {
                            isFinalScale_plus = true;
                            distance -= Touch_ScaleSpeed * Time.smoothDeltaTime;
                        }
                    }
                    else
                    {
                        if (isStartLockToUnLock == true)
                        {
                            return;
                        }
                        if (distance <= scale_min)
                        {
                            isFinalScale_plus = false;
                            distance += Touch_ScaleSpeed * Time.smoothDeltaTime;
                        }
                    }
                    // 备份上一次触摸点的位置，用于对比
                    oldPosition1 = newPos1;
                    oldPosition2 = newPos2;
                }
                else
                {
                    isFinalScale = false;
                    // 计算出当前两点触摸点的位置
                    var tempPosition1 = Input.GetTouch(0).position;
                    var tempPosition2 = Input.GetTouch(1).position;
                    // 函数返回真为放大，返回假为缩小
                    if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                    {
                        if (isStartLockToUnLock == true)
                        {
                            return;
                        }
                        if (distance >= scale_final)
                        {
                            distance -= Touch_ScaleSpeed * Time.smoothDeltaTime;
                        }
                    }
                    else
                    {
                        if (isStartLockToUnLock == true)
                        {
                            return;
                        }

                        // 缩小系数返回18.5后不允许继续缩小
                        // 这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                        if (distance <= scale_min)
                        {
                            distance += Touch_ScaleSpeed * Time.smoothDeltaTime;
                        }
                    }
                    // 备份上一次触摸点的位置，用于对比
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                }

                //EnableRotWhenLock();
                //EnableRotWhenUnLock();
            }


        }
    }


    #endregion



    #region 实用函数

    // 函数返回真为放大，返回假为缩小
    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        // 函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));

        if (leng1 < leng2)
        {
            // 放大手势
            return true;
        }
        else
        {
            // 缩小手势
            return false;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    #endregion


}