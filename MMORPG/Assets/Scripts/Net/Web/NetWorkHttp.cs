using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetWorkHttp : MonoBehaviour {

    private static NetWorkHttp _instance;
    public static NetWorkHttp Instance
    {
        get
        {
            if (_instance ==null )
            {
                GameObject network = new GameObject("NetWorkHttp");
                _instance = network.GetOrCreatComponent<NetWorkHttp>();
                DontDestroyOnLoad(network);
            }
            return _instance;
        }
    }

    /// <summary>
    /// Web事件回调
    /// </summary>
    private Action<CallBackArgs> OnWebCallBackEvent;
    private CallBackArgs _callBackArgs;


    #region 属性

    private bool _isBusy = false;
    /// <summary>
    /// 是否繁忙 (用于防止多次点击发送)
    /// </summary>
    public bool IsBusy
    {
        get
        {
            return _isBusy;
        }
    }

    #endregion


    void Start()
    {
        //只需new一次.
        _callBackArgs = new CallBackArgs();
    }


    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="url">地址</param>
    /// <param name="callBackEvent">回调event</param>
    /// <param name="isPost">是否发送</param>
    /// <param name="json">json数据</param>
    public void SendData(string url,Action<CallBackArgs> callBackEvent , bool isPost = false, string json = "")
    {
        if (_isBusy) { return; }

        _isBusy = true;
        OnWebCallBackEvent = callBackEvent;


        if (isPost)
        {
            PostUrl(url, json);
        }
        else
        {
            GetUrl(url);
        }
    }



    #region GetUrl 得到url请求

    /// <summary>
    /// GetUrl请求
    /// </summary>
    /// <param name="url"></param>
    void GetUrl(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(Get(www));
    }


    IEnumerator Get(WWW www)
    {
        //等待Web服务器的反应 (等待下载\上传完毕)
        yield return www;  
        _isBusy = false;

        //错误信息不为空!(报错！)
        if (!string.IsNullOrEmpty(www.error))
        {
            if (OnWebCallBackEvent != null)
            {
                _callBackArgs.ErrorInfo = www.error;
                _callBackArgs.IsError = true;
                OnWebCallBackEvent(_callBackArgs);
            }
        }
        else
        {
            if (OnWebCallBackEvent != null)
            {
                _callBackArgs.IsError = false;
                _callBackArgs.Json = www.text;
                OnWebCallBackEvent(_callBackArgs);
            }
        }
    }



    #endregion


    #region PostUrl 发送请求

    /// <summary>
    /// PostUrl请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    void PostUrl(string url, string json)
    {
        //创建一个表单
        WWWForm wWWForm = new WWWForm();
        //给表单添加值
        wWWForm.AddField("jsonData", json);

        WWW www = new WWW(url, wWWForm);
        StartCoroutine(Post(www));
    }

    IEnumerator Post(WWW www)
    {
        yield return www;
        _isBusy = false;

        //错误信息不为空!(报错！)
        if (!string.IsNullOrEmpty(www.error))
        {
            if (OnWebCallBackEvent != null)
            {
                _callBackArgs.ErrorInfo = www.error;
                _callBackArgs.IsError = true;
                OnWebCallBackEvent(_callBackArgs);
            }
        }
        else
        {
            if (OnWebCallBackEvent != null)
            {
                _callBackArgs.IsError = false;
                _callBackArgs.Json = www.text;
                OnWebCallBackEvent(_callBackArgs);
            }
        }
    }

    #endregion

}

public class CallBackArgs
{

    /// <summary>
    /// 是否报错
    /// </summary>
    public bool IsError;

    /// <summary>
    /// 报错信息
    /// </summary>
    public string ErrorInfo;

    /// <summary>
    /// json数据信息
    /// </summary>
    public string Json;

}