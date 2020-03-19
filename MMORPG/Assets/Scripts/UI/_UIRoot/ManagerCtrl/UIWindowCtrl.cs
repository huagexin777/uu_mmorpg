using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 窗体UI管理
/// </summary>
public class UIWindowCtrl : Singleton<UIWindowCtrl>
{
    /// <summary>
    /// 窗体类型
    /// </summary>
    public enum WindowType
    {
        None,
        /// <summary>
        /// 登录
        /// </summary>
        Login,
        /// <summary>
        /// 注册
        /// </summary>
        Register,
        /// <summary>
        /// 角色信息表
        /// </summary>
        RoleInfoList,
        /// <summary>
        /// 消息框
        /// </summary>
        Message,
        /// <summary>
        /// 区服入口
        /// </summary>
        GameServerEnter,
        /// <summary>
        /// 游戏服务器
        /// </summary>
        GameServer,

    }

    /// <summary>
    /// 位置类型
    /// </summary>
    public enum PositionType
    {
        RightTop,
        LeftTop,
        RightBottom,
        LeftBottom,
        Center,
    }

    /// <summary>
    /// 窗口显示类型
    /// </summary>
    public enum WindowShowType
    {
        Normal,
        /// <summary>
        /// 中心放大
        /// </summary>
        Center2Big,
        /// <summary>
        /// 从上下落
        /// </summary>
        FromTop,
        /// <summary>
        /// 从下上升
        /// </summary>
        FromBottom,
        /// <summary>
        /// 从左平移
        /// </summary>
        FromLeft,
        /// <summary>
        /// 从右平移
        /// </summary>
        FromRight,
    }

    /// <summary>
    /// 打开窗体
    /// </summary>
    public GameObject OpenWindow(WindowType type)
    {
        GameObject go = null;
        switch (type)
        {
            case WindowType.Login:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "LoginWindow", isCache:true);
                break;
            case WindowType.Register:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "RegisterWindow", isCache: true);
                break;
            case WindowType.RoleInfoList:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "RoleInfoList", isCache: true);
                break;
            case WindowType.Message:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "MessageWindow", isCache: true);
                break;
            case WindowType.GameServerEnter:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "GameServerEnter", isCache: true);
                break;
            case WindowType.GameServer:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "GameServer", isCache: true);
                break;
        }

        if (go == null) { return null; }

        UIWindowViewBase viewBase = go.GetComponentInChildren<UIWindowViewBase>();

        Transform parentContainer = null;
        switch (viewBase.positionType)
        {
            case PositionType.RightTop:
                parentContainer = UISceneCtrl.Instance.currentSceneUI.rootContainer_RT;
                break;
            case PositionType.LeftTop:
                parentContainer = UISceneCtrl.Instance.currentSceneUI.rootContainer_LT;
                break;
            case PositionType.RightBottom:
                parentContainer = UISceneCtrl.Instance.currentSceneUI.rootContainer_RB;
                break;
            case PositionType.LeftBottom:
                parentContainer = UISceneCtrl.Instance.currentSceneUI.rootContainer_LB;
                break;
            case PositionType.Center:
                parentContainer = UISceneCtrl.Instance.currentSceneUI.rootContainer_C;
                break;
        }

        go.transform.SetParent(parentContainer,false);
        //go.SetActive(false);

        //实现各种-窗体效果
        ShowWindowDoTween(go, viewBase.windowShowType,true);

        return go;
    }

    /// <summary>
    /// 关闭窗体
    /// </summary>
    public void CloseWindow(WindowType type)
    {
        //加载窗体类型
        GameObject go = null;
        switch (type)
        {
            case WindowType.Login:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "LoginWindow", isCache: true);
                break;
            case WindowType.Register:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "RegisterWindow", isCache: true);
                break;
            case WindowType.RoleInfoList:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "RoleInfoList", isCache: true);
                break;
            case WindowType.Message:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "MessageWindow", isCache: true);
                break;
            case WindowType.GameServerEnter:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "GameServerEnter", isCache: true);
                break;
            case WindowType.GameServer:
                go = ResourcesMgr.Instance.Load(ResourcesType.WindowsUI, "GameServer", isCache: true);
                break;
        }

        //对齐类型
        if (go == null) { return ; }
        UIWindowViewBase viewBase = go.GetComponentInChildren<UIWindowViewBase>();

        //实现各种-窗体效果
        ShowWindowDoTween(go, viewBase.windowShowType, false);
    }




    #region 显示窗体DoTween

    /// <summary>
    /// 显示窗体DoTween 
    /// </summary>
    void ShowWindowDoTween(GameObject obj, WindowShowType type, bool isOpen)
    {
        switch (type)
        {
            case WindowShowType.Normal:
                NormalShow(obj, isOpen);
                break;
            case WindowShowType.Center2Big:
                Center2Big(obj, isOpen);
                break;
            case WindowShowType.FromTop:
                FromTop(obj, isOpen);
                break;
            case WindowShowType.FromBottom:
                FromBottom(obj, isOpen);
                break;
            case WindowShowType.FromLeft:
                FromLeft(obj, isOpen);
                break;
            case WindowShowType.FromRight:
                FromRight(obj, isOpen);
                break;
        }
    }

    #endregion

    #region 效果集

    void NormalShow(GameObject obj,bool isOpen)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(isOpen);
    }

    void Center2Big(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.zero;
            obj.transform.DOScale(1, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.DOScale(0, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        obj.SetActive(isOpen);
    }

    void FromTop(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            obj.transform.localPosition = new Vector3(0,1500f,0);
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(new Vector3(0, 1500f, 0), 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        obj.SetActive(isOpen);
    }

    void FromBottom(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            obj.transform.localPosition = new Vector3(0, -1500f, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(new Vector3(0, -1500f, 0), 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        obj.SetActive(isOpen);
    }

    void FromLeft(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            obj.transform.localPosition = new Vector3(-2000f, 0, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(new Vector3(2000f, 0, 0), 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        obj.SetActive(isOpen);
    }

    void FromRight(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            obj.transform.localPosition = new Vector3(2000f, 0, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.DOLocalMove(new Vector3(2000f, 0, 0), 0.6f).SetEase(obj.GetComponent<UIWindowViewBase>().animationCurve);
        }
        obj.SetActive(isOpen);
    }

    #endregion



}
