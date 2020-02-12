﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 登录视图
/// </summary>
public class UILoginView : UIWindowViewBase
{

    public override UIWindowCtrl.WindowType CurrentWindowType { get { return UIWindowCtrl.WindowType.Login ; } }

    public override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "LoginBtn":
                UIDispatcher.Instance.Dispatcher(ConstDefine.UILoginView_LoginBtn);
                break;
            case "GoToRegisterBtn":
                UIDispatcher.Instance.Dispatcher(ConstDefine.UILoginView_GoToRegisterBtn);
                break;
        }


        //如果是,关闭按钮.
        if (go.name.Equals("BackBtn", System.StringComparison.CurrentCultureIgnoreCase) ||
            go.name.Equals("CloseBtn", System.StringComparison.CurrentCultureIgnoreCase)||
            go.name.Equals("GoToRegisterBtn", System.StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }


    public override void Close()
    {
        base.Close();

        UIWindowCtrl.Instance.CloseWindow(CurrentWindowType);

        if (OnCloseBeforeViewEvent != null)
        {
            OnCloseBeforeViewEvent(CurrentWindowType);
        }
    }

}