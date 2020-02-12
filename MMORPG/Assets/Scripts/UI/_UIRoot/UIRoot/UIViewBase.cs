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
    
    void Awake()
    {
        Instance = this;

        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            UIEventListener.Get(btnArr[i].gameObject).onClick = OnClick;
        }

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
