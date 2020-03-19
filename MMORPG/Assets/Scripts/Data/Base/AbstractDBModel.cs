using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 抽象DBModel基类
/// </summary>
/// <typeparam name="T">子类--Model类</typeparam>
/// <typeparam name="E">子类--数据Entity实体类</typeparam>
public abstract class AbstractDBModel<T, E> : IDisposable
    //new()指明了创建T的实例时应该具有构造函数。
    where T : class, new()
    where E : AbstractEntity
{

    #region 单例

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
    /// 存储所有已经加载完成的entity实体类
    /// </summary>
    public List<E> _list;
    /// <summary>
    /// 以key=id;E=实体类的字典
    /// </summary>
    protected Dictionary<int, E> _dict;


    protected abstract string FileName { get; }


    public AbstractDBModel()
    {
        _list = new List<E>();
        _dict = new Dictionary<int, E>();
        //每次new得时候加载.
        Load();
    }

    public void Load()
    {
        using (GameDataTableParser parse = new GameDataTableParser(@"E:\Unity3D\Program\uu_mmorpg\MMORPG\Assets\WWW\Data\" + FileName))
        {
            while (!parse.Eof)
            {
                //交给子类,去实例化和构建!
                E entity = MakeEntity(parse);

                _list.Add(entity);
                _dict.Add(entity.Id, entity);
                
                parse.Next();
            }
        }
    }


    /// <summary>
    /// 创建实体类
    /// </summary>
   protected abstract E MakeEntity(GameDataTableParser parse);



    /// <summary>
    /// 得到 实体类List
    /// </summary>
    public List<E> GetEntityList()
    {
        return _list;
    }

    /// <summary>
    /// 通过id查找到实体类
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
