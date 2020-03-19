using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


/// <summary>
/// 账号管理Ctrl
/// </summary>
public class AccountCtrl : SystemCtrlBase<AccountCtrl>
{
    private UILoginView ui_loginView;
    private UIRegisterView ui_registerView;

    private int accountId;
    private string username;
    private string password;

    public AccountCtrl()
    {
        //登录视图
        AddBtnEventListener(ConstDefine.UILoginView_LoginBtn, OnBtnLoginClick);
        AddBtnEventListener(ConstDefine.UILoginView_GoToRegisterBtn, OnBtnGoToRegisterClick);

        //注册视图
        AddBtnEventListener(ConstDefine.UIRegisterView_RegisterBtn, OnBtnRegisterClick);
        AddBtnEventListener(ConstDefine.UIRegisterView_BackBtn, OnBtnBackClick);

    }


    #region 》》》》登录

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
        username = ui_loginView.inputFields[0].text.Trim();//用户名
        password = ui_loginView.inputFields[1].text.Trim();//密码
        int channelId = 0;
        dict["type"] = type;
        dict["username"] = username;
        dict["password"] = password;
        dict["channelId"] = channelId;
        if (string.IsNullOrEmpty(username)) { Show("消息", "用户名不能为空!"); return; }
        if (string.IsNullOrEmpty(password)) { Show("消息", "密码不能为空!"); return; }

        NetWorkHttp.Instance.SendData("api/account", OnLoginBackEvent, true, dict);
    }

    /// <summary>
    /// 登录回调
    /// </summary>
    private void OnLoginBackEvent(RetValue ret)
    {
        Debug.LogError(ret.ErrorMessage + "\t value:" + ret.Value);
        if (ret.HasError)
        {
            Show("登录", "账号不存在!");
            Debug.LogError("登录失败!\n message:" + ret.ErrorMessage + "\t value:" + ret.Value);
        }
        else
        {
            //对返回retValue进行拆分.
            string[] json = ret.Value.ToString().Split('#');

            //保存到本地
            GlobalInit.AccountId = int.Parse(json[0]);
            AccountPref pref = new AccountPref()
            {
                accountId = GlobalInit.AccountId,
                userName = username,
                password = password,
            };
            PlayerPrefInstance.SetAccount(pref);

            //关闭登录界面
            ui_loginView.Close();
            //开启-服务器入口界面
            UIGamerServerEnterView serverEnterView = GameServerCtrl.Instance.OpenGameServerEnterView();
            //回调,当前account是否有
            RetGameServerEntity retEntity = JsonMapper.ToObject<RetGameServerEntity>(json[1]);
            if (retEntity != null)
            {
                serverEnterView.UpdateSelectingServer(retEntity.Name);
            }
        }
    }


    /// <summary>
    /// 快速登录
    /// </summary>
    public void QuitLoginOn()
    {
        //查看是否有本地账号
        if (PlayerPrefInstance.HasAccount(ConstDefine.Login_Account))
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            int type = 1;
            int channelId = 0;

            //本地有账号
            AccountPref pref = PlayerPrefInstance.GetAccount();
            if (pref != null)
            {
                dict["type"] = type;
                dict["username"] = pref.userName;
                dict["password"] = pref.password;
                dict["channelId"] = channelId;
                NetWorkHttp.Instance.SendData("api/account", OnQuitLoginBackEvent, true, dict);
            }
        }
    }

    /// <summary>
    /// 快速登录-回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuitLoginBackEvent(RetValue ret)
    {
        Debug.LogError("快速登录!\nErrorMessage:" + ret.ErrorMessage + "\t value:" + ret.Value);
        if (ret.HasError)
        {
            Show("快速登录", "登录失败!");
            OpenLogonView(false);

            //清除本地临时账号.
            PlayerPrefInstance.Clear(PrefType.Account);
        }
        else
        {
            //对返回retValue进行拆分.
            string[] json = ret.Value.ToString().Split('#');

            //更新 全局AccountId
            GlobalInit.AccountId = int.Parse(json[0]);

            ui_loginView.Close();
            //开启-服务器入口界面
            UIGamerServerEnterView serverEnterView = GameServerCtrl.Instance.OpenGameServerEnterView();
            //回调,当前account是否有
            RetGameServerEntity retEntity = JsonMapper.ToObject<RetGameServerEntity>(json[1]);
            if (retEntity != null)
            {
                serverEnterView.UpdateSelectingServer(retEntity.Name);
            }
        }
    }

    #endregion

    #region 》》》》注册


    /// <summary>
    /// 注册按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnBtnRegisterClick(object[] param)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int type = 0;
        username = ui_registerView.inputFields[0].text.Trim();//用户名
        password = ui_registerView.inputFields[1].text.Trim();//密码
        string repassword = ui_registerView.inputFields[2].text.Trim();//重复密码
        int channelId = 0;
        dict["type"] = type;
        dict["username"] = username;
        dict["password"] = password;
        dict["channelId"] = channelId;

        if (string.IsNullOrEmpty(username)) { Show("消息", "用户名不能为空!"); return; }
        if (string.IsNullOrEmpty(password)) { Show("消息", "密码不能为空!"); return; }
        if (string.IsNullOrEmpty(repassword)) { Show("消息", "重新输入的密码不能为空!"); return; }
        if (password != repassword) { Show("消息", "两次输入的密码不一致!"); return; }

        NetWorkHttp.Instance.SendData("api/account", OnRegisterBackEvent, true, dict);
    }

    /// <summary>
    /// 注册按钮-回调
    /// </summary>
    private void OnRegisterBackEvent(RetValue args)
    {
        if (args.HasError)
        {
            Show("注册", "注册失败!");
            Debug.LogError("message:" + args.ErrorMessage + "\t value:" + args.Value);
        }
        else
        {
            //关闭注册界面
            ui_registerView.Close();

            //保存到本地
            AccountPref pref = new AccountPref()
            {
                accountId = (int)args.Value,
                userName = username,
                password = password,
            };
            PlayerPrefInstance.SetAccount(pref);

            //进入.服务器入口
            GameServerCtrl.Instance.OpenGameServerEnterView();
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


    #region View-打开



    /// <summary>
    /// 打开-登录View
    /// </summary>
    public void OpenLogonView(bool isCloseBefore = true)
    {
        //默认生成-LoginWindow
        ui_loginView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Login).GetComponent<UILoginView>();
        //监听上----登录View的关闭
        ui_loginView.OnCloseBeforeViewEvent = (UIWindowCtrl.WindowType uiType) =>
        {
            //如果,当前关闭的是-登录View. 那么就打开注册View.
            if (uiType == UIWindowCtrl.WindowType.Login && isCloseBefore)
            {
                OpenRegisterView();
            }
        };
    }


    /// <summary>
    /// 打开-注册View
    /// </summary>
    public void OpenRegisterView(bool isCloseBefore = true)
    {
        //默认生成-LoginWindow
        ui_registerView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Register).GetComponent<UIRegisterView>();
        //监听上----注册View的关闭
        ui_registerView.OnCloseBeforeViewEvent = (UIWindowCtrl.WindowType uiType) =>
        {
            //如果,当前关闭的是-注册View. 那么就打开登录View.
            if (uiType == UIWindowCtrl.WindowType.Register && isCloseBefore)
            {
                OpenLogonView();
            }
        };
    }

    #endregion


    public override void Dispose()
    {
        base.Dispose();
        RemoveBtnEventListener("UILoginView_LoginBtn", OnBtnLoginClick);
    }

}
