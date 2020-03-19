using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIWindowCtrl;

public class SystemCtrlBase<T> : IDisposable, ISystemCtrl where T : new()
{
    #region µ¥Àý

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    #endregion


    protected virtual void Show(string title, string msg)
    {
        MessageCtrl.Instance.Show(title, msg);
    }

    protected virtual void AddBtnEventListener(string key, DispatcherBase<UIDispatcher, string, object[]>.OnActionHandler onActionHandler)
    {
        UIDispatcher.Instance.AddBtnEventListener(key, onActionHandler);
    }
    protected virtual void RemoveBtnEventListener(string key, DispatcherBase<UIDispatcher, string, object[]>.OnActionHandler onActionHandler)
    {
        UIDispatcher.Instance.RemoveBtnEventListener(key, onActionHandler);
    }



    public virtual void Dispose()
    {

    }

    public virtual void OpenView(WindowType type)
    {
        
    }
}
