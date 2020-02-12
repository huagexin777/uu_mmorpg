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
    /// ��Ұ��ת
    /// </summary>
    public static Action<FingerDir> OnFingerDragEvent;
    /// <summary>
    /// ��Ұ����
    /// </summary>
    public static Action<MouseScroll> OnMouseScaleEvent;
    /// <summary>
    /// ��ɫ�������
    /// </summary>
    public static Action OnPlayerClickGroundEvent;

    #endregion


    //��ָ
    private Vector2 FingerPos;
    private Vector2 oldFingerPos;
    private Vector2 FingerOffset;

    private Vector2 m_Dir; //��ָ�ƶ��ķ���
    private int FingerCount = -1;
    private int m_PreFinger = -1;

    //˫ָ
    private Vector2 oldFingerPos_dou1;
    private Vector2 oldFingerPos_dou2;
    private Vector2 FingerPos_dou1;
    private Vector2 FingerPos_dou2;
    
    private void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        #region ������

        //�Ŵ�
        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f) { OnMouseScaleEvent(MouseScroll.Forward); }
        //��С
        if (Input.GetAxis("Mouse ScrollWheel") < -0.01f) { OnMouseScaleEvent(MouseScroll.Back); }

        if (Input.GetAxis("Mouse X") < -0.01f) { OnFingerDragEvent(FingerDir.Left); }
        if (Input.GetAxis("Mouse X") > 0.01f) { OnFingerDragEvent(FingerDir.Right); }
        if (Input.GetAxis("Mouse Y") < 0.01f) { OnFingerDragEvent(FingerDir.Up); }
        if (Input.GetAxis("Mouse Y") > -0.01f) { OnFingerDragEvent(FingerDir.Down); }

        #endregion


        #region ���̿���

        if (Input.GetKey(KeyCode.A)) { OnFingerDragEvent(FingerDir.Left); }
        if (Input.GetKey(KeyCode.D)) { OnFingerDragEvent(FingerDir.Right); }
        if (Input.GetKey(KeyCode.W)) { OnFingerDragEvent(FingerDir.Up); }
        if (Input.GetKey(KeyCode.S)) { OnFingerDragEvent(FingerDir.Down); }

        if (Input.GetKey(KeyCode.Z)) { OnMouseScaleEvent(MouseScroll.Forward); }
        if (Input.GetKey(KeyCode.C)) { OnMouseScaleEvent(MouseScroll.Back); }

        #endregion
        
#elif UNITY_ANDROID || UNITY_IPHONE
       //��ָ (����)
        if (Input.touchCount == 1)
        {
            //��ָ����   (���������� .x .y ���ڸ���.��-x,-y���жԱȼ��㡣)
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //����ƫ��
                FingerPos = Input.GetTouch(0).position;

                //����
                if (FingerOffset.x > FingerOffset.y && FingerOffset.x > -FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Right);
                    }
                }
                //����
                else if (-FingerOffset.x > FingerOffset.y && FingerOffset.x < -FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Left);
                    }
                }
                //����
                else if (FingerOffset.x < FingerOffset.y && -FingerOffset.x < FingerOffset.y)
                {
                    if (OnFingerDragEvent != null)
                    {
                        OnFingerDragEvent(FingerDir.Up);
                    }
                }
                //����
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
            //��ָ̧��
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if (OnPlayerClickGroundEvent != null)
                {
                    OnPlayerClickGroundEvent();
                }
            }
        }

        //˫ָ����
        if (Input.touchCount == 2)
        {
            FingerPos_dou1 = Input.GetTouch(0).position;
            FingerPos_dou2 = Input.GetTouch(1).position;


            float dis1 = Vector2.Distance(FingerPos_dou1, FingerPos_dou2);
            float dis2 = Vector2.Distance(oldFingerPos_dou1, oldFingerPos_dou2);

            //��С
            if (dis2 > dis1)
            {
                OnMouseScaleEvent(MouseScroll.Forward);
            }
            //�Ŵ�
            else
            {
                OnMouseScaleEvent(MouseScroll.Back);
            }

            oldFingerPos_dou1 = FingerPos_dou1;
            oldFingerPos_dou2 = FingerPos_dou2;
        }

#endif


    }




    #region ��� FingerGesture

    void OnEnable()
    {
        ////Single
        //FingerGestures.OnFingerDown += OnFingerDown;                        //��ָ����
        //FingerGestures.OnFingerUp += OnFingerUp;                            //��ָ�ɿ�
        //FingerGestures.OnFingerDragBegin += OnFingerDragBegin;              //��ָ��ʼ����
        //FingerGestures.OnFingerDragMove += OnFingerDragMove;                //��ָһֱ����
        //FingerGestures.OnFingerDragEnd += OnFingerDragEnd;                  //��ָ��������

        ////Double
        //FingerGestures.OnTwoFingerDragBegin += OnTwoFingerDragBegin;        //˫ָ��ʼ����
        //FingerGestures.OnTwoFingerDragMove += OnTwoFingerDragMove;          //˫ָһֱ����
        //FingerGestures.OnTwoFingerDragEnd += OnTwoFingerDragEnd;            //˫ָ��������



        //FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;  //����
    }
    private void OnDisable()
    {
        //FingerGestures.OnFingerDown -= OnFingerDown;                        //��ָ����
        //FingerGestures.OnFingerUp -= OnFingerUp;                            //��ָ�ɿ�
        //FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;              //��ָ��ʼ����
        //FingerGestures.OnFingerDragMove -= OnFingerDragMove;                //��ָһֱ����
        //FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;                  //��ָ��������


        //FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;  //����
    }


    #region ��ָ���

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
            if (OnPlayerClickGroundEvent != null)//Ҫ����ϰ��
            {
                OnPlayerClickGroundEvent();
            }
        }
    }

    #endregion

    #region ��ָ����

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

    #region ˫������

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
