using UnityEngine;
using System.Collections;
using System;

//�̳��� IDisposable [��Դ�ͷ�] where T:new() [�������� ��������]
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