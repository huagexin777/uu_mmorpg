using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 账号管理Ctrl
/// </summary>
public class AccountCtrl : Singleton<AccountCtrl>
{
    private UILoginView ui_loginView;

    private UIRegisterView ui_registerView;


    public AccountCtrl()
    {
        //登录视图
        UIDispatcher.Instance.AddBtnEventListener(ConstDefine.UILoginView_LoginBtn, OnBtnLoginClick);
        UIDispatcher.Instance.AddBtnEventListener(ConstDefine.UILoginView_GoToRegisterBtn, OnBtnGoToRegisterClick);

        //注册视图
        UIDispatcher.Instance.AddBtnEventListener(ConstDefine.UIRegisterView_RegisterBtn, OnBtnRegisterClick);
        UIDispatcher.Instance.AddBtnEventListener(ConstDefine.UIRegisterView_BackBtn, OnBtnBackClick);
    }

  


    /// <summary>
    /// 打开-登录View
    /// </summary>
    public void OpenLogonView()
    {
        //默认生成-LoginWindow
        ui_loginView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Login).GetComponent<UILoginView>();
        //监听上----登录View的关闭
        ui_loginView.OnCloseBeforeViewEvent = (UIWindowCtrl.WindowType uiType) => 
        {
            //如果,当前关闭的是-登录View. 那么就打开注册View.
            if (uiType == UIWindowCtrl.WindowType.Login)
            {
                OpenRegisterView();
            }
        };
    }


    /// <summary>
    /// 打开-注册View
    /// </summary>
    public void OpenRegisterView()
    {
        //默认生成-LoginWindow
        ui_registerView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Register).GetComponent<UIRegisterView>();
        //监听上----注册View的关闭
        ui_registerView.OnCloseBeforeViewEvent = (UIWindowCtrl.WindowType uiType) =>
        {
            //如果,当前关闭的是-注册View. 那么就打开登录View.
            if (uiType == UIWindowCtrl.WindowType.Register)
            {
                OpenLogonView();
            }
        };
    }



    #region 按钮事件监听

    #region 登录视图

    /// <summary>
    /// (goto)注册点击
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnGoToRegisterClick(object[] param)
    {
        OpenRegisterView();
    }

    /// <summary>
    /// 登录点击
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnLoginClick(object[] param)
    {
        Debug.LogError("登录点击");
    }

    #endregion


    #region 注册视图
    
    /// <summary>
    /// 注册按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnRegisterClick(object[] param)
    {
        Debug.LogError("注册点击");
    }

    /// <summary>
    /// 返回按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnBackClick(object[] param)
    {
        ui_loginView.Close();
    }


    #endregion

    #endregion




    public override void Dispose()
    {
        base.Dispose();
        UIDispatcher.Instance.RemoveBtnEventListener("UILoginView_LoginBtn", OnBtnLoginClick);
    }

}
