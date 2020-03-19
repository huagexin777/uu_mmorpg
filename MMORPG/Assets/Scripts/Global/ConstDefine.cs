using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstDefine
{
    //自动登录-key
    //public const string Login_ID = "Login_ID";
    public const string Login_Account = "Login_Account";
    public const string Login_Password = "Login_Password";




    #region 登录

    /// <summary>
    /// 登录视图|Key=登录按钮
    /// </summary>
    public const string UILoginView_LoginBtn = "UILoginView_LoginBtn";
    /// <summary>
    /// 登录视图|Key=(goto)注册按钮
    /// </summary>
    public const string UILoginView_GoToRegisterBtn = "UILoginView_GoToRegisterBtn";

    #endregion

    #region 注册

    /// <summary>
    /// 注册视图|Key=注册按钮
    /// </summary>
    public const string UIRegisterView_RegisterBtn = "UIRegisterView_RegisterBtn";
    /// <summary>
    /// 注册视图|Key=返回按钮
    /// </summary>
    public const string UIRegisterView_BackBtn = "UIRegisterView_BackBtn";

    #endregion

    #region 消息框

    /// <summary>|Key==确定按钮
    /// 消息框视图
    /// </summary>
    public const string UIMessageView_EnsureBtn = "UIMessageView_EnsureBtn";

    #endregion

    #region 服务器列表

    /// <summary>
    ///  区服入口|Key=换区
    /// </summary>
    public const string UIGameServerEnterView_SwitchZoneBtn = "UIGameServerEnterView_SwitchZoneBtn";
    /// <summary>
    ///  区服入口|Key=返回
    /// </summary>
    public const string UIGameServerEnterView_BackBtn = "UIGameServerEnterView_BackBtn";
    /// <summary>
    ///  区服入口|Key=前往
    /// </summary>
    public const string UIGameServerEnterView_GoToBtn = "UIGameServerEnterView_GoToBtn";

    //------------------------------ 服务器,选中按钮--------------------------------

    /// <summary>
    ///  区服|Key=选中
    /// </summary>
    public const string UIGameServerView_SelectBtn = "UIGameServerView_SelectBtn";

    #endregion
}
