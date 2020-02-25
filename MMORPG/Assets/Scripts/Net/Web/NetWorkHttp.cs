using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetWorkHttp : MonoBehaviour
{
    #region 单例

    private static NetWorkHttp _instance;
    public static NetWorkHttp Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject network = new GameObject("NetWorkHttp");
                _instance = network.GetOrCreatComponent<NetWorkHttp>();
                DontDestroyOnLoad(network);
            }
            return _instance;
        }
    }

    #endregion


    /// <summary>
    /// Web事件回调
    /// </summary>
    private Action<RetValue> OnWebCallBackEvent;
    private RetValue retValue = new RetValue();  //new一次. 生成RetValue类.

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




    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="url">地址</param>
    /// <param name="callBackEvent">回调event</param>
    /// <param name="isPost">是否发送</param>
    /// <param name="json">json数据</param>
    public void SendData(string url, Action<RetValue> callBackEvent, bool isPost,Dictionary<string,object> dict)
    {
        if (_isBusy) { return; }

        _isBusy = true;
        OnWebCallBackEvent = callBackEvent;

        //web安全
        //http://127.0.0.1/api/account?username=aaa&pwd=***&cdId=mac(物理地址)&sign=aadfdf (签名=时间戳:cdId) &t=时间戳
        //mac(物理地址是具有唯一性); 签名(会一直变化)

        if (isPost)
        {
            if (dict != null)
            {
                //客户端标识符
                dict["deviceIdentifier"] = GlobalInit.DeviceIdentifier;

                //设备型号
                dict["deviceModel"] = GlobalInit.DeviceModel;

                //签名
                dict["sign"] = EncrypUntil.Md5(GlobalInit.CurrentServerTime + ":" + GlobalInit.DeviceIdentifier);

                //时间戳
                dict["t"] = GlobalInit.CurrentServerTime;
            }

            PostUrl(url, LitJson.JsonMapper.ToJson(dict));
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

        //www出现错误 ()
        if (!string.IsNullOrEmpty(www.error))
        {

            Debug.LogError("www.text: " + www.text);
        }
        else
        {
            //由于.事实是: www.error是否为空. 
            //返回信息都会写在www.text里面.
            RetValue retTemp = LitJson.JsonMapper.ToObject<RetValue>(www.text);
            if (OnWebCallBackEvent != null)
            {
                this.retValue = retTemp;
                OnWebCallBackEvent(retValue);
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
        wWWForm.AddField("", json);

        WWW www = new WWW(url, wWWForm);
        StartCoroutine(Post(www));
    }

    IEnumerator Post(WWW www)
    {
        yield return www;
        _isBusy = false;
      

        //www出现错误 ()
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("www.text: " + www.text);
        }
        else
        {
            //由于.事实是: www.error是否为空. 
            //返回信息都会写在www.text里面.
            RetValue retTemp = LitJson.JsonMapper.ToObject<RetValue>(www.text);
            if (OnWebCallBackEvent != null)
            {
                this.retValue = retTemp;
                OnWebCallBackEvent(retValue);
            }
        }

    }

    #endregion

}

/// <summary>
/// 返回值
/// <para>与webAccount 服务器交互</para>
/// </summary>
public class RetValue
{
    /// <summary>
    /// 是否报错
    /// </summary>
    public bool HasError;

    /// <summary>
    /// 报错信息
    /// </summary>
    public string ErrorMessage;

    /// <summary>
    /// 数据信息
    /// </summary>
    public object Value;
}