using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageDBModel : AbstractDBModel<LanguageDBModel, LanguageEntity>
{

    public LanguageType CurrentLanguage { get; set; }

    /// <summary>
    /// ����ģ���key ��ȡֵ
    /// </summary>
    /// <param name="module">ģ��</param>
    /// <param name="key">key</param>
    public string GetText(string module, string key)
    {
        if (_list.Count > 0 && base._list != null)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                //ģ���key ����Ӧ��
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
    /// ��������ģ��
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
    /// ��������Key
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
    /// ͨ��ģ��õ�key
    /// </summary>
    public List<string> GetKeyByModule(string module)
    {
        List<string> keys = new List<string>();
        if (_list.Count != 0 && _list != null)
        for (int i = 0; i < _list.Count; i++)
        {
            //ģ���key ����Ӧ��
            if (_list[i].Module.Equals(module, System.StringComparison.InvariantCultureIgnoreCase))
            {
                keys.Add(_list[i].Key);
            }
        }
        return keys;
    }


}
