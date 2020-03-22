using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// 打包XML集合
/// </summary>
public class AssetBundleDAL
{

    private string m_path;

    private List<AssetBundleEntity> bundleEntities;

    public AssetBundleDAL(string path)
    {
        this.m_path = path;
        bundleEntities = new List<AssetBundleEntity>();
    }


    public List<AssetBundleEntity> GetList()
    {
        bundleEntities.Clear();

        XDocument xDoc = XDocument.Load(m_path);
        XElement root = xDoc.Root;

        XElement assetBundle = root.Element("AssetBundle");
        IEnumerable<XElement> entities = assetBundle.Elements("Item");

        int index = 0;
        foreach (XElement item in entities)
        {
            AssetBundleEntity entity = new AssetBundleEntity();
            entity.Num = "Key" + ++index;
            entity.Name = item.Attribute("Name").Value;
            entity.Tag = item.Attribute("Tag").Value;
            entity.Version = item.Attribute("Version").Value.ToInt();
            entity.Size = item.Attribute("Size").Value.ToLong();
            entity.ToPath = item.Attribute("ToPath").Value;

            IEnumerable<XElement> pathList = item.Elements("Path");
            foreach (XElement p in pathList)
            {
                entity.PathLists.Add(string.Format("Assets/{0}", p.Attribute("Value").Value));
            }
            bundleEntities.Add(entity);
        }
        return bundleEntities;
    }

}

