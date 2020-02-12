using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //��Ұ����
    private Transform _player;
    private Vector3 _offset;
    [SerializeField] private float _scrollSpeed = 3;
    [SerializeField] private float distance = 0;

    //��Ұ��Χ
    [SerializeField] private float _slideSpeed = 3;

    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(_player.position);
        _offset = transform.position - _player.position;
    }

    void LateUpdate()
    {
        //ȷ����������׼ȷ, ��֡��  ��λ transform.position��λ��
        transform.position = _offset + _player.position;  //���߸ı�Ķ��� offset�Ĵ�С

        //��Ұ����ת
        RangeView();

        //��Ұ����������ԶЧ��
        ScrollView();

    }



    void ScrollView()
    {
        distance = _offset.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed;
        distance = Mathf.Clamp(distance, 4, 16);
        _offset = _offset.normalized * distance;
    }

    void RangeView()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(_player.position, _player.up, _slideSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            transform.RotateAround(_player.position, transform.right, -_slideSpeed * Input.GetAxis("Mouse Y"));//Ӱ������������� һ����position һ����rotation
            float x = transform.eulerAngles.x;
            if (x < 10 || x > 80)
            {//��������Χ֮�����ǽ����Թ�λԭ���ģ���������ת��Ч 
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
            _offset = transform.position - _player.position;
        }
    }
}
