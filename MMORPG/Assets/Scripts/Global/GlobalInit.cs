
using UnityEngine;

using System.Net.NetworkInformation;



public class GlobalInit : MonoBehaviour
{

    /// <summary>
    /// WWW-连接
    /// </summary>
    public static string HttpIPAdress = "http://127.0.0.1:7788/";

    /// <summary>
    /// 用户名
    /// </summary>
    public static string UserName;

    /// <summary>
    /// 密码
    /// </summary>
    public static string Password;

    /// <summary>
    /// 服务器时间 (现实时间戳)
    /// </summary>
    public static long ServerTime;

    /// <summary>
    /// 当前服务器时间
    /// </summary>
    public static long CurrentServerTime { get { return ServerTime + (long) Time.realtimeSinceStartup; } }

    /// <summary>
    /// 设备ID (mac物理地址)
    /// </summary>
    public static string DeviceIdentifier 
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    /// <summary>
    /// 设备型号
    /// </summary>
    public static string DeviceModel 
    {
        get 
        {
            //苹果
#if UNITY_IPHONE && !UNITY_EDITOR
            return UnityEngine.iOS.Device.generation.ToString();
            //安卓
#elif UNITY_ANDROID
              return SystemInfo.deviceModel;
#elif UNITY_EDITOR_WIN
            return SystemInfo.deviceModel;
#endif
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //获得时间戳
        SendMsg_TimeStamp();
    }

#region 获得时间戳

    /// <summary>
    /// 发送消息-时间戳
    /// </summary>
    void SendMsg_TimeStamp()
    {
        //Get去获得.
        NetWorkHttp.Instance.SendData(HttpIPAdress + "/api/time", OnReturnTimeTamp, isPost: false, dict: null);
    }

    /// <summary>
    /// 获得时间戳-回调函数
    /// </summary>
    void OnReturnTimeTamp(RetValue ret)
    {
        if (!ret.HasError)
        {
            ServerTime = (long)ret.Value; //拿到,时间戳.
        }
        else
        {
            Debug.LogError("获取时间戳失败! msg:" + ret.ErrorMessage);
        }
    }

#endregion
}


