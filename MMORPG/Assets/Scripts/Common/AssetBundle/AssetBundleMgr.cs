using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleMgr : Singleton<AssetBundleMgr>
{


    void Start()
    {
        
    }


    void Update()
    {

    }

    /// <summary>
    /// 创建异步AssetBundle
    /// </summary>
    /// <param name="path">assetBundle路径</param>
    /// <param name="name">资源name</param>
    /// <returns></returns>
    public AssetBundleLoaderAsync CreateAsyncObject(string path,string name)
    {
        GameObject obj = new GameObject("AssetBundleLoaderAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatComponent<AssetBundleLoaderAsync>();
        async.Init(path,name);
        return async;
    }

}
