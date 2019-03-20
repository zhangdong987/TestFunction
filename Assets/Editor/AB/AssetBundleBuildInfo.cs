using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        for (int i=0;i<Assets.Count;++i)
        {
            AssetInfo info = Assets[i];
            info.Bundled = Name;
            AssetImporter importer = AssetImporter.GetAtPath(info.AssetPath);
            importer.assetBundleName = Name;
        }
    }
    public void RemoveAsset(AssetInfo assetInfo)
    {
        assetInfo.Bundled = "";
        AssetImporter importer = AssetImporter.GetAtPath(assetInfo.AssetPath);
        importer.assetBundleName = "";
        Assets.Remove(assetInfo);
    }

    public void AddAsset(AssetInfo assetInfo)
    {
        if (assetInfo.Bundled == Name)
            return;
        AssetImporter importer = AssetImporter.GetAtPath(assetInfo.AssetPath);
        importer.assetBundleName = Name;
        assetInfo.Bundled = Name;
        Assets.Add(assetInfo);
    }
}
