using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageView : UIWindowViewBase
{
    [HideInInspector] public Text title;//标题
    [HideInInspector] public Text content;//内容

    private UIWindowCtrl.WindowType currentWindowType;
    public override UIWindowCtrl.WindowType CurrentWindowType 
    { 
        get { return UIWindowCtrl.WindowType.Message; } 
    }

    public override void OnAwake()
    {
        base.OnAwake();

        Text[] texts = GetComponentsInChildren<Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == "lblMessage")
            {
                content = texts[i];
            }
            else if (texts[i].name == "Title")
            {
                title = texts[i];
            }
        }
    }

    private void Update()
    {
      
    }

    public override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        if (go.name == "lblMessage")
        {
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIMessageView_EnsureBtn);
        }
    }

    public void UpdateMessageContent(string content) 
    {
        this.content.text = content;
    }


    public override void Close()
    {
        base.CurrentWindowType = CurrentWindowType;
        base.Close();

        this.content.text = ""; //内容重置
    }

}
