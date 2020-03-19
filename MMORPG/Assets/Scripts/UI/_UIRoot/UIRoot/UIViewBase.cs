using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所有UI基类
/// </summary>
public class UIViewBase : MonoBehaviour
{
    public static UIViewBase Instance;


    protected List<Transform> allTransList = new List<Transform>();
    
    void Awake()
    {
        Instance = this;

        Transform[] trans = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < trans.Length; i++)
        {
            allTransList.Add(trans[i]);
        }

        //找出所有的Button组件.
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            UIEventListener.Get(btnArr[i].gameObject).onClick = OnClick;
        }

        ////找出所有的Text组件.
        //Text[] txts = GetComponentsInChildren<Text>(true);
        //for (int i = 0; i < txts.Length; i++)
        //{
        //    texts[i].gameObject.AddComponent<TextCom>();
        //}

        OnAwake();
    }
    void Start()
    {
        OnStart();
    }
    void Update()
    {
        OnUpdate();
    }

    /// ************************** Reading The Fucking Code *************************************

    void OnClick(GameObject go)
    {
        OnBtnClick(go);
    }

    /// ************************** Reading The Fucking Code *************************************


    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnBtnClick(GameObject go) { }
}
