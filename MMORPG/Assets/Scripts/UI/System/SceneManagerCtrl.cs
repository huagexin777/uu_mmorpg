using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType 
{
    /// <summary>
    /// 登录界面
    /// </summary>
    LogOn,
    /// <summary>
    /// 角色选择界面
    /// </summary>
    RoleSelect,
    /// <summary>
    /// 主城
    /// </summary>
    MainCity,

}
public class SceneManagerCtrl : Singleton<SceneManagerCtrl>
{

    public void Load(SceneType type) 
    {
        if (type == SceneType.LogOn)
        {
            SceneManager.LoadScene(SceneType.LogOn.ToString());
        }
        else if (type == SceneType.RoleSelect)
        {
            SceneManager.LoadScene(SceneType.RoleSelect.ToString());
        }
        else if (type == SceneType.MainCity)
        {
            SceneManager.LoadScene(SceneType.MainCity.ToString());
        }
    }

}
