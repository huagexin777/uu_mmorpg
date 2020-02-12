using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AndroidTouch : MonoBehaviour
{

    [Range(1,100)] public float rotateSpeed = 3;

    private int isforward;//�����������ƶ�����
                          //��¼������ָ�ľ�λ��
    private Vector2 oposition1 = new Vector2();
    private Vector2 oposition2 = new Vector2();

    Vector2 m_screenPos = new Vector2(); //��¼��ָ������λ��



    void Start()
    {
        Input.multiTouchEnabled = true;//������㴥��
    }

    public float view_x = 0;
    public float view_y = 0;
    public float distance = 0;
    void Update()
    {
        #region ��ָ������ת

        if (Input.touchCount <= 0)
            return;
        //��ָ
        if (Input.touchCount == 1) //���㴥���ƶ������
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                m_screenPos = Input.touches[0].position;   //��¼��ָ�մ�����λ��
            if (Input.touches[0].phase == TouchPhase.Moved) //��ָ����Ļ���ƶ����ƶ������
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
        else if (Input.touchCount > 1)//��㴥��
        {
            //��¼������ָ��λ��
            Vector2 fingure_pos1 = new Vector2();
            Vector2 fingure_pos2 = new Vector2();

            //��¼��ָ��ÿ֡�ƶ�����
            Vector2 deltaFingure_pos1 = new Vector2();
            Vector2 deltaFingure_pos2 = new Vector2();

            for (int i = 0; i < 2; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Ended)
                    break;
                if (touch.phase == TouchPhase.Moved) //��ָ���ƶ�
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

                        //�ж����������Ӷ����������ǰ���ƶ���������Ч��
                        if (isEnlarge(oposition1, oposition2, deltaFingure_pos1, deltaFingure_pos2))
                        {
                            isforward = 1;
                        }
                        else
                        {
                            isforward = -1;
                        }
                    }
                    //��¼�ɵĴ���λ��
                    oposition1 = deltaFingure_pos1;
                    oposition2 = deltaFingure_pos2;
                }
                //�ƶ������
                //Camera.main.transform.Translate(isforward * Vector3.forward * Time.deltaTime * (Mathf.Abs(deltaDis2.x + deltaDis1.x) + Mathf.Abs(deltaDis1.y + deltaDis2.y)));
            }
        }

        #endregion



    }

    /// <summary>
    /// �����ж��Ƿ�Ŵ�
    /// </summary>
    /// <param name="oldP1">��һ�����ƻ���, ��ָ1λ��</param>
    /// <param name="oldP2">��һ�����ƻ���, ��ָ2λ��</param>
    /// <param name="newP1">�ڶ������ƻ���, ��ָ1λ��</param>
    /// <param name="newP2">�ڶ������ƻ���, ��ָ2λ��</param>
    /// <returns></returns>
    bool isEnlarge(Vector2 oldP1, Vector2 oldP2, Vector2 newP1, Vector2 newP2)
    {
        //����������һ�δ��������λ���뱾�δ��������λ�ü�����û�������
        float leng1 = Mathf.Sqrt((oldP1.x - oldP2.x) * (oldP1.x - oldP2.x) + (oldP1.y - oldP2.y) * (oldP1.y - oldP2.y));
        float leng2 = Mathf.Sqrt((newP1.x - newP2.x) * (newP1.x - newP2.x) + (newP1.y - newP2.y) * (newP1.y - newP2.y));
        if (leng1 < leng2)
        {
            //�Ŵ�����
            return true;
        }
        else
        {
            //��С����
            return false;
        }
    }

}

