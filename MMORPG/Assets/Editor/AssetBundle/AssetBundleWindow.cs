using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


/// <summary>
/// 打包窗体
/// </summary>
public class AssetBundleWindow : EditorWindow
{

    private AssetBundleDAL dal;
    private string path;
    private List<AssetBundleEntity> entities;
    private Dictionary<string, bool> dict;


    private BuildTarget buildTarget = BuildTarget.StandaloneWindows;

    private int tagIndex = 0;
    private string[] arrTag = { "All", "Scene", "Role", "Effect", "Audio", "None" };
    private string[] arrBuildTarget = { "Android", "iOS", "Window" };


#if UNITY_STANDALONE_WIN
    private int buildTargetIndex = 2;
    private BuildTarget target = BuildTarget.StandaloneWindows;
#elif UNITY_IPHONE
    private int buildTargetIndex = 1;
    private BuildTarget target =  BuildTarget.iOS;
#elif UNITY_ANDROID
    private int buildTargetIndex = 0;
    private BuildTarget target = BuildTarget.Android;
#endif

    private void OnEnable()
    {
        Debug.Log("Application.dataPath (当前工程路径/Assets)：" + Application.dataPath);
        Debug.Log("Application.persistentDataPath (持久数据路径)：" + Application.persistentDataPath);
        Debug.Log("Application.streamingAssetsPath(流资源的路径)：" + Application.streamingAssetsPath);
        Debug.Log("Application.temporaryCachePath(临时缓存路径)：" + Application.temporaryCachePath);

        path = Application.dataPath + @"\Editor\AssetBundle\AssetBundleConfig.xml";
        dal = new AssetBundleDAL(path); //把路径交给dal来加载.(调用他的内部方法)
        entities = dal.GetList();

        dict = new Dictionary<string, bool>();
        for (int i = 0; i < entities.Count; i++)
        {
            dict.Add(entities[i].Num, true);
        }
        
    }


    //每次初始化的时候,已经就加载好了.资源.
    public AssetBundleWindow()
    {





    }

