using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressBarTool : MonoBehaviour
{
    //单例
    private static ProgressBarTool _instance;
    public static ProgressBarTool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ProgressBarTool>();
            }
            return _instance;
        }
    }

    public GameObject TargetGo;

    #region 属性

    private Slider _slider;
    public Slider Slider
    {
        get
        {
            if (_slider == null)
            {
                _slider = TargetGo.transform.Find("BG/ProgressBG").GetComponent<Slider>();
            }
            return _slider;
        }
    }
    private Text _txt;
    public Text Txt
    {
        get
        {
            if (_txt == null)
            {
                _txt = TargetGo.transform.Find("BG/ProgressBG/slider").GetComponent<Text>();
            }
            return _txt;
        }
    }

    private GameObject _bg;
    public GameObject BG
    {
        get
        {
            if (_bg == null)
            {
                _bg = TargetGo.transform.Find("BG").gameObject;
            }
            return _bg;
        }
    }

    #endregion

    
    void Start()
    {
        BG.SetActive(false);
    }

    void Update()
    {

    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneType"></param>
    public void LoadScene(SceneType sceneType)
    {
        transform.Find("BG").gameObject.SetActive(true);
        transform.SetAsLastSibling();

        string scenename = sceneType.ToString();
        AsyncOperation async = SceneManager.LoadSceneAsync(scenename);
        StartCoroutine(LoadScene(async));
    }

    /// <summary>
    /// 场景加载-协程
    /// </summary>
    /// <param name="async"></param>
    /// <returns></returns>
    IEnumerator LoadScene(AsyncOperation async)
    {
        async.allowSceneActivation = false;

        int num = 0;
        while (num < 100)
        {
            num++;
            _txt.text = num + "%";
            _slider.value = num / 100f;
            yield return null;
        }
        _slider.value = 1;
        async.allowSceneActivation = true;
    }
}
