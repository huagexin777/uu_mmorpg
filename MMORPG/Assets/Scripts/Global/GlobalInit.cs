
using UnityEngine;

using System.Net.NetworkInformation;


public class CurrentServer
{
    /// <summary>
    /// Socket连接
    /// <para>127.0.0.1</para>
    /// </summary>
    public string ip = "127.0.0.1";
    
    /// <summary>
    /// Socket 端口
    /// <para>默认 1001</para>
    /// </summary>
    public int port = 1001;
}

public class GlobalInit : MonoBehaviour
{
    /// <summary>
    /// 当前服务器
    /// </summary>
    public static CurrentServer currentServer = new CurrentServer();


    /// <summary>
    /// WWW-连接
    /// <para>http://127.0.0.1:7788/</para>
    /// </summary>
    public static string HttpIPAdress = "http://127.0.0.1:8080/";

    /// <summary>
    /// 账号Id
    /// </summary>
    public static int AccountId;

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
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.LogError("输出所有 PlayerPrefs.Key:");
            Debug.LogError(PlayerPrefs.GetString(ConstDefine.Login_Account));
            Debug.LogError(PlayerPrefs.GetString(ConstDefine.Login_Password));
        }
    }

    #region 获得时间戳

    /// <summary>
    /// 发送消息-时间戳
    /// </summary>
    void SendMsg_TimeStamp()
    {
        //Get去获得.
        NetWorkHttp.Instance.SendData("api/time", OnReturnTimeTamp, isPost: false, dict: null);
    }

    /// <summary>
    /// 获得时间戳-回调函数
    /// </summary>
    void OnReturnTimeTamp(RetValue ret)
    {
        if (!ret.HasError)
        {
            long.TryParse( ret.Value.ToString(),out ServerTime); //拿到,时间戳.
        }
        else
        {
            Debug.LogError("获取时间戳失败! msg:" + ret.ErrorMessage);
        }
    }

#endregion
}


