using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FingerDir
{
    Left,
    Right,
    Up,
    Down
}

public enum MouseScroll
{
    Forward,
    Back
}

public class FingerEvent : MonoBehaviour
{

    public static FingerEvent _instance;
    public static FingerEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (FingerEvent)FindObjectOfType(typeof(FingerEvent));
            }
            return _instance;
        }
    }

    #region Event
    /// <summary>
    /// 视野旋转
    /// </summary>
    public static Action<FingerDir> OnFingerDragEvent;
    /// <summary>
    /// 视野缩放
    /// </summary>
    public static Action<MouseScroll> OnMouseScaleEvent;
    /// <summary>
    /// 角色点击地面
    /// </summary>
    public static Action OnPlayerClickGroundEvent;

    #endregion


    //单指
    private Vector2 FingerPos;
    private Vector2 oldFingerPos;
    private Vector2 FingerOffset;

    private Vector2 m_Dir; //手指移动的方向
    private int FingerCount = -1;
    private int m_PreFinger = -1;

    //双指
    private Vector2 oldFingerPos_dou1;
    private Vector2 oldFingerPos_dou2;
    private Vector2 FingerPos_dou1;
    private Vector2 FingerPos_dou2;
    
    private void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        #region 鼠标控制

        //放大
        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f) { OnMouseScaleEvent(MouseScroll.Forward); }
        //缩小
        if (Input.GetAxis("Mouse ScrollWheel") < -0.01f) { OnMouseScaleEvent(MouseScroll.Back); }

        if (Input.GetAxis("Mouse X") < -0.01f) { OnFingerDragEvent(FingerDir.Left); }
        if (Input.GetAxis("Mouse X") > 0.01f) { OnFingerDragEvent(FingerDir.Right); }
        if (Input.GetAxis("Mouse Y") < 0.01f) { OnFingerDragEvent(FingerDir.Up); }
        if (Input.GetAxis("Mouse Y") > -0.01f) { OnFingerDragEvent(FingerDir.Down); }

        #endregion


        #region 键盘控制

        if (Input.GetKey(KeyCode.A)) { OnFingerDragEvent(FingerDir.Left); }
        if (Input.GetKey(KeyCode.D)) { OnFingerDragEvent(FingerDir.Right); }
        if (Input.GetKey(KeyCode.W)) { OnFingerDragEvent(FingerDir.Up); }
        if (Input.GetKey(KeyCode.S)) { OnFingerDragEvent(FingerDir.Down); }

        if (Input.GetKey(KeyCode.Z)) { OnMouseScaleEvent(MouseScroll.Forward); }
        if (Input.GetKey(KeyCode.C)) { OnMouseScaleEvent(MouseScroll.Back); }

        #endregion
        
#elif UNITY_ANDROID || UNITY_IPHONE
       //单指 (滑动)
        if (Input.touchCount == 1)
        {
            //手指滑动   (当向量处于 .x .y 处于负轴.以-x,-y进行对比计算。)
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //计算偏移
                FingerPos = Input.GetTouch(0).position;

                //向右
                if (FingerOffset.x > FingerOffset.y && FingerOffset.x > -FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Right);
                    }
                }
                //向左
                else if (-FingerOffset.x > FingerOffset.y && FingerOffset.x < -FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Left);
                    }
                }
                //向上
                else if (FingerOffset.x < FingerOffset.y && -FingerOffset.x < FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Up);
                    }
                }
                //向下
                else
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Down);
                    }
                }

                oldFingerPos = FingerPos;
                FingerOffset = FingerPos - oldFingerPos;
            }
            //手指抬起
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if (OnPlayerClickGroundEvent != null)
                {
                    OnPlayerClickGroundEvent();
                }
            }
        }

        //双指缩放
        if (Input.touchCount == 2)
        {
            FingerPos_dou1 = Input.GetTouch(0).position;
            FingerPos_dou2 = Input.GetTouch(1).position;


            float dis1 = Vector2.Distance(FingerPos_dou1, FingerPos_dou2);
            float dis2 = Vector2.Distance(oldFingerPos_dou1, oldFingerPos_dou2);

            //缩小
            if (dis2 > dis1)
            {
                OnMouseScaleEvent(MouseScroll.Forward);
            }
            //放大
            else
            {
                OnMouseScaleEvent(MouseScroll.Back);
            }

            oldFingerPos_dou1 = FingerPos_dou1;
            oldFingerPos_dou2 = FingerPos_dou2;
        }

