using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AssetBundle的异步加载
/// </summary>
public class AssetBundleLoaderAsync : MonoBehaviour
{
    private string _fullName;
    private string _name;

    //协程请求
    private AssetBundleCreateRequest _request;
    private AssetBundle _bundle;

    //delegate
    public System.Action<Object> OnLocalComplete;

    public void Init(string path, string name)
    {
        _fullName = LocalFileMgr.Instance.LocalFilePath + path;
        _name = name;
    }

    void Start()
    {
        StartCoroutine(Load()); 
    }

    IEnumerator Load()
    {
        #region WWW读取

        //using (WWW www = new WWW(_fullName))
        //{
        //    yield return www;
        //    _bundle = www.assetBundle;

        //    if (OnLocalComplete != null)
        //    {
        //        OnLocalComplete(_bundle.LoadAllAssets(_name));
        //        Destroy(this.gameObject);
        //    }
        //}

        #endregion

        #region 使用流读取

        //从内存中去[异步]读取
        _request = AssetBundle.LoadFromMemoryAsync(LocalFileMgr.Instance.GetBuffer(_fullName));
        yield return _request;

        _bundle = _request.assetBundle;

        if (OnLocalComplete != null)//_bundle.isStreamedSceneAssetBundle
        {
            OnLocalComplete(_bundle.LoadAsset(_name));
            Destroy(this.gameObject);
        }

        #endregion


    }

    //销毁后,再去Unload();
    private void OnDestroy()
    {
        if (_bundle != null)
        {
            //AssetBundle.Unload(false):释放AssetBundle文件内存镜像
            //AssetBundle.Unload(true):释放AssetBundle文件内存镜像同时销毁所有已经Load的Assets内存对象
            _bundle.Unload(false);
            _fullName = null;
            _name = null;
        }
    }

}
