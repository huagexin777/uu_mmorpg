using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Menu : EditorMenu
{
    [MenuItem("LDx->工具/语言设置")]
    public static void OpenLanguage() 
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(LanguageWindowEditor));
        
    }


    [MenuItem("LDx->工具/AssetBundleCreate")]
    //创建AssetBundle文件夹.
    public static void AssetBundleCreateFile()
    {
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent = new GUIContent("资源打包");
        win.Show();
    }

}