#endif


    }




    #region 插件 FingerGesture

    void OnEnable()
    {
        ////Single
        //FingerGestures.OnFingerDown += OnFingerDown;                        //手指按下
        //FingerGestures.OnFingerUp += OnFingerUp;                            //手指松开
        //FingerGestures.OnFingerDragBegin += OnFingerDragBegin;              //手指开始滑动
        //FingerGestures.OnFingerDragMove += OnFingerDragMove;                //手指一直滑动
        //FingerGestures.OnFingerDragEnd += OnFingerDragEnd;                  //手指结束滑动

        ////Double
        //FingerGestures.OnTwoFingerDragBegin += OnTwoFingerDragBegin;        //双指开始滑动
        //FingerGestures.OnTwoFingerDragMove += OnTwoFingerDragMove;          //双指一直滑动
        //FingerGestures.OnTwoFingerDragEnd += OnTwoFingerDragEnd;            //双指结束滑动



        //FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;  //长按
    }
    private void OnDisable()
    {
        //FingerGestures.OnFingerDown -= OnFingerDown;                        //手指按下
        //FingerGestures.OnFingerUp -= OnFingerUp;                            //手指松开
        //FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;              //手指开始滑动
        //FingerGestures.OnFingerDragMove -= OnFingerDragMove;                //手指一直滑动
        //FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;                  //手指结束滑动


        //FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;  //长按
    }


    #region 手指轻击

    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        Debug.Log("fingerIndex = " + fingerIndex + " ,fingerPos = " + fingerPos);

        m_PreFinger = 1;
    }
    void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHoldDown)
    {
        if (m_PreFinger == 1)
        {
            m_PreFinger = -1;
            if (OnPlayerClickGroundEvent != null)//要养成习惯
            {
                OnPlayerClickGroundEvent();
            }
        }
    }

    #endregion

    #region 单指滑动

    void OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
    {
        m_PreFinger = 2;
        oldFingerPos = fingerPos;
    }
    void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
    {
        m_PreFinger = 3;
        m_Dir = fingerPos - oldFingerPos;
        if (-m_Dir.x < m_Dir.y && m_Dir.y < m_Dir.x)  //Image Two (Verticle)Cube
        {//Turn Right
            if (OnFingerDragEvent != null)
            {
                OnFingerDragEvent(FingerDir.Right);
            }
        }
        else if (m_Dir.x < m_Dir.y && m_Dir.y < -m_Dir.x)//Positive
        {//Turn left
            if (OnFingerDragEvent != null)
            {
                OnFingerDragEvent(FingerDir.Left);
            }
        }
        else if (m_Dir.y > -m_Dir.x && m_Dir.y > m_Dir.x)//Image Two (Honrizontal)Cube
        {//Turn Up
            if (OnFingerDragEvent != null)
            {
                OnFingerDragEvent(FingerDir.Up);
            }
        }
        else
        {
            if (OnFingerDragEvent != null)
            {
                OnFingerDragEvent(FingerDir.Down);
            }
        }
    }
    void OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
    {
        m_PreFinger = 4;
    }

    #endregion

    #region 双击滑动

    void OnTwoFingerDragBegin(Vector2 fingerPos, Vector2 startPos)
    {

    }

    void OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
    {

    }

    void OnTwoFingerDragEnd(Vector2 fingerPos)
    {

    }


    #endregion

    void OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
    {

    }

    #endregion
}
