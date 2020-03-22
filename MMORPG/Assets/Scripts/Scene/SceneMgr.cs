using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景类型
/// </summary>
public enum SceneType
{
    /// <summary>
    /// 登录界面
    /// </summary>
    LogOn,
    /// <summary>
    /// 加载界面
    /// </summary>
    Loading,
    /// <summary>
    /// 长安
    /// </summary>
    ChangAn,
    /// <summary>
    /// 角色选择场景
    /// </summary>
    SelectRole,
}

public class SceneMgr : MonoBehaviour
{

    #region 单例

    private static SceneMgr _instance;
    public static SceneMgr Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneMgr>();
            }
            return _instance;
        }
    }

    #endregion


    public SceneType targetScene;


    /// <summary>
    /// 载入-加载场景
    /// </summary>    
    /// <param name="type"></param>
    public void LoadingScene(SceneType type)
    {
        this.targetScene = type;

        //【加载场景】
        SceneManager.LoadScene("GameSceneUI_Loading");
    }


    /// <summary>
    /// 加载场景
    /// <para>异步</para>
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="sceneMode"></param>
    public AsyncOperation LoadSceneAsync(string sceneName , LoadSceneMode sceneMode)
    {
        return SceneManager.LoadSceneAsync(sceneName , sceneMode);
    }

}
