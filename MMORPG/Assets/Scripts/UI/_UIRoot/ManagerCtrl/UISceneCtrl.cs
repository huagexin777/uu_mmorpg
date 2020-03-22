using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景UI类型
/// </summary>
public enum SceneUIType
{
    LogOn,
    City,
}

public class UISceneCtrl : Singleton<UISceneCtrl>
{

    /// <summary>
    /// 当前场景容器
    /// </summary>
    public UISceneViewBase currentSceneUI;


    /// <summary>
    /// 加载场景UI
    /// </summary>
    /// <param name="type"></param>
    public void LoadSceneUI(SceneUIType type)
    {
        GameObject sceneUI_Go = null;
        switch (type)
        {
            case SceneUIType.LogOn:
                sceneUI_Go = ResourcesMgr.Instance.Load(ResourcesType.SceneUI , "UIRoot_Login&Register" , true);
                break;
            case SceneUIType.City:
                break;
        }

        currentSceneUI = sceneUI_Go.GetComponent<UISceneViewBase>();

    }
}
