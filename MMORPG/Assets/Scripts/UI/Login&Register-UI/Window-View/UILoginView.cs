using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 登录视图
/// </summary>
public class UILoginView : UIWindowViewBase
{

    private UIWindowCtrl.WindowType currentWindowType;
    public override UIWindowCtrl.WindowType CurrentWindowType 
    {
        get { return UIWindowCtrl.WindowType.Login; }
    }


    public List<InputField> inputFields = new List<InputField>();


    public override void OnAwake()
    {
        base.OnAwake();

        InputField[] temps = GetComponentsInChildren<InputField>();
        for (int i = 0; i < temps.Length; i++)
        {
            //用户名 & 密码
            if (temps[i].name == "username-InputField" || temps[i].name == "password-InputField")
            {
                inputFields.Add(temps[i]);
            }
        }
    }

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
    }

}
