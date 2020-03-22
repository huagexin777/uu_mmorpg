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
    /// ���÷�������Ϣ
    /// </summary>
    /// <param name="serverInfo"></param>
    public void SetServerInfo(string serverInfo) 
    {
        this.serverName.text = serverInfo;
    }

    public override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        if (go.name == "btnEnterGame")//ǰ���������ɫѡ��
        {
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_GoToBtn);
        }
        else if (go.name == "btnSelectGameServer")//����
        {
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_SwitchZoneBtn);
        }
        else if (go.name == "backBtn")//����
        {
            //TODO �����Ż�
            //UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerEnterView_SwitchZoneBtn);

            //��,��¼���� 
            //�ر�,��ǰ����
            UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Login);
            this.Close();
        }
    }



    //----------------------------- �������� ---------------------------------

    /// <summary>
    /// ����ѡ�еķ�����
    /// </summary>
    public void UpdateSelectingServer(string serverName)
    {
        this.serverName.text = serverName;
    }

}
