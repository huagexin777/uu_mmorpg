using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController_MC : MonoBehaviour
{
    public float _speed = 3;

    private CharacterController cc;
    private PlayerAnim_MC p_anim;
    private bool _isMove = false;
    private Vector3 _targetPos;

    //动画状态信息
    private AnimatorStateInfo mStateInfo;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        p_anim = GetComponent<PlayerAnim_MC>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position,5);
        Gizmos.DrawWireSphere(transform.position, 5);

    }

    int atkAmount = 0;
    void Update()
    {

        #region 射线 多物体检测
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] hitArray = null;
        //    hitArray = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Test"));  //使用位移 << 去锁定 层
        //    if (hitArray != null)
        //    {
        //        foreach (RaycastHit hit in hitArray)
        //        {
        //            Debug.Log("射线检测到了 : " + hit.collider.name);
        //        }
        //    }
        //}
        #endregion

        #region 球形 射线检测
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 5, 1 << LayerMask.NameToLayer("Test"));
        //if (colliders != null)
        //{
        //    foreach (Collider c in colliders)
        //    {
        //        Debug.Log(c.name);
        //    }
        //} 
        #endregion


        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }


#if UNITY_EDITOR
        //1.控制移动
        Move();
        


#elif UNITY_IOS
    Debug.Log("Unity iPhone");

#elif UNITY_ANDROID
    Debug.Log("Unity android");
#else
    Debug.Log("Any other platform");

#endif
    }


    /// <summary>
    /// 移动
    /// </summary>
    void Move()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isCast = Physics.Raycast(ray, out hitInfo);
            if (isCast)
            {
                _targetPos = hitInfo.point;
                _isMove = true;
            }
        }

        if (_isMove)
        {
            //Debug.Log(Vector3.forward);  //这样是局部坐标 永远等于(0,0,1)
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            cc.SimpleMove(forward * _speed);

            #region 直接朝向目标
            _targetPos.y = transform.position.y;//确保y轴一致。
            transform.LookAt(_targetPos);
            #endregion

            #region 朝向目标  (晃慢效果)
            //Quaternion _tar = Quaternion.LookRotation(_targetPos - transform.position);
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, _tar, Time.deltaTime * 10); 
            #endregion
            p_anim.Run();
            if (Vector3.Distance(transform.position, _targetPos) <= 0.1f)
            {
                p_anim.Idle();
                _isMove = false;
            }
        }
    }


    /// <summary>
    /// 攻击
    /// </summary>
    void Attack()
    {
        GlobalParameters.attack1_Combine++;
        p_anim.Anim.SetInteger("Attack1_Combine", GlobalParameters.attack1_Combine);
    }


    int mHitCount = 0;
    void Attack2()
    {
        //获取状态信息
        mStateInfo = p_anim.Anim.GetCurrentAnimatorStateInfo(0);
        //假设玩家处于Idle状态且攻击次数为0，则玩家依照攻击招式1攻击，否则依照攻击招式2攻击，否则依照攻击招式3攻击
        if (mStateInfo.IsName("Idle_Normal") && mHitCount == 0 && mStateInfo.normalizedTime > 0.50F)
        {
            p_anim.Anim.SetInteger("Attack1_Combine", 1);
            mHitCount = 1;
        }
        else if (mStateInfo.IsName("PhyAttack2 0") && mHitCount == 1 && mStateInfo.normalizedTime > 0.65F)
        {
            p_anim.Anim.SetInteger("Attack1_Combine", 2);
            mHitCount = 2;
        }
        else if (mStateInfo.IsName("PhyAttack3 0") && mHitCount == 2 && mStateInfo.normalizedTime > 0.70F)
        {
            p_anim.Anim.SetInteger("Attack1_Combine", 3);
            mHitCount = 3;
        }
    }

}
