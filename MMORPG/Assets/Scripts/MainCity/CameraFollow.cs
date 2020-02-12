using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //视野缩放
    private Transform _player;
    private Vector3 _offset;
    [SerializeField] private float _scrollSpeed = 3;
    [SerializeField] private float distance = 0;

    //视野范围
    [SerializeField] private float _slideSpeed = 3;

    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(_player.position);
        _offset = transform.position - _player.position;
    }

    void LateUpdate()
    {
        //确保相机跟随的准确, 首帧就  定位 transform.position的位置
        transform.position = _offset + _player.position;  //两者改变的都是 offset的大小

        //视野的旋转
        RangeView();

        //视野的拉近和拉远效果
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

            transform.RotateAround(_player.position, transform.right, -_slideSpeed * Input.GetAxis("Mouse Y"));//影响的属性有两个 一个是position 一个是rotation
            float x = transform.eulerAngles.x;
            if (x < 10 || x > 80)
            {//当超出范围之后，我们将属性归位原来的，就是让旋转无效 
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
            _offset = transform.position - _player.position;
        }
    }
}
