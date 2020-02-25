using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 资源加载类型
/// </summary>
public enum ResourcesType
{
    /// <summary>
    /// 窗体UI
    /// </summary>
    WindowsUI,
    /// <summary>
    /// 窗体UI_Item
    /// </summary>
    WindowsItemUI,
    /// <summary>
    /// 场景UI
    /// </summary>
    SceneUI,
    /// <summary>
    /// 角色_1
    /// </summary>
    Role_1,
    /// <summary>
    /// 特效
    /// </summary>
    Effect,

}

/// <summary>
/// 资源加载管理
/// </summary>
public class ResourcesMgr : Singleton<ResourcesMgr>
{
    /// <summary>
    /// 资源-hashTable
    /// </summary>
    private Hashtable hashtablePref = new Hashtable();


    /// <summary>
    /// 资源加载
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="path">资源路径</param>
    /// <param name="isCache">是否从缓存中加载</param>
    /// <returns></returns>
    public GameObject Load(ResourcesType type, string path, bool isCache = false)
    {
        StringBuilder sb = new StringBuilder();
        switch (type)
        {
            case ResourcesType.WindowsUI:
                sb.Append("UI/UI-Window/");
                break;
            case ResourcesType.WindowsItemUI:
                sb.Append("UI/UI-Window/UI-Item/");
                break;
            case ResourcesType.SceneUI:
                sb.Append("UI/UI-Scene/");
                break;
            case ResourcesType.Role_1:
                sb.Append("Prefab/Role/");
                break;
            case ResourcesType.Effect:
                sb.Append("Prefab/Effect/");
                break;
        }
        sb.Append(path);

        GameObject obj = null;
        //从缓存中加载
        if (isCache)
        {
            if (hashtablePref.ContainsKey(sb.ToString()))
            {
                obj = hashtablePref[sb.ToString()] as GameObject;
            }
            else
            {
                obj = Resources.Load<GameObject>(sb.ToString());
                obj = GameObject.Instantiate(obj);
                hashtablePref.Add(sb.ToString(), obj);
            }
        }
        else
        {
            obj = Resources.Load<GameObject>(sb.ToString());
            obj = GameObject.Instantiate(obj);
        }



        return obj;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        //释放,hashTable.
        hashtablePref.Clear();
        //不加载没有用到的资源 (也就是从缓存中删除)
        Resources.UnloadUnusedAssets();
    }

}
