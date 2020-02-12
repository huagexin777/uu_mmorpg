using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ����DBModel����
/// </summary>
/// <typeparam name="T">����--Model��</typeparam>
/// <typeparam name="P">����--����Entityʵ����</typeparam>
public abstract class AbstractDBModel<T, P> : IDisposable
    //new()ָ���˴���T��ʵ��ʱӦ�þ��й��캯����
    where T : class, new()
    where P : AbstractEntity
{

    #region ����

    private static T _instace;
    public static T Instance
    {
        get
        {
            if (_instace == null)
            {
                _instace = new T();
            }
            return _instace;
        }
    }

    #endregion


    public List<P> _list;
    protected Dictionary<int, P> _dict;


    protected abstract string FileName { get; }


    public AbstractDBModel()
    {
        _list = new List<P>();
        _dict = new Dictionary<int, P>();
        //ÿ��new��ʱ�����.
        Load();
    }

    public void Load()
    {
        using (GameDataTableParser parse = new GameDataTableParser(@"E:\Unity3D\Program\UU\MMORPG\Assets\WWW\Data\"+ FileName))
        {
            while (!parse.Eof)
            {
                //��������,ȥʵ�����͹���!
                P entity = MakeEntity(parse);

                _list.Add(entity);
                _dict.Add(entity.Id, entity);
                
                parse.Next();
            }
        }
    }


    /// <summary>
    /// ����ʵ����
    /// </summary>
   protected abstract P MakeEntity(GameDataTableParser parse);



    /// <summary>
    /// �õ� ʵ����List
    /// </summary>
    public List<P> GetEntityList()
    {
        return _list;
    }

    /// <summary>
    /// ͨ��id���ҵ�ʵ����
    /// </summary>
    /// <returns></returns>
    public P TryGetEntityById(int id)
    {
        P p = null;
        _dict.TryGetValue(id, out p);
        return p;
    }

    public void Dispose()
    {

    }
}
