using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 本地文件管理
/// </summary>
public class LocalFileMgr : Singleton<LocalFileMgr>
{


#if UNITY_EDITOR

    #if UNITY_STANDALONE_WIN
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/Window/";
    #elif UNITY_IPHONE
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/iOS/";
    #elif UNITY_ANDROID
        /// <summary>
        /// 根据平台类型拿到对应的包路径名
        /// <para>自读属性</para>
        /// </summary>
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/Android/";
    #endif

#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN
    public readonly string LocalFilePath = Application.persistentDataPath + "/../AssetBundle/";
#endif


    void Start()
    {
        
    }


    void Update()
    {

    }

    /// <summary>
    /// 读取本地文件到byte[]
    /// </summary>
    /// <returns></returns>
    public byte[] GetBuffer(string path)
    {
        byte[] buffer = null;

        //用于定义一个范围，在此范围的末尾将释放对象。
        //using 语句中使用的对象必须实现 IDisposable 接口.

        //(这里继承的父类)Singleton类里面实现了 IDisposable 接口.
        using (FileStream fs = new FileStream(path,FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }

}
