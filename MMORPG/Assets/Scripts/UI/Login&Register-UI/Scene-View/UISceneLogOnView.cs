using UnityEngine;

public class UISceneLogOnView : UISceneViewBase
{
    public override void OnStart()
    {
        //旧方法
        ////默认生成-LoginWindow
        //GameObject go = UIWindowCtrl.Instance.OpenWindow(WindowType);

        AccountCtrl.Instance.OpenLogonView();
    }
    
}
