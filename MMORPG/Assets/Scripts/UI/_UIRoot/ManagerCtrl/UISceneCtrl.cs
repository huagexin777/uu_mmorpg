using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����UI����
/// </summary>
public enum SceneUIType
{
    LogOn,
    City,
}

public class UISceneCtrl : Singleton<UISceneCtrl>
{

    /// <summary>
    /// ��ǰ��������
    /// </summary>
    public UISceneViewBase currentSceneUI;


    /// <summary>
    /// ���س���UI
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
