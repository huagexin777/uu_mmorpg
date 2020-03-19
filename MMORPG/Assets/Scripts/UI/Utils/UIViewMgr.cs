using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIWindowCtrl;

public class UIViewMgr : Singleton<UIViewMgr>
{
    private Dictionary<WindowType, ISystemCtrl> dict = new Dictionary<WindowType, ISystemCtrl>();

    public UIViewMgr() 
    {
        dict.Add(WindowType.Login, AccountCtrl.Instance);
        dict.Add(WindowType.Register, AccountCtrl.Instance);
        dict.Add(WindowType.GameServerEnter, GameServerCtrl.Instance);
        dict.Add(WindowType.GameServer, GameServerCtrl.Instance);
    }

    /// <summary>
    /// ´ò¿ªÊÓÍ¼
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(WindowType type) 
    {
        dict[type].OpenView(type);
    }
}
