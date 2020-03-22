using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 加载场景Ctrl
/// </summary>
public class SceneLoadingCtrl : MonoBehaviour
{

    #region 属性

    private Slider _slider;
    public Slider Slider
    {
        get
        {
            if (_slider == null)
            {
                _slider = transform.Find("BG/ProgressBG").GetComponent<Slider>();
            }
            return _slider;
        }
    }

    private Text progressTxt;
    public Text ProgressTxt
    {
        get
        {
            if (progressTxt == null)
            {
                progressTxt = transform.Find("BG/ProgressBG/progressTxt").GetComponent<Text>();
            }
            return progressTxt;
        }
    }

    #endregion

    //private
    AsyncOperation async = null;
    float amount = 0;


    private void Awake()
    {
        StartCoroutine(LoadingScene());
    }


    /// <summary>
    /// 加载场景
    /// </summary>
    public IEnumerator LoadingScene()
    {
        string sceneName = "";

        switch (SceneMgr.Instance.targetScene)
        {
            case SceneType.LogOn:
                sceneName = "GameSceneUI_LogOn";
                break;

            //地图场景
            case SceneType.SelectRole:
                sceneName = "Scene_SelectRole";
                break;
            case SceneType.ChangAn:
                sceneName = "GameScene_ChangAn";
                break;
        }

        //加载场景Pref
        if (SceneMgr.Instance.targetScene == SceneType.SelectRole || SceneMgr.Instance.targetScene == SceneType.ChangAn)
        {
            AssetBundleMgr.Instance.CreateAsyncObject(string.Format("Scene/{0}.unity3d" , sceneName).ToLower() , sceneName.ToLower()).OnLocalComplete =
            (Object Obj) =>
            {
                async = SceneMgr.Instance.LoadSceneAsync(sceneName , UnityEngine.SceneManagement.LoadSceneMode.Additive);
                async.allowSceneActivation = false;
            };
        }
        else
        {
            async = SceneMgr.Instance.LoadSceneAsync(sceneName , UnityEngine.SceneManagement.LoadSceneMode.Additive);
            async.allowSceneActivation = false;
            yield return async;
        }
    }


    void Update()
    {
        if (async == null) { return; }
        if (async.progress >= 0.98f)
        {
            async.allowSceneActivation = true;
            Slider.value = 1;
            amount = 100;
        }
        else
        {
            amount++;
            Slider.value = async.progress;
        }
        ProgressTxt.text = amount + "%";
    }
}
