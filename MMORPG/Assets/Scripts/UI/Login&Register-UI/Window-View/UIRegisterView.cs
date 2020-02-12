using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 注册视图
/// </summary>
public class UIRegisterView : UIWindowViewBase
{
    public override UIWindowCtrl.WindowType CurrentWindowType { get { return UIWindowCtrl.WindowType.Register; } }

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

        UIWindowCtrl.Instance.CloseWindow(CurrentWindowType);

        if (OnCloseBeforeViewEvent != null)
        {
            OnCloseBeforeViewEvent(CurrentWindowType);
        }
    }


}
