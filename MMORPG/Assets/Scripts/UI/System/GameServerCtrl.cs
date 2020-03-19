using System;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>
{
    private UIGamerServerEnterView serverEnterView; //服务器进入
    private UIGameServerView serverView; //服务器列表


    public GameServerCtrl()
    {
        AddBtnEventListener(ConstDefine.UIGameServerEnterView_SwitchZoneBtn, OnSwtichZoneBtn);
        AddBtnEventListener(ConstDefine.UIGameServerView_SelectBtn, OnSelectingBtn);
        AddBtnEventListener(ConstDefine.UIGameServerEnterView_GoToBtn, OnGoToBtn);
        AddBtnEventListener(ConstDefine.UIRegisterView_BackBtn, OnBackBtn);

    }


    #region 》》》》区服换区 (GameServerEntry)

    /// <summary>
    /// 区服换区
    /// </summary>
    /// <param name="p"></param>
    void OnSwtichZoneBtn(object[] p)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["type"] = 0;

        //发送
        NetWorkHttp.Instance.SendData("api/gameserver", OnSwtichZoneEvent, true, dict);
    }

    /// <summary>
    /// 获取页码 (换区回调)
    /// <para>所有-服务器PageIndex</para>
    /// </summary>
    void OnSwtichZoneEvent(RetValue obj)
    {
        if (obj.HasError)
        {
            Debug.LogError("换区失败!\n message:" + obj.ErrorMessage + "\t value:" + obj.Value);
        }
        else
        {
            List<RetGameServerPageEntity> lsit = JsonMapper.ToObject<List<RetGameServerPageEntity>>(obj.Value.ToString());
            //开启-服务器界面
            serverView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.GameServer).GetComponent<UIGameServerView>();
            serverView.UpdateServerPage(lsit);
        }
    }


    /// <summary>
    /// 进入游戏
    /// </summary>
    void OnGoToBtn(object[] p)
    {
        //建立连接
        NetWorkSocket.Instance.Connect();
        NetWorkSocket.Instance.OnConncetSuccess += OnConncetSuccessCallBack;
    }

    /// <summary>
    /// 连接成功-回调
    /// </summary>
    void OnConncetSuccessCallBack() 
    {
        //加载场景
        SceneManagerCtrl.Instance.Load(SceneType.RoleSelect);
    }


    /// <summary>
    /// 返回登录&注册
    /// </summary>
    /// <param name="p"></param>
    private void OnBackBtn(object[] p)
    {
        serverEnterView.Close();
        AccountCtrl.Instance.OpenLogonView(false);
    }

    #endregion

    #region 》》》》服务器Item点选（GameServerSelecting）


    /// <summary>
    /// 选中-服务器
    /// </summary>
    private void OnSelectingBtn(object[] p)
    {
        //拿到,ret类.
        RetGameServerEntity ret = p[0] as RetGameServerEntity;
        //更新到GamerServerEnter （UI视图上）
        serverEnterView.UpdateSelectingServer(ret.Name);
        //更新到服务器
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            ["type"] = 2,
            ["userName"] = PlayerPrefInstance.GetString("userName"), //拿到当前账号
            ["serverInfo"] = JsonMapper.ToJson(ret)
        };
        NetWorkHttp.Instance.SendData("api/gameserver", OnSelectingServerCallBack, true, dict);
    }

    /// <summary>
    /// 选中了服务器-回调
    /// </summary>
    private void OnSelectingServerCallBack(RetValue obj)
    {
        //这里,只是更新到服务器.就没有别的操作了.
        if (obj.HasError)
        {
            Debug.LogError("错误信息:" + obj.ErrorMessage);
        }
        else
        {
            serverView.Close();
        }
    }

    #endregion


    #region View-打开


    /// <summary>
    /// 打开-服务器入口View
    /// </summary>
    public UIGamerServerEnterView OpenGameServerEnterView()
    {
        serverEnterView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.GameServerEnter).GetComponent<UIGamerServerEnterView>();
        return serverEnterView;
    }

    #endregion

}

