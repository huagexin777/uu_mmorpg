using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageDBModel : AbstractDBModel<LanguageDBModel, LanguageEntity>
{

    public LanguageType CurrentLanguage { get; set; }

    /// <summary>
    /// 根据模块和key 获取值
    /// </summary>
    /// <param name="module">模块</param>
    /// <param name="key">key</param>
    public string GetText(string module, string key)
    {
        if (_list.Count > 0 && base._list != null)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                //模块和key 都对应上
                if (_list[i].Module.Equals(module, System.StringComparison.InvariantCultureIgnoreCase) &&
                    _list[i].Key.Equals(key, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    switch (CurrentLanguage)
                    {
                        case LanguageType.EN:
                            return _list[i].EN;
                        case LanguageType.CN:
                            return _list[i].CN;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 返回所有模块
    /// </summary>
    public List<string> GetAllModules()
    {
        List<string> modules = new List<string>();
        if (_list.Count != 0 && _list != null)
        for (int i = 0; i < _list.Count; i++)
        {
            modules.Add(_list[i].Module);
        }
        return modules;
    }

    /// <summary>
    /// 返回所有Key
    /// </summary>
    public List<string> GetAllKeys()
    {
        List<string> keys = new List<string>();
        if (_list.Count != 0 && _list != null)
        for (int i = 0; i < _list.Count; i++)
        {
            keys.Add(_list[i].Key);
        }
        return keys;
    }
    /// <summary>
    /// 通过模块得到key
    /// </summary>
    public List<string> GetKeyByModule(string module)
    {
        List<string> keys = new List<string>();
        if (_list.Count != 0 && _list != null)
        for (int i = 0; i < _list.Count; i++)
        {
            //模块和key 都对应上
            if (_list[i].Module.Equals(module, System.StringComparison.InvariantCultureIgnoreCase))
            {
                keys.Add(_list[i].Key);
            }
        }
        return keys;
    }


}
