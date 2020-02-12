using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Linq;

public class EditorMenu
{
    private static  AssetBundleDAL assetBundleDAL;
    private static List<AssetBundleEntity> entities;


    [MenuItem("Lin/AssetBundleCreate")]
    //创建AssetBundle文件夹.
    public static void AssetBundleCreateFile()
    {
        //@是限制 转义符的作用.
        //string path = Application.dataPath + @"\Editor\AssetBundle\AssetBundleConfig.xml";
        //assetBundleDAL = new AssetBundleDAL(path);
        //entities = assetBundleDAL.GetList();


        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent = new GUIContent("资源打包");
        win.Show();

    }



}
