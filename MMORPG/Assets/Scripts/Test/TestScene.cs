using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestScene : MonoBehaviour
{

    
    void Start()
    {
        using (AssetBundleLoader abLoader = new AssetBundleLoader(@"role/role_main_player.assetbundle"))
        {
            GameObject obj = abLoader.LoadAsset<GameObject>("role_main_player");
            Instantiate(obj);
        }

        AssetBundleMgr.Instance.CreateAsyncObject(@"role\role_main_player.assetbundle", "role_main_player");
    }

    
    void Update()
    {

    }
}
