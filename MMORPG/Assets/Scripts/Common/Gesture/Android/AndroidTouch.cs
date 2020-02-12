using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AndroidTouch : MonoBehaviour
{

    [Range(1,100)] public float rotateSpeed = 3;

    private int isforward;//标记摄像机的移动方向
                          //记录两个手指的旧位置
    private Vector2 oposition1 = new Vector2();
    private Vector2 oposition2 = new Vector2();

    Vector2 m_screenPos = new Vector2(); //记录手指触碰的位置



    void Start()
    {
        Input.multiTouchEnabled = true;//开启多点触碰
    }

    public float view_x = 0;
    public float view_y = 0;
    public float distance = 0;
    void Update()
    {
        #region 单指控制旋转

        if (Input.touchCount <= 0)
            return;
        //单指
        if (Input.touchCount == 1) //单点触碰移动摄像机
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                m_screenPos = Input.touches[0].position;   //记录手指刚触碰的位置
            if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动，移动摄像机
            {
                float x = Input.touches[0].deltaPosition.x;
                float y = Input.touches[0].deltaPosition.y;
                view_x += x;
                view_y += y;
                y = Mathf.Clamp(y, 0,60);

                //transform.Rotate(Vector3.up, x * Time.deltaTime * rotateSpeed);
                transform.Rotate(Vector3.right, -y * Time.deltaTime * rotateSpeed);
                //transform.Rotate(new Vector3(-y, x, 0) * Time.deltaTime * rotateSpeed);
                //transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime, Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
            }
        }
        else if (Input.touchCount > 1)//多点触碰
        {
            //记录两个手指的位置
            Vector2 fingure_pos1 = new Vector2();
            Vector2 fingure_pos2 = new Vector2();

            //记录手指的每帧移动距离
            Vector2 deltaFingure_pos1 = new Vector2();
            Vector2 deltaFingure_pos2 = new Vector2();

            for (int i = 0; i < 2; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Ended)
                    break;
                if (touch.phase == TouchPhase.Moved) //手指在移动
                {

                    if (i == 0)
                    {
                        fingure_pos1 = touch.position;
                        deltaFingure_pos1 = touch.deltaPosition;
                    }
                    else
                    {
                        fingure_pos2 = touch.position;
                        deltaFingure_pos2 = touch.deltaPosition;

                        //判断手势伸缩从而进行摄像机前后移动参数缩放效果
                        if (isEnlarge(oposition1, oposition2, deltaFingure_pos1, deltaFingure_pos2))
                        {
                            isforward = 1;
                        }
                        else
                        {
                            isforward = -1;
                        }
                    }
                    //记录旧的触摸位置
                    oposition1 = deltaFingure_pos1;
                    oposition2 = deltaFingure_pos2;
                }
                //移动摄像机
                //Camera.main.transform.Translate(isforward * Vector3.forward * Time.deltaTime * (Mathf.Abs(deltaDis2.x + deltaDis1.x) + Mathf.Abs(deltaDis1.y + deltaDis2.y)));
            }
        }

        #endregion



    }

    /// <summary>
    /// 用于判断是否放大
    /// </summary>
    /// <param name="oldP1">第一段手势滑动, 手指1位置</param>
    /// <param name="oldP2">第一段手势滑动, 手指2位置</param>
    /// <param name="newP1">第二段手势滑动, 手指1位置</param>
    /// <param name="newP2">第二段手势滑动, 手指2位置</param>
    /// <returns></returns>
    bool isEnlarge(Vector2 oldP1, Vector2 oldP2, Vector2 newP1, Vector2 newP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        float leng1 = Mathf.Sqrt((oldP1.x - oldP2.x) * (oldP1.x - oldP2.x) + (oldP1.y - oldP2.y) * (oldP1.y - oldP2.y));
        float leng2 = Mathf.Sqrt((newP1.x - newP2.x) * (newP1.x - newP2.x) + (newP1.y - newP2.y) * (newP1.y - newP2.y));
        if (leng1 < leng2)
        {
            //放大手势
            return true;
        }
        else
        {
            //缩小手势
            return false;
        }
    }

}

