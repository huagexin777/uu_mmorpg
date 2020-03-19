using UnityEngine;
using System.Collections;

public class UISceneLogOnView : UISceneViewBase
{
    public override void OnStart()
    {
        StartCoroutine(Wait2QuitLogin());
    }

    IEnumerator Wait2QuitLogin() 
    {
        AccountCtrl.Instance.OpenLogonView(false);
        yield return new WaitForSeconds(1);
        //直接进入快速登录
        AccountCtrl.Instance.QuitLoginOn();
    }
    
}
