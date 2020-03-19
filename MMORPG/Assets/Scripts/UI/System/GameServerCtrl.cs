using System;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>
{
    private UIGamerServerEnterView serverEnterView; //����������
    private UIGameServerView serverView; //�������б�


    public GameServerCtrl()
    {
        AddBtnEventListener(ConstDefine.UIGameServerEnterView_SwitchZoneBtn, OnSwtichZoneBtn);
        AddBtnEventListener(ConstDefine.UIGameServerView_SelectBtn, OnSelectingBtn);
        AddBtnEventListener(ConstDefine.UIGameServerEnterView_GoToBtn, OnGoToBtn);
        AddBtnEventListener(ConstDefine.UIRegisterView_BackBtn, OnBackBtn);

    }


    #region ���������������� (GameServerEntry)

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="p"></param>
    void OnSwtichZoneBtn(object[] p)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["type"] = 0;

        //����
        NetWorkHttp.Instance.SendData("api/gameserver", OnSwtichZoneEvent, true, dict);
    }

    /// <summary>
    /// ��ȡҳ�� (�����ص�)
    /// <para>����-������PageIndex</para>
    /// </summary>
    void OnSwtichZoneEvent(RetValue obj)
    {
        if (obj.HasError)
        {
            Debug.LogError("����ʧ��!\n message:" + obj.ErrorMessage + "\t value:" + obj.Value);
        }
        else
        {
            List<RetGameServerPageEntity> lsit = JsonMapper.ToObject<List<RetGameServerPageEntity>>(obj.Value.ToString());
            //����-����������
            serverView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.GameServer).GetComponent<UIGameServerView>();
            serverView.UpdateServerPage(lsit);
        }
    }


    /// <summary>
    /// ������Ϸ
    /// </summary>
    void OnGoToBtn(object[] p)
    {
        //��������
        NetWorkSocket.Instance.Connect();
        NetWorkSocket.Instance.OnConncetSuccess += OnConncetSuccessCallBack;
    }

    /// <summary>
    /// ���ӳɹ�-�ص�
    /// </summary>
    void OnConncetSuccessCallBack() 
    {
        //���س���
        SceneManagerCtrl.Instance.Load(SceneType.RoleSelect);
    }


    /// <summary>
    /// ���ص�¼&ע��
    /// </summary>
    /// <param name="p"></param>
    private void OnBackBtn(object[] p)
    {
        serverEnterView.Close();
        AccountCtrl.Instance.OpenLogonView(false);
    }

    #endregion

    #region ��������������Item��ѡ��GameServerSelecting��


    /// <summary>
    /// ѡ��-������
    /// </summary>
    private void OnSelectingBtn(object[] p)
    {
        //�õ�,ret��.
        RetGameServerEntity ret = p[0] as RetGameServerEntity;
        //���µ�GamerServerEnter ��UI��ͼ�ϣ�
        serverEnterView.UpdateSelectingServer(ret.Name);
        //���µ�������
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            ["type"] = 2,
            ["userName"] = PlayerPrefInstance.GetString("userName"), //�õ���ǰ�˺�
            ["serverInfo"] = JsonMapper.ToJson(ret)
        };
        NetWorkHttp.Instance.SendData("api/gameserver", OnSelectingServerCallBack, true, dict);
    }

    /// <summary>
    /// ѡ���˷�����-�ص�
    /// </summary>
    private void OnSelectingServerCallBack(RetValue obj)
    {
        //����,ֻ�Ǹ��µ�������.��û�б�Ĳ�����.
        if (obj.HasError)
        {
            Debug.LogError("������Ϣ:" + obj.ErrorMessage);
        }
        else
        {
            serverView.Close();
        }
    }

    #endregion


    #region View-��


    /// <summary>
    /// ��-���������View
    /// </summary>
    public UIGamerServerEnterView OpenGameServerEnterView()
    {
        serverEnterView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.GameServerEnter).GetComponent<UIGamerServerEnterView>();
        return serverEnterView;
    }

    #endregion

}

