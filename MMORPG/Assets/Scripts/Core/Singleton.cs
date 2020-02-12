using UnityEngine;
using System.Collections;
using System;

//继承自 IDisposable [资源释放] where T:new() [限制条件 引用类型]
public class Singleton<T> : IDisposable where T :new()
{
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

    public virtual void Dispose()
    {
        
    }
}