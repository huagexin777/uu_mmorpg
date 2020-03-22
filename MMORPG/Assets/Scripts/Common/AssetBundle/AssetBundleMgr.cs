using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleMgr : Singleton<AssetBundleMgr>
{

    /// <summary>
    /// 加载镜像
    /// </summary>
    public GameObject Load(string path , string name) 
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
    }

    /// <summary>
    /// 加载克隆物体
    /// </summary>
    public GameObject Clone(string path,string name) 
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return GameObject.Instantiate(loader.LoadAsset<GameObject>(name));
        }
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
