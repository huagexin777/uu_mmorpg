using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType 
{
    /// <summary>
    /// ��¼����
    /// </summary>
    LogOn,
    /// <summary>
    /// ��ɫѡ�����
    /// </summary>
    RoleSelect,
    /// <summary>
    /// ����
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
