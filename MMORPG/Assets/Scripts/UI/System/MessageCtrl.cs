using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCtrl : SystemCtrlBase<MessageCtrl>
{
    private System.Action OnClickHandler; //点击事件

    private UIMessageView messageView;

    public MessageCtrl()
    {
        AddBtnEventListener(ConstDefine.UIMessageView_EnsureBtn, OnBtnEnsureClick);
    }

    private void OnBtnEnsureClick(object[] p)
    {
        messageView.Close();
        //todo: 消息框-确定按钮-按下之后.要做的事情.

    }

    public void Update() 
    {
    
    }

    /// <summary>
    /// 打开消息
    /// </summary>
    protected override void Show(string title,string msg) 
    {
        messageView = UIWindowCtrl.Instance.OpenWindow(UIWindowCtrl.WindowType.Message).GetComponent<UIMessageView>();
        messageView.title.text = title;
        messageView.content.text = msg;
    }



}