    void OnGUI()
    {
        //if (entities == null)
        //{
        //    return;
        //}

        #region 按钮行

        //开始一个水平组.
        GUILayout.BeginHorizontal("box");
        
        //选定Tag
        tagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (GUILayout.Button("标签类型", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectionTagCallBack;
        }

        //选定打包平台
        buildTargetIndex = EditorGUILayout.Popup(buildTargetIndex, arrBuildTarget,GUILayout.Width(100));
        if (GUILayout.Button("打包平台", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectTargetCallBack;
        }

        //确定打包
        if (GUILayout.Button("确定打包", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectAssetBundleCallBack;
        }

        //清空AssetBundle包
        if (GUILayout.Button("清空AB包", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectClearCallBack;
        }

        GUILayout.EndHorizontal();
        #endregion

        #region 内容标题行

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("---::", GUILayout.Width(100));
        GUILayout.Label("包名:", GUILayout.Width(150));
        GUILayout.Label("标记:", GUILayout.Width(100));
        GUILayout.Label("版本:", GUILayout.Width(100));
        GUILayout.Label("保存路径", GUILayout.Width(200));
        GUILayout.Label("大小:", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        #endregion

        #region 内容行
        GUILayout.BeginVertical();

        for (int i = 0; i < entities.Count; i++)
        {
            AssetBundleEntity entity = entities[i];
            GUILayout.BeginHorizontal("box");

            dict[entity.Num] = GUILayout.Toggle(dict[entity.Num], "", GUILayout.Width(100));
            GUILayout.Label(entity.Name, GUILayout.Width(150));
            GUILayout.Label(entity.Tag, GUILayout.Width(100));
            GUILayout.Label(entity.Version + "", GUILayout.Width(100));
            GUILayout.Label(entity.ToPath, GUILayout.Width(200));
            GUILayout.Label(entity.Size + "", GUILayout.Width(100));
            GUILayout.EndHorizontal();

            foreach (var path in entity.PathLists)
            {
                GUILayout.BeginHorizontal("box");
                //GUILayout.Space(40);
                GUILayout.Label("完整资源路径:", GUILayout.Width(100));
                GUILayout.Label(path);                             //显示ToPath路径.
                GUILayout.EndHorizontal();
            }

        }
        GUILayout.EndVertical(); 
        #endregion

    }


    
    /// <summary>
    /// 选定-标签类型
    /// </summary>
    private void OnSelectionTagCallBack()
    {
        switch (tagIndex)
        {
            //回调去实现,下拉菜单. 筛选后的结果.

            case 0:
                foreach (AssetBundleEntity e in entities)
                {
                    dict[e.Num] = true;
                }
                break;
            case 1:
                foreach (AssetBundleEntity e in entities)
                {
                    //使用 StringComparison.CurrentCulture.进行对比,高效率.
                    //详细 : http://developer.51cto.com/art/201001/175935.htm
                    dict[e.Num] = e.Tag.Equals("Scene", StringComparison.CurrentCulture);
                }
                break;
            case 2:
                foreach (AssetBundleEntity e in entities)
                {
                    //使用 StringComparison.CurrentCulture.进行对比,高效率.
                    //详细 : http://developer.51cto.com/art/201001/175935.htm
                    dict[e.Num] = e.Tag.Equals("Role", StringComparison.CurrentCulture);
                }
                break;
            case 3:
                foreach (AssetBundleEntity e in entities)
                {
                    //使用 StringComparison.CurrentCulture.进行对比,高效率.
                    //详细 : http://developer.51cto.com/art/201001/175935.htm
                    dict[e.Num] = e.Tag.Equals("Effect", StringComparison.CurrentCulture);
                }
                break;
            case 4:
                foreach (AssetBundleEntity e in entities)
                {
                    //使用 StringComparison.CurrentCulture.进行对比,高效率.
                    //详细 : http://developer.51cto.com/art/201001/175935.htm
                    dict[e.Num] = e.Tag.Equals("Audio", StringComparison.CurrentCulture);
                }
                break;
            case 5:
                foreach (AssetBundleEntity e in entities)
                {
                    //使用 StringComparison.CurrentCulture.进行对比,高效率.
                    //详细 : http://developer.51cto.com/art/201001/175935.htm
                    dict[e.Num] = e.Tag.Equals("None", StringComparison.CurrentCulture);
                }
                break;

               
        }
        Debug.LogFormat("当前选择的是{0}",arrTag[tagIndex]);
    }

    /// <summary>
    /// 选定-打包平台
    /// </summary>
    public void OnSelectTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                target = BuildTarget.Android;
                break;
            case 1:
                target = BuildTarget.iOS;
                break;
            case 2:
                target = BuildTarget.StandaloneWindows;
                break;
        }
        Debug.LogFormat("当前选择打包的平台是:{0}", arrBuildTarget[buildTargetIndex]);
    }




    /// <summary>
    /// 清空资源包
    /// </summary>
    public void OnSelectClearCallBack()
    {
        string toPath = Application.dataPath + "/../AssetBundle/" + arrBuildTarget[buildTargetIndex];
        if (System.IO.Directory.Exists(toPath))
        {
            System.IO.Directory.Delete(toPath, true); //,true 如果他里面存在子目录.也一起删除.
        }
        Debug.Log("清空对应的assetBundle包.");
    }

    /// <summary>
    /// 确定打包
    /// </summary>
    public void OnSelectAssetBundleCallBack()
    {
        //找出需要打包的对象. (加入到list中)
        List<AssetBundleEntity> lstNeedBuild = new List<AssetBundleEntity>();
        foreach (var e in entities)
        {
            //筛选出,要打包的assetBundle.
            if (dict[e.Num])
            {
                lstNeedBuild.Add(e);
            }
        }

        //正式打包.
        for (int i = 0; i < lstNeedBuild.Count; i++)
        {
            Debug.LogFormat("正在打包{0}/{1}", i+1, lstNeedBuild.Count);
            BuildAssetBundle(lstNeedBuild[i]);
        }
        Debug.Log("打包完毕!");
    }

    /// <summary>
    /// 构建打包
    /// </summary>
    /// <param name="entity"></param>
    void BuildAssetBundle(AssetBundleEntity entity)
    {
        AssetBundleBuild[] arrBuild = new AssetBundleBuild[1];
        AssetBundleBuild build = new AssetBundleBuild();

        //包名 + 后缀
        build.assetBundleName = string.Format("{0}.{1}", entity.Name, (entity.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase) ? "unity3d" : "assetbundle"));
        //资源路径
        build.assetNames = entity.PathLists.ToArray();
        //注意:  ../ 代表上一级目录
        //也就是: 
        //Application.dataPath ==  E:/Unity3D/Program/UU/MMORPG/Assets/
        //             变成:       E:/Unity3D/Program/UU/MMORPG/

        string toPath = Application.dataPath + "/../AssetBundle/" + arrBuildTarget[buildTargetIndex] + entity.ToPath;

        arrBuild[0] = build;//给数组赋值.为了在BuildPipeline调用！

        //路径不存在,创建文件夹
        if (System.IO.Directory.Exists(toPath) == false)
        {
            System.IO.Directory.CreateDirectory(toPath);
        }

        BuildPipeline.BuildAssetBundles(toPath, arrBuild, BuildAssetBundleOptions.None, target);
    }


}
