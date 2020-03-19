using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine.UI;

public enum LanguageType
{
    EN,
    CN,
}

//在edit模式,执行
[ExecuteInEditMode]
public class TextCom : MonoBehaviour
{
    [HideInInspector] public string Key;    //key-->注册模块.子组件

    [HideInInspector] public string Module; //模块-->注册模块


    private Text Txt { get { return GetComponent<Text>(); } }

    /// <summary>
    /// 刷新
    /// </summary>
    public void Refresh()
    {
        if (string.IsNullOrEmpty(Module) && string.IsNullOrEmpty(Key)) { return; }
        string value = LanguageDBModel.Instance.GetText(Module, Key);
        Txt.text = value;
    }

}
