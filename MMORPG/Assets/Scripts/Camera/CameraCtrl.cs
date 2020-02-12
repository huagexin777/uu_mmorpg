using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraCtrl : MonoBehaviour
{


    [SerializeField] private Transform _cameraRotateAndFollow;      //���� ˮƽ��ת
    [SerializeField] private Transform _cameraUpAndDown;            //���� ��ֱ��ת
    [SerializeField] private Transform _cameraContainer;            //���� ��Ұ����


    private int _camSpeed_H = 100;
    private int _camSpeed_V = 100;
    private int _camSpeed_Z = 300;

    void Awake()
    {
        FingerEvent.OnFingerDragEvent += OnFingerDragEvent;
        FingerEvent.OnMouseScaleEvent += OnMouseScaleEvent;
        FingerEvent.OnPlayerClickGroundEvent += OnPlayerClickGroundEvent;
    }

   

    private void OnDestroy()
    {
        FingerEvent.OnFingerDragEvent -= OnFingerDragEvent;
        FingerEvent.OnMouseScaleEvent -= OnMouseScaleEvent;
        FingerEvent.OnPlayerClickGroundEvent -= OnPlayerClickGroundEvent;
    }

    #region ��ָ����

    /// <summary>
    /// ���� (��������)������Ұ��ת
    /// </summary>
    /// <param name="obj"></param>
    void OnFingerDragEvent(FingerDir obj)
    {
        switch (obj)
        {
            case FingerDir.Left:
                ControlViewRotate_H(-1);
                break;
            case FingerDir.Right:
                ControlViewRotate_H(1);
                break;
            case FingerDir.Up:
                ControlViewRotate_V(1);
                break;
            case FingerDir.Down:
                ControlViewRotate_V(-1);
                break;
        }
    }
  

    /// <summary>
    /// ����(ǰ����)������Ұ����
    /// </summary>
    /// <param name="obj"></param>
    void OnMouseScaleEvent(MouseScroll obj)
    {
        switch (obj)
        {
            case MouseScroll.Forward:
                ControlViewScale(1);
                break;
            case MouseScroll.Back:
                ControlViewScale(-1);
                break;
        }
    }

    /// <summary>
    /// ����(���)���ƽ�ɫ����
    /// </summary>
    void OnPlayerClickGroundEvent()
    {
        
    }

    #endregion





    void Update()
    {

        
    }

    public void ControlViewRotate_H(float args)
    {
        _cameraRotateAndFollow.Rotate(0, Time.deltaTime * args * _camSpeed_H, 0);
    }

    public void ControlViewRotate_V(float args)
    {
        _cameraUpAndDown.Rotate(Time.deltaTime * args * _camSpeed_V, 0, 0);
        _cameraUpAndDown.localEulerAngles = new Vector3(Mathf.Clamp(_cameraUpAndDown.localEulerAngles.x, 10, 65), 0, 0);
    }

    public void ControlViewScale(float args)
    {
        _cameraContainer.Translate(0, 0, Time.deltaTime * args * _camSpeed_Z);
        _cameraContainer.localPosition = new Vector3(0, 0, Mathf.Clamp(_cameraContainer.localPosition.z, -15, -1));
    }




    void OnDrawGizmos()
    {
        if (_cameraRotateAndFollow == null) { return; }

        //���� ֻ����������ֱ�۵Ŀ���
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_cameraRotateAndFollow.position, 15);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_cameraRotateAndFollow.position, 14);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_cameraRotateAndFollow.position, 13);
    }

}
