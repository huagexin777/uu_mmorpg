using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowViewBase : UIViewBase
{

    #region 视觉效果

    /// <summary>
    /// 窗体类型
    /// </summary>
    public UIWindowCtrl.WindowType windowType;

    /// <summary>
    /// 位置对齐类型
    /// </summary>
    public UIWindowCtrl.PositionType positionType = UIWindowCtrl.PositionType.Center;

    /// <summary>
    /// 窗体显示类型
    /// </summary>
    public UIWindowCtrl.WindowShowType windowShowType;

    /// <summary>
    /// 持续时间
    /// </summary>
    public float duration = 0.2f;

    /// <summary>
    /// 动画曲线
    /// <para>用于DoTween显示效果</para>
    /// </summary>
    public AnimationCurve animationCurve;

    #endregion


    ///// <summary>
    ///// 下一个要打开的窗体类型
    ///// </summary>
    //public UIWindowCtrl.WindowType nextOpenWindow = UIWindowCtrl.WindowType.None;


    ///// <summary>
    ///// 要打开的下一个窗体类型
    ///// </summary>
    //public UIWindowCtrl.WindowType nextWindow;

    /// <summary>
    /// 关闭之前窗体委托事件
    /// </summary>
    public System.Action<UIWindowCtrl.WindowType> OnCloseBeforeViewEvent;

    /// <summary>
    /// 当前窗体类型
    /// </summary>
    public virtual UIWindowCtrl.WindowType CurrentWindowType { get; }
    
    /// <summary>
    /// 打开窗口
    /// </summary>
    public virtual void Open(UIWindowCtrl.WindowType windowType)
    {
        UIWindowCtrl.Instance.OpenWindow(windowType);
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    public virtual void Close()
    {
        
    }
}
