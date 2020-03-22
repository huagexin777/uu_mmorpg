using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 注册视图
/// </summary>
public class UIRegisterView : UIWindowViewBase
{
    private UIWindowCtrl.WindowType currentWindowType;
    public override UIWindowCtrl.WindowType CurrentWindowType 
    {
        get { return UIWindowCtrl.WindowType.Register; }
    }

    [HideInInspector] public List<InputField> inputFields = new List<InputField>(); 

    public override void OnAwake()
    {
        base.OnAwake();

        InputField[] temps = GetComponentsInChildren<InputField>();
        for (int i = 0; i < temps.Length; i++)
        {
            //用户名 & 密码 & 重复密码
            if (temps[i].name == "input_Account" || temps[i].name == "input_Pwd" || temps[i].name == "input_RePwd")
            {
                inputFields.Add(temps[i]);
            }
        }
    }

    public override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "RegisterBtn":
                UIDispatcher.Instance.Dispatcher(ConstDefine.UIRegisterView_RegisterBtn);
                break;
            case "BackBtn":
                UIDispatcher.Instance.Dispatcher(ConstDefine.UIRegisterView_BackBtn);
                break;
        }

        //如果是,关闭按钮.
        if (go.name.Equals("BackBtn", System.StringComparison.CurrentCultureIgnoreCase) ||
            go.name.Equals("CloseBtn", System.StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }


    public override void Close()
    {
        base.Close();
    }


}
