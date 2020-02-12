using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AssetBundle µÃÂ
/// </summary>
public class AssetBundleEntity
{
    public string Num;
    public string Name;
    public string Tag;
    public int Version;
    public long Size;
    public string ToPath;

    public AssetBundleEntity()
    {
        pathLists = new List<string>();
    }

    private List<string> pathLists;

    public List<string> PathLists
    {
        get
        {
            return pathLists;
        }
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4}", Name, Tag, Version, Size, ToPath);
    }
}
