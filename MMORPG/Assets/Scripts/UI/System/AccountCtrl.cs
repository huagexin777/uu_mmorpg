using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

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
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int type = 1;
        string username = ui_loginView.inputFields[0].text.Trim();//用户名
        string password = ui_loginView.inputFields[1].text.Trim();//密码
        int channelId = 0;
        dict["type"] = type;
        dict["username"] = username;
        dict["password"] = password;
        dict["channelId"] = channelId;
        if (string.IsNullOrEmpty(username)) { Debug.LogError("用户名不能为空!"); return; }
        if (string.IsNullOrEmpty(password)) { Debug.LogError("密码不能为空!"); return; }

        string json = JsonMapper.ToJson(dict);
        NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress + "api/account", OnLoginBackEvent, true, json);
    }

    /// <summary>
    /// 登录回调
    /// </summary>
    private void OnLoginBackEvent(RetValue obj)
    {
        if (obj.HasError)
        {
            Debug.LogError("登录失败!\n message:" + obj.ErrorMessage + "\t value:" + obj.Value);
        }
        else
        {
            Debug.LogError("登录成功!\n message:" + obj.ErrorMessage + "\t value:" + obj.Value);
        }
    }



    #endregion


    #region 注册视图

    /// <summary>
    /// 注册按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnRegisterClick(object[] param)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int type = 0;
        string username = ui_registerView.inputFields[0].text.Trim();//用户名
        string password = ui_registerView.inputFields[1].text.Trim();//密码
        string repassword = ui_registerView.inputFields[2].text.Trim();//重复密码
        int channelId = 0;
        dict["type"] = type;
        dict["username"] = username;
        dict["password"] = password;
        dict["channelId"] = channelId;

        if (string.IsNullOrEmpty(username)) { Debug.LogError("用户名不能为空!"); return; }
        if (string.IsNullOrEmpty(password)) { Debug.LogError("密码不能为空!"); return; }
        if (string.IsNullOrEmpty(repassword)) { Debug.LogError("重新输入的密码不能为空!"); return; }
        if (password != repassword) { Debug.LogError("两次输入的密码不一致!"); return; }

        string json = JsonMapper.ToJson(dict);
        NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress + "api/account", OnRegisterBackEvent, true, json);
    }

    /// <summary>
    /// 注册按钮-回调
    /// </summary>
    private void OnRegisterBackEvent(RetValue args)
    {
        if (args.HasError)
        {
            Debug.LogError("注册失败!\n message:" + args.ErrorMessage + "\t value:" + args.Value);
        }
        else
        {
            //返回登录界面
            OpenLogonView();
            ui_registerView.Close();
            Debug.LogError("注册成功!");
        }
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
