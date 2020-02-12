using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheThirdController : MonoBehaviour
{
    #region 属性

    private Rigidbody _rig;
    public Rigidbody Rig
    {
        get
        {
            if (_rig == null)
            {
                _rig = GetComponent<Rigidbody>();
            }
            return _rig;
        }
    }



    #endregion


    [SerializeField] float rotateSpeed = 3;
    [SerializeField] float moveSpeed = 3;


    void Start()
    {

    }


    float x, y;
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (Mathf.Abs(x) >= 0.01f || Mathf.Abs(y) >= 0.01f)
        {
            Move(x,y);
        }

    }

    void Move(float x , float y)
    {
        //1.控制旋转
        //Quaternion targetQua = Quaternion.Euler();
        Vector3 targetDir = new Vector3(x, 0, y);
        transform.rotation = Quaternion.LookRotation(targetDir);
        //tra = Quaternion.Lerp(transform.rotation, tempQua, Time.deltaTime * rotateSpeed);

        //2.控制位移
        Rig.velocity = targetDir * Time.deltaTime * moveSpeed;




    }

}
