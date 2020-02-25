using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ��Դ��������
/// </summary>
public enum ResourcesType
{
    /// <summary>
    /// ����UI
    /// </summary>
    WindowsUI,
    /// <summary>
    /// ����UI_Item
    /// </summary>
    WindowsItemUI,
    /// <summary>
    /// ����UI
    /// </summary>
    SceneUI,
    /// <summary>
    /// ��ɫ_1
    /// </summary>
    Role_1,
    /// <summary>
    /// ��Ч
    /// </summary>
    Effect,

}

/// <summary>
/// ��Դ���ع���
/// </summary>
public class ResourcesMgr : Singleton<ResourcesMgr>
{
    /// <summary>
    /// ��Դ-hashTable
    /// </summary>
    private Hashtable hashtablePref = new Hashtable();


    /// <summary>
    /// ��Դ����
    /// </summary>
    /// <param name="type">��Դ����</param>
    /// <param name="path">��Դ·��</param>
    /// <param name="isCache">�Ƿ�ӻ����м���</param>
    /// <returns></returns>
    public GameObject Load(ResourcesType type, string path, bool isCache = false)
    {
        StringBuilder sb = new StringBuilder();
        switch (type)
        {
            case ResourcesType.WindowsUI:
                sb.Append("UI/UI-Window/");
                break;
            case ResourcesType.WindowsItemUI:
                sb.Append("UI/UI-Window/UI-Item/");
                break;
            case ResourcesType.SceneUI:
                sb.Append("UI/UI-Scene/");
                break;
            case ResourcesType.Role_1:
                sb.Append("Prefab/Role/");
                break;
            case ResourcesType.Effect:
                sb.Append("Prefab/Effect/");
                break;
        }
        sb.Append(path);

        GameObject obj = null;
        //�ӻ����м���
        if (isCache)
        {
            if (hashtablePref.ContainsKey(sb.ToString()))
            {
                obj = hashtablePref[sb.ToString()] as GameObject;
            }
            else
            {
                obj = Resources.Load<GameObject>(sb.ToString());
                obj = GameObject.Instantiate(obj);
                hashtablePref.Add(sb.ToString(), obj);
            }
        }
        else
        {
            obj = Resources.Load<GameObject>(sb.ToString());
            obj = GameObject.Instantiate(obj);
        }



        return obj;
    }

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        //�ͷ�,hashTable.
        hashtablePref.Clear();
        //������û���õ�����Դ (Ҳ���Ǵӻ�����ɾ��)
        Resources.UnloadUnusedAssets();
    }

}
