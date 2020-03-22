using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGamerServerEnterView : UIWindowViewBase
{

    public override UIWindowCtrl.WindowType CurrentWindowType
    {
        get
        {
            return UIWindowCtrl.WindowType.GameServerEnter;
        }

        set
        {
            base.CurrentWindowType = value;
        }
    }

    
    private Text serverName;




    public override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < allTransList.Count; i++)
        {
            if (allTransList[i].name == "lblDefaultGameServer")
            {
                serverName = allTransList[i].GetComponent<Text>();
            }
        }
    }


    /// <summary>
    /// 设置服务器信息
    /// </summary>
    /// <param name="serverInfo"></param>
    public void SetServerInfo(string serverInfo) 
    {
        this.serverName.text = serverInfo;
    }

    public override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        if (go.name == "btnEnterGame")//前往（进入角色选择）
        {
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_GoToBtn);
        }
        else if (go.name == "btnSelectGameServer")//换区
        {
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_SwitchZoneBtn);
        }
        else if (go.name == "backBtn")//返回
        {
            //TODO 后续优化
            //UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_SwitchZoneBtn);

            //打开,登录窗口 
            //关闭,当前窗口
            UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Login);
            this.Close();
        }
    }



    //----------------------------- 公开方法 ---------------------------------

    /// <summary>
    /// 更新选中的服务器
    /// </summary>
    public void UpdateSelectingServer(string serverName)
    {
        this.serverName.text = serverName;
    }

}
