using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageWindowEditor : EditorWindow
{
    private List<string> lanList = new List<string>();
    
    public LanguageWindowEditor() 
    {
        lanList.Add("CN");
        lanList.Add("EN");
    }

    int selectIndex = 0;
    private void OnGUI()
    {
        //开启一个水平布局
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("语言选择");
        selectIndex = EditorGUILayout.Popup(selectIndex, lanList.ToArray(), new GUILayoutOption[0]);

        // 添加脚本
        if (GUILayout.Button("添加TextCom脚本", GUILayout.Width(200)))
        {
            //EditorApplication.delayCall = OnAddTextComScript;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box");
        // 保存
        if (GUILayout.Button("保存", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSaveLanguageChose;
        }
        EditorGUILayout.EndHorizontal();
    }


    /// <summary>
    /// 添加TextCom脚本
    /// </summary>
    void OnAddTextComScript() 
    {
        Text[] txt = FindObjectsOfType<Text>();
        for (int i = 0; i < txt.Length; i++)
        {
            if (txt[i].name != "input-Text")
            {
                txt[i].transform.gameObject.GetOrCreatComponent<TextCom>();
            }
        }
        //通知unity改变了
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 保存按钮
    /// </summary>
    private void OnSaveLanguageChose()
    {
        string language = lanList.ToArray()[selectIndex];
        LanguageDBModel.Instance.CurrentLanguage = (LanguageType)Enum.Parse(typeof(LanguageType), language);

        TextCom[] textComs = FindObjectsOfType<TextCom>();

        for (int i = 0; i < textComs.Length; i++)
        {
            textComs[i].Refresh();
        }

        //通知unity改变了
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
