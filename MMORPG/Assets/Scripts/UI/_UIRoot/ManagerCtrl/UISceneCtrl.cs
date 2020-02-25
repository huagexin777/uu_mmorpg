using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UISceneCtrl : Singleton<UISceneCtrl>
{

    /// <summary>
    /// 场景类型
    /// </summary>
    public enum SceneType
    {
        /// <summary>
        /// 登录界面场景
        /// </summary>
        Logon,
        /// <summary>
        /// 加载场景
        /// </summary>
        Loading,
        /// <summary>
        /// 主城场景
        /// </summary>
        MainCity,
        /// <summary>
        /// 战斗场景
        /// </summary>
        FightScene,

    }

    /// <summary>
    /// 当前场景容器
    /// </summary>
    public UISceneViewBase currentSceneUI;

    /// <summary>
    /// 加载场景
    /// </summary>
    public GameObject LoadScene(SceneType type)
    {
        GameObject go = null;
        switch (type)
        {
            case SceneType.Logon:
                go = ResourcesMgr.Instance.Load(ResourcesType.SceneUI, "UIRoot_Login&Register", isCache: false);
                currentSceneUI = go.GetComponentInChildren<UISceneLogOnView>();
                break;
            case SceneType.Loading:
                go = ResourcesMgr.Instance.Load(ResourcesType.SceneUI, "RegisterWindow", isCache: false);
                break;
            case SceneType.MainCity:
                go = ResourcesMgr.Instance.Load(ResourcesType.SceneUI, "ServerListWindow", isCache: false);
                break;
        }
       
        return go;
    }
}
