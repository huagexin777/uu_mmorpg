using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCtrl : SystemCtrlBase<MessageCtrl>
{
    private System.Action OnClickHandler; //����¼�

    private UIMessageView messageView;

    public MessageCtrl()
    {
        AddBtnEventListener(ConstDefine.UIMessageView_EnsureBtn, OnBtnEnsureClick);
    }

    private void OnBtnEnsureClick(object[] p)
    {
        messageView.Close();
        //todo: ��Ϣ��-ȷ����ť-����֮��.Ҫ��������.

    }

    public void Update() 
    {
    
    }

    /// <summary>
    /// ����Ϣ
    /// </summary>
    protected override void Show(string title,string msg) 
    {
        messageView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Message).GetComponent<UIMessageView>();
        messageView.title.text = title;
        messageView.content.text = msg;
    }



}
