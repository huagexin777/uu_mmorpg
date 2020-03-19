using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TextCom), true)]
public class TextEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        TextCom textCom = (TextCom)target; //拿到target对象

        //(选中的)selectIndex , (初始的)index
        int selectIndex, index = 0;

        string[] modules = LanguageDBModel.Instance.GetAllModules().ToArray();

        selectIndex = LanguageDBModel.Instance.GetAllModules().IndexOf(textCom.Module);
        //模块
        index = EditorGUILayout.Popup("模块",selectIndex, modules, new GUILayoutOption[0]);//初始,modules[0]为默认.
        if (selectIndex != index)
        {
            textCom.Module = LanguageDBModel.Instance.GetAllModules()[index];//获取到index对应的[module],并赋值.

            //通知面板 值改变了
            EditorUtility.SetDirty(target);
            //通知Scene 值改变了
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            textCom.Refresh();
        }

        //Key
        //由于,上面module值已经改变.所以.这边会给key也赋值.Kyes[0]
        selectIndex = LanguageDBModel.Instance.GetKeyByModule(textCom.Module).IndexOf(textCom.Key);

        index = EditorGUILayout.Popup("Key", selectIndex, LanguageDBModel.Instance.GetKeyByModule(textCom.Module).ToArray(), new GUILayoutOption[0]);
        if (selectIndex != index)
        {
            textCom.Key = LanguageDBModel.Instance.GetKeyByModule(textCom.Module)[index];//用来给[key]初始化的

            //通知面板 值改变了
            EditorUtility.SetDirty(target);
            //通知Scene 值改变了
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            textCom.Refresh();
        }
    }

}
