using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 同步加载 
/// <para>AssetBundle</para>
/// </summary>
public class AssetBundleLoader : IDisposable
{
    private AssetBundle bundle;

    public AssetBundleLoader(string path)
    {
        string fullPathName = LocalFileMgr.Instance.LocalFilePath + path;
        //从内存流中加载
        bundle = AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(fullPathName));
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        if (bundle == null) { return default(T); }
        return bundle.LoadAsset(name) as T;
    }


    //--------------------------
    //Unity中的Object和object的区别
    //举个简单的例子：
    //  Debug.Log(gameObject is Object);
    //  Debug.Log(gameObject is object);
    //  前者返回true，后者也返回true；
    //  int num = 5;
    //  Debug.Log(num is Object);
    //  Debug.Log(num is object);
    //  前者返回false，后者返回true
    //--------------------------




    /// <summary>
    /// 资源释放
    /// </summary>
    public void Dispose()
    {
        if (bundle != null)
        {
            bundle.Unload(false);
        }
    }

}
