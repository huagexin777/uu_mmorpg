using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EventDispatcher : Singleton<EventDispatcher>
{
    /// <summary>
    /// 协议-事件
    /// </summary>
    /// <param name="param"></param>
    public delegate void OnActionHandler(params object[] param);

    private Dictionary<ushort, List<OnActionHandler>> dic = new Dictionary<ushort, List<OnActionHandler>>();

    /// <summary>
    /// 添加监听者-协议
    /// </summary>
    public void AddEventListener(ushort protoCode, OnActionHandler onActionHandler)
    {
        if (dic.ContainsKey(protoCode))
        {
            dic[protoCode].Add(onActionHandler);
        }
        else
        {
            List<OnActionHandler> tempActions = new List<OnActionHandler>();
            tempActions.Add(onActionHandler);
            dic.Add(protoCode, tempActions);
        }
    }

    /// <summary>
    /// 移除监听者-协议
    /// </summary>
    public void RemoveEventListener(ushort protoCode, OnActionHandler onActionHandler)
    {
        if (!dic.ContainsKey(protoCode))
        {
            Debug.LogError("要移除的protoCode,在当前字典中.不存在！");
        }
        else
        {
            List<OnActionHandler> tempActions = dic[protoCode];
            tempActions.Remove(onActionHandler);
            if (tempActions.Count == 0)
            {
                dic.Remove(protoCode);
            }
        }
    }

    /// <summary>
    /// 派发-协议
    /// </summary>
    public void Dispacher(ushort protoCode, byte[] buffer)
    {
        if (!dic.ContainsKey(protoCode))
        {
            Debug.LogError("要派发的protoCode,在当前字典中.不存在！");
        }
        else
        {
            List<OnActionHandler> tempActions = dic[protoCode];
            for (int i = 0; i < tempActions.Count; i++)
            {
                if (tempActions[i] != null)
                {
                    tempActions[i].Invoke(buffer);
                }
            }
        }
    }


    ///------------------------------ btn 按钮监听 ------------------------------------------------


}
