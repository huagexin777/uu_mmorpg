using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ����DBModel����
/// </summary>
/// <typeparam name="T">����--Model��</typeparam>
/// <typeparam name="E">����--����Entityʵ����</typeparam>
public abstract class AbstractDBModel<T, E> : IDisposable
    //new()ָ���˴���T��ʵ��ʱӦ�þ��й��캯����
    where T : class, new()
    where E : AbstractEntity
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


    /// <summary>
    /// �洢�����Ѿ�������ɵ�entityʵ����
    /// </summary>
    public List<E> _list;
    /// <summary>
    /// ��key=id;E=ʵ������ֵ�
    /// </summary>
    protected Dictionary<int, E> _dict;


    protected abstract string FileName { get; }


    public AbstractDBModel()
    {
        _list = new List<E>();
        _dict = new Dictionary<int, E>();
        //ÿ��new��ʱ�����.
        Load();
    }

    public void Load()
    {
        using (GameDataTableParser parse = new GameDataTableParser(@"E:\Unity3D\Program\uu_mmorpg\MMORPG\Assets\WWW\Data\" + FileName))
        {
            while (!parse.Eof)
            {
                //��������,ȥʵ�����͹���!
                E entity = MakeEntity(parse);

                _list.Add(entity);
                _dict.Add(entity.Id, entity);
                
                parse.Next();
            }
        }
    }


    /// <summary>
    /// ����ʵ����
    /// </summary>
   protected abstract E MakeEntity(GameDataTableParser parse);



    /// <summary>
    /// �õ� ʵ����List
    /// </summary>
    public List<E> GetEntityList()
    {
        return _list;
    }

    /// <summary>
    /// ͨ��id���ҵ�ʵ����
    /// </summary>
    /// <returns></returns>
    public E TryGetEntityById(int id)
    {
        E e = null;
        _dict.TryGetValue(id, out e);
        return e;
    }

    public void Dispose()
    {

    }
}
