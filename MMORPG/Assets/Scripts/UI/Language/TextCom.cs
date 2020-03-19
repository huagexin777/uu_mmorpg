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

//��editģʽ,ִ��
[ExecuteInEditMode]
public class TextCom : MonoBehaviour
{
    [HideInInspector] public string Key;    //key-->ע��ģ��.�����

    [HideInInspector] public string Module; //ģ��-->ע��ģ��


    private Text Txt { get { return GetComponent<Text>(); } }

    /// <summary>
    /// ˢ��
    /// </summary>
    public void Refresh()
    {
        if (string.IsNullOrEmpty(Module) && string.IsNullOrEmpty(Key)) { return; }
        string value = LanguageDBModel.Instance.GetText(Module, Key);
        Txt.text = value;
    }

}
