using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim_MC : MonoBehaviour
{
    private Animator _anim;
    public Animator Anim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            return _anim;
        }
    }
    private int _run;
    private int _idleFight;
    private int _atk1;
    private int _atk2;
    private int _atk3;
    private int _hurt;
    private int _die;

    void Awake()
    {

        #region Init Anim State
        _run = Animator.StringToHash("IsRun");
        _idleFight = Animator.StringToHash("IsFight");
        _atk1 = Animator.StringToHash("ATK1");
        _atk2 = Animator.StringToHash("ATK2");
        _atk3 = Animator.StringToHash("ATK3");
        _hurt = Animator.StringToHash("IsHurt");
        _die = Animator.StringToHash("IsDie"); 
        #endregion 

    }

    void Update()
    {

    }

    public void Run()
    {
        Anim.SetBool(_run, true);
    }

    public void Idle()
    {
        Anim.SetBool(_run, false);
        Anim.SetBool(_idleFight, false);
    }

    /// <summary>
    /// ½øÈëÕ½¶·×´Ì¬
    /// </summary>
    public void FightIdle()
    {
        Anim.SetBool(_idleFight, true);
    }

    public void phpAttack1()
    {
        Anim.SetTrigger(_atk1);
    }

    public void phpAttack2()
    {
        Anim.SetTrigger(_atk2);
    }

    public void phpAttack3()
    {
        Anim.SetTrigger(_atk3);
    }

    public void Hurt()
    {
        Anim.SetTrigger(_hurt);
    }

    public void Die()
    {
        Anim.SetTrigger(_die);
    }

}
