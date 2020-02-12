using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssetBundleLoader : IDisposable
{
    private AssetBundle bundle;

    public AssetBundleLoader(string path)
    {
        string fullPathName = LocalFileMgr.Instance.LocalFilePath + path;
        //���ڴ����м���
        bundle = AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(fullPathName));
    }

    /// <summary>
    /// ������Դ
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
    //Unity�е�Object��object������
    //�ٸ��򵥵����ӣ�
    //  Debug.Log(gameObject is Object);
    //  Debug.Log(gameObject is object);
    //  ǰ�߷���true������Ҳ����true��
    //  int num = 5;
    //  Debug.Log(num is Object);
    //  Debug.Log(num is object);
    //  ǰ�߷���false�����߷���true
    //--------------------------




    /// <summary>
    /// ��Դ�ͷ�
    /// </summary>
    public void Dispose()
    {
        if (bundle != null)
        {
            bundle.Unload(false);
        }
    }

}
