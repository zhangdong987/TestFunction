using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一个AB包对象
public class AssetBundleBuildInfo 
{
    public string Name
    {
        get;
        set;
    }
    //AB包中的所有资源
    public List<AssetInfo> Assets
    {
        get;
        set;
    }
    public AssetBundleBuildInfo(string name)
    {
        Name = name;
        Assets = new List<AssetInfo>();
    }
    public void RenameAssetBundle(string name)
    {
        Name = name;
    }
}
