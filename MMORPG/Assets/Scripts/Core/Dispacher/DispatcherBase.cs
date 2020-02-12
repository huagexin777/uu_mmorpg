using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承自 IDisposable [资源释放] where T:new() [限制条件 引用类型]
public class DispatcherBase<T,K,P> : IDisposable 
    where T : new()
    where P : class
{
    #region 单例

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    #endregion

    public delegate void OnActionHandler(P p);

    protected Dictionary<K, List<OnActionHandler>> dict = new Dictionary<K, List<OnActionHandler>>();




    /// <summary>
    /// 添加监听
    /// </summary>
    public void AddBtnEventListener(K key, OnActionHandler onBtnActionHandler)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Add(onBtnActionHandler);
        }
        else
        {
            List<OnActionHandler> tempActions = new List<OnActionHandler>();
            tempActions.Add(onBtnActionHandler);
            dict.Add(key, tempActions);
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveBtnEventListener(K key, OnActionHandler onBtnActionHandler)
    {
        if (!dict.ContainsKey(key))
        {
            Debug.LogError("要移除的key,在当前字典中.不存在！");
        }
        else
        {
            List<OnActionHandler> tempActions = dict[key];
            tempActions.Remove(onBtnActionHandler);
            if (tempActions.Count == 0)
            {
                dict.Remove(key);
            }
        }
    }

    /// <summary>
    /// 派发监听
    /// </summary>
    public void Dispatcher(K key,P param)
    {
        if (!dict.ContainsKey(key))
        {
            Debug.LogError("要派发的key,在当前字典中.不存在！");
        }
        else
        {
            List<OnActionHandler> tempActions = dict[key];
            for (int i = 0; i < tempActions.Count; i++)
            {
                if (tempActions[i] != null)
                {
                    tempActions[i].Invoke(param);
                }
            }
        }
    }

    /// <summary>
    /// 派发监听
    /// </summary>
    public void Dispatcher(K key)
    {
        Dispatcher(key, null);
    }


    /// <summary>
    /// 释放资源
    /// </summary>
    public virtual void Dispose()
    {

    }

}
