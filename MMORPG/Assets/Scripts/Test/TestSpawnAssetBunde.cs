using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnAssetBunde : MonoBehaviour
{


    void Start()
    {
        //ͬ��1
        byte[] bt = LocalFileMgr.Instance.GetBuffer(@"E:\Unity3D\Program\UU\MMORPG\AssetBundle\Android\Role\role_mainplayer.assetbundle");
        AssetBundle ab = AssetBundle.LoadFromMemory(bt);

        Instantiate(ab.LoadAsset("Role_MainPlayer") as GameObject);


        //ͬ��2




        //�첽
        //AssetBundleLoaderAsync async = AssetBundleMgr.Instance.CreateAsyncObject(@"Role\role_mainplayer.assetbundle", "Role_MainPlayer");
        //async.OnLocalComplete = OnLocalComplete;

    }

    void Update()
    {

    }

    void OnLocalComplete(Object obj)
    {
        Instantiate(obj);
    }

}
